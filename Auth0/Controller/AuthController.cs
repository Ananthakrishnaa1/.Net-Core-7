using Main.Service.Auth0.Contract;
using Main.Service.Auth0.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Main.Service.Auth0.Controller
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) { 
            _authService = authService;
        }
        /// <summary>
        /// Sign Up to Auth 0
        /// </summary>
        /// <param name="signUpRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SignUp")]
        public async Task<ActionResult> SignUp(SignupUserRequest signUpRequest)
        {
            var response = await _authService.SignUpAsync(signUpRequest);
            return Ok(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signInUserRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SignIn")]
        public async Task<ActionResult> SignIn(SignInUserRequest signInUserRequest)
        {
            var response = await _authService.SignInAsync(signInUserRequest);
            return Ok(response);
        }

        [HttpPost]
        [Route("SignOut")]
        public Task<ActionResult> SignOut(SignInUserRequest signInUserRequest)
        {
            _authService.SignOutAsync();
            return Task.FromResult<ActionResult>(Ok());
        }

    }
}
