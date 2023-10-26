using System.ComponentModel.DataAnnotations;

namespace DevOfWebApp.Models.ViewModels
{
    public class UserVM
    {
        [Required]
        public string Login { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
