using System.ComponentModel.DataAnnotations;

namespace SE1607_Group4_A3.Models.DTO
{
    public class UserDTO
    {
        [Required(ErrorMessage = "UserName can be blank")]
        public string UserName { get; set; } = null!;
        [Required(ErrorMessage = "Password can be blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
