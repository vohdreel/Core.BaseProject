using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Global.API.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [EmailAddress(ErrorMessage = "O email digitado não é valido!")]
        public string Email { get; set; }
        
        [Display(Name = "Senha")]
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
