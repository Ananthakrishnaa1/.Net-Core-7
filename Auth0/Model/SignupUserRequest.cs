using Newtonsoft.Json;
using System;

namespace Main.Service.Auth0.Model
{
    /// <summary>
    /// Represents a request to sign up a new user.
    /// </summary>
    public class SignupUserRequest : UserMaintenanceRequestBase
    {
        /// <summary>
        /// Initial password for this user.
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// Username of this user. Only valid if the connection requires a username.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }
    }
}