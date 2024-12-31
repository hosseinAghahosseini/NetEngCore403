using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NetEngCore403.Entities;
using System.ComponentModel;

namespace NetEngCore403.Models
{
    [NotMapped]
    public class UserRegisterModel
    {
        public string PhoneNumber { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public DateTime CreationDate { set; get; }
        [StringLength(128, MinimumLength = 8)]
        [PasswordPropertyText]
        public string Password { set; get; }
        public string RePassword { set; get; }
        [EmailAddress]
        public string? Email { set; get; }
        public string? Address { set; get; }
        public string? ImageUrl { set; get; }
        public DateTime? BirthDate { set; get; }
        public UserState State { set; get; }
        public IFormFile? ImageFile { set; get; }
    }

}
