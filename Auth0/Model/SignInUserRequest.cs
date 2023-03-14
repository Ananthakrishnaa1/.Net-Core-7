using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Main.Service.Auth0.Model
{
    public class SignInUserRequest
    {
        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonProperty("password")]
        public string? Password { get; set; }
    }
}
