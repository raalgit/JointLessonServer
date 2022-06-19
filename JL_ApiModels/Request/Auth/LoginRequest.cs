using System.ComponentModel.DataAnnotations;

namespace JL_ApiModels.Request.Auth
{
    public class LoginRequest : IRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
