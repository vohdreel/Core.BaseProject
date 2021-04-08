using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProject.API.Models
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [Display(Name = "Usuário")]
        [RegularExpression(@"^(?=.{7,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$", ErrorMessage = "O nome de usuário pode ter:<br /> <li>Entre 7 e 20 caracteres</li> <li>Letras maiúsculas ou minúsculas</li> <li>Números</li> <li>1 ponto (.) ou 1 sublinhado (_)</li>")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]        
        [EmailAddress(ErrorMessage = "Por favor, insira um email valido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]        
        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[$*&@#])[0-9a-zA-Z$*&@#]{8,}$", ErrorMessage = "A senha deve ter:<br /> <li>No mínimo 8 caracteres</li> <li>Pelo menos 1 letra maiúscula e minúscula</li> <li>Pelo menos 1 número e 1 símbolo</li>")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [Display(Name = "Confirme a senha")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "A senha e a confirmação da senha não conferem!")]
        public string ConfirmPassword { get; set; }
    }
}
