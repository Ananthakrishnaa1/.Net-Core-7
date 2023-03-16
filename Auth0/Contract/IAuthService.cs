using Main.Service.Auth0.Model;

namespace Main.Service.Auth0.Contract
{
    public interface IAuthService
    {
        Uri BaseUri { get; }
        String Client_Id { get; }
        String Connection { get; }
        public Task<SignupUserResponse> SignUpAsync(SignupUserRequest SignupUserRequest, CancellationToken cancellationToken = default);
        public Task<SignInResponse> SignInAsync(SignInRequest signInRequest);
        public void SignOutAsync();
    }
}
