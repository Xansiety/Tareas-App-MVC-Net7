using System.ComponentModel.DataAnnotations;

namespace TareasMVC.Models
{
    public class LoginViewModel : RegistroViewModel
    {
         
        [Display(Name = "Recuerdame")]
        public bool Recuerdame { get; set; }
    }
}
