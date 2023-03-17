using Newtonsoft.Json;

namespace Main.Service.Auth0.Model
{
    public class RefreshTokenRequest
    {
        [JsonProperty("client_id")]
        public string? ClientId { get; set; }

        [JsonProperty("grant_type")]
        public string? GrantType { get; set; }

        [JsonProperty("client_secret")]
        public string? ClientSecretId { get; set; }

        [JsonProperty("refresh_token")]
        public string? RefereshToken { get; set; }
    }
}
