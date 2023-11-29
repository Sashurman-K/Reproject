using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Reproject.Models.Requests
{
    [Serializable]
    public class UserRequest
    {
        [Required]
        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("username")]
        public string UserName { get; set; }
    }
}
