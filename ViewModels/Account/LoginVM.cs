using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace WebApplication8.ViewModels.Account
{
    public class LoginVM
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string UserNameOrEmail { get; set; }

        [DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
