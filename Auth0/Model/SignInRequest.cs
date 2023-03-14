using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Main.Service.Auth0.Model
{
    public class SignInRequest
    {
        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonProperty("password")]
        public string? Password { get; set; }

        [JsonProperty("audience")]
        public string? Audience { get; set; }

        [JsonProperty("scope")]
        public string? Scope { get; set; }

        [JsonProperty("client_id")]
        public string? ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string? ClientSecretId { get; set; }

        [JsonProperty("grant_type")]
        public string? GrantType { get; set; }

        [JsonProperty("realm")]
        public string? Realm { get; set; }

    }
}
