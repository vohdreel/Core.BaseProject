using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Global.API.Models
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
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[$*&@#])[0-9a-zA-Z$*&@#]{8,}$", ErrorMessage = "A senha deve ter no mínimo 8 dígitos, com pelo menos 1 maiúsculo, 1 minúsculo, 1 número e 1 símbolo!")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nova senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a confirmação da nova senha devem ser iguais!")]
        public string ConfirmNewPassword { get; set; }
    }
}
