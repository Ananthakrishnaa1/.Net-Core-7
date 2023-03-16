using Auth0.AuthenticationApi;
using Main.Service.Auth0.Contract;
using Main.Service.Auth0.Model;
using Main.Service.Utility;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi.Models;
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



        public AuthService(IAuthenticationConnection connections, HttpClient httpClient)
        {
            BaseUri = new System.Uri(StaticConfigurationManager.AppSetting["Authentication:Domain"]);
            Client_Id = StaticConfigurationManager.AppSetting["Authentication:ClientId"];
            Connection = StaticConfigurationManager.AppSetting["Authentication:DataBase"];
            _Scope = "read";
            _Audience = StaticConfigurationManager.AppSetting["Authentication:Audience"];
            _SecretId = StaticConfigurationManager.AppSetting["Authentication:SecretId"];
            _httpClient = httpClient;
            _connections = connections;
        }

        public async Task<SignInResponse> SignInAsync(SignInUserRequest signInUserRequest, CancellationToken cancellationToken = default)
        {
            if (signInUserRequest == null)
                throw new ArgumentNullException(nameof(signInUserRequest));

           SignInRequest signInRequest = GetSignInRequest(signInUserRequest);

            return await _connections.SendAsync<SignInResponse>(
             HttpMethod.Post,
             BuildUri("oauth/token"),
             signInRequest,
             cancellationToken: cancellationToken);
        }

        private SignInRequest GetSignInRequest(SignInUserRequest signInUserRequest)
        {
            return new SignInRequest { 
                Username = signInUserRequest.Username,
                Password = signInUserRequest.Password,
                Audience = _Audience,
                Scope = _Scope,
                ClientSecretId = _SecretId,
                ClientId = Client_Id,
                GrantType = "http://auth0.com/oauth/grant-type/password-realm",
                Realm = Connection
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

                signUpRequest.ClientId = Client_Id;
                signUpRequest.Connection = Connection;

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
    }
}
