using Auth0.AuthenticationApi;
using Main.Service.Auth0.Contract;
using Main.Service.Auth0.Model;
using Main.Service.MongoDB;
using Main.Service.Utility;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using RabbitMQ.Client;
using System.Net.Mime;
using System.Text;
using System.Threading;

namespace Main.Service.Auth0.Repository
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        protected readonly IAuthenticationConnection _connections;
        public Uri BaseUri { get; private set; }

        public String? Client_Id { get; private set; }

        public String? Connection { get; private set; }
        public String? _Audience { get; private set; }
        public String? _Scope { get; private set; }
        public String? _SecretId { get; private set; }
        public IMongoDbService _mongoDbService { get; private set;}

        UserMaintenanceRequestBase _maintenanceRequest = new UserMaintenanceRequestBase();


        public AuthService(IAuthenticationConnection connections, HttpClient httpClient,IMongoDbService mongoDbService)
        {
            BaseUri = new System.Uri(StaticConfigurationManager.AppSetting["Authentication:Domain"]);
            _maintenanceRequest.ClientId = StaticConfigurationManager.AppSetting["Authentication:ClientId"];
            _maintenanceRequest.Connection = StaticConfigurationManager.AppSetting["Authentication:DataBase"];
            // _Scope = "read";
            _Scope = "offline_access";
            _Audience = StaticConfigurationManager.AppSetting["Authentication:Audience"];
            _SecretId = StaticConfigurationManager.AppSetting["Authentication:SecretId"];
            _httpClient = httpClient;
            _connections = connections;
            _mongoDbService = mongoDbService;
        }

        public async Task<SignInResponse> SignInAsync(SignInUserRequest signInUserRequest, CancellationToken cancellationToken = default)
        {
            if (signInUserRequest == null)
                throw new ArgumentNullException(nameof(signInUserRequest));

           SignInRequest signInRequest = GetSignInRequest(signInUserRequest);

            SignInResponse signInResponse = await _connections.SendAsync<SignInResponse>(
             HttpMethod.Post,
             BuildUri("oauth/token"),
             signInRequest,
             cancellationToken: cancellationToken);

            await SetSesssion(signInRequest, signInResponse);

            return signInResponse;
        }

        public async Task SetSesssion(SignInRequest signInRequest, SignInResponse signInResponse)
        {
            SessionItem sessionItem = new SessionItem
            {
                Id = ObjectId.GenerateNewId(),
                Username = signInRequest.Username,
                AccessToken = signInResponse.AccessToken,
                ExpiresIn = signInResponse.ExpiresIn,
            };
            await _mongoDbService.InsertSession(sessionItem);
        }

        private SignInRequest GetSignInRequest(SignInUserRequest signInUserRequest)
        {
            return new SignInRequest { 
                Username = signInUserRequest.Username,
                Password = signInUserRequest.Password,
                Audience = _Audience,
                Scope = _Scope,
                ClientSecretId = _SecretId,
                ClientId = _maintenanceRequest.ClientId,
                GrantType = "http://auth0.com/oauth/grant-type/password-realm",
                Realm = _maintenanceRequest.Connection,
            };
        }

        public void SignOutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<SignupUserResponse> SignUpAsync(SignupUserRequest signUpRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                if (signUpRequest == null)
                    throw new ArgumentNullException(nameof(signUpRequest));

                signUpRequest.ClientId = _maintenanceRequest.ClientId;
                signUpRequest.Connection = _maintenanceRequest.Connection;

                return await _connections.SendAsync<SignupUserResponse>(
                 HttpMethod.Post,
                 BuildUri("dbconnections/signup"),
                 signUpRequest,
                 cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        private Uri BuildUri(string path)
        {
            return Utils.BuildUri(BaseUri.AbsoluteUri, path, null, null);
        }

        public async Task<string> ChangePasswordAsync(String EmailId, CancellationToken cancellationToken = default)
        {
            if (EmailId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(EmailId));

            UserMaintenanceRequestBase userMaintenanceRequestBase = _maintenanceRequest;
            userMaintenanceRequestBase.Email = EmailId;

            return await _connections.SendAsync<String>(
             HttpMethod.Post,
             BuildUri("dbconnections/change_password"),
             userMaintenanceRequestBase,
             cancellationToken: cancellationToken);
        }

        public async Task<SignInResponse> RefreshTokenAsync(string refreshTokenRequest, CancellationToken cancellationToken = default)
        {
            if (refreshTokenRequest == null)
                throw new ArgumentNullException(nameof(refreshTokenRequest));

            RefreshTokenRequest request = new RefreshTokenRequest
            {
                GrantType = "refresh_token",
                ClientId = _maintenanceRequest?.ClientId,
                ClientSecretId = _SecretId,
                RefereshToken = refreshTokenRequest,
            };

            return await _connections.SendAsync<SignInResponse>(
             HttpMethod.Post,
             BuildUri("oauth/token"),
             request,
             cancellationToken: cancellationToken);
        }
    }
}
