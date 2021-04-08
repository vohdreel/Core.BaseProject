using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProject.API.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha atual")]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[$*&@#])[0-9a-zA-Z$*&@#]{8,}$", ErrorMessage = "A senha deve ter:<br /> <li>No mínimo 8 caracteres</li> <li>Pelo menos 1 letra maiúscula e minúscula</li> <li>Pelo menos 1 número e 1 símbolo</li>")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nova senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a confirmação da nova senha não conferem!")]
        public string ConfirmNewPassword { get; set; }
    }

}
