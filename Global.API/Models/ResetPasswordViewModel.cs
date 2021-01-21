using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Global.API.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[$*&@#])[0-9a-zA-Z$*&@#]{8,}$", ErrorMessage = "A senha deve ter:<br /> <li>No mínimo 8 caracteres</li> <li>Pelo menos 1 letra maiúscula e minúscula</li> <li>Pelo menos 1 número e 1 símbolo</li>")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a senha")]
        [Compare("Password", ErrorMessage = "A senha e a confirmação da senha não conferem!")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}
