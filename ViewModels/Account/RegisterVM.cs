using System.ComponentModel.DataAnnotations;

namespace WebApplication8.ViewModels.Account
{
    public class RegisterVM
    {
        [Required]
        [MinLength(3,ErrorMessage ="Adiniz min 3simvol olmalidir")]
        public string Name { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Soyadiniz min 3simvol olmalidir")]
        public string Surname { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password),Compare(nameof(ConfirmPassword))]
        [MinLength(8)]
        
        public string Password { get; set; }
        [Required]
        [MinLength(8)]
        public string ConfirmPassword { get; set; }
        
        
    }
}
