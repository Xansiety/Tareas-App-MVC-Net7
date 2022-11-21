using System.ComponentModel.DataAnnotations;

namespace TareasMVC.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo {0} no es una dirección de correo válida")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
