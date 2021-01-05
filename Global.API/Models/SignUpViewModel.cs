using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Global.API.Models
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [Display(Name = "Usuário")]
        [RegularExpression(@"^(?=.{8,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$", ErrorMessage = "O nome de usuário deve ter entre 8 e 20 digitos, sendo eles apenas letras, números, ponto (.) ou underline (_)")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]        
        [EmailAddress(ErrorMessage = "Por favor, insira um email valido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]        
        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[$*&@#])[0-9a-zA-Z$*&@#]{8,}$", ErrorMessage = "A senha deve ter no mínimo 8 dígitos, com pelo menos 1 maiúsculo, 1 minúsculo, 1 número e 1 símbolo!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [Display(Name = "Confirme a senha")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "A senha e a confirmação de senha devem ser iguais!")]
        public string ConfirmPassword { get; set; }
    }
}
