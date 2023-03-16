using Main.Service.Auth0.Model;

namespace Main.Service.Auth0.Contract
{
    public interface IAuthService
    {
        Uri BaseUri { get; }
        String Client_Id { get; }
        String Connection { get; }
        public Task<SignupUserResponse> SignUpAsync(SignupUserRequest SignupUserRequest, CancellationToken cancellationToken = default);
        public Task<SignInResponse> SignInAsync(SignInUserRequest signInUserRequest, CancellationToken cancellationToken = default);
        public void SignOutAsync();
        public Task<string> ChangePasswordAsync(String EmailId, CancellationToken cancellationToken = default);
    }
}
