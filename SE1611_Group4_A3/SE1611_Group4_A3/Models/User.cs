using System.ComponentModel.DataAnnotations;

namespace SE1607_Group4_A3.Models
{
    public partial class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="UserName can be blank")]
        public string UserName { get; set; } = null!;
        [Required(ErrorMessage = "Password can be blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; } = null!;
        public int Role { get; set; }
    }
}
