using System.ComponentModel.DataAnnotations;

namespace NetEngCore403.Entities
{
    public class User
    {
        [Key]
        public string PhoneNumber { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public DateTime CreationDate { set; get; }
        [StringLength(128, MinimumLength = 8)]
        public string Password { set; get; }
        [EmailAddress]
        public string? Email { set; get; }
        public string? Address { set; get; }
        public string? ImageUrl { set; get; }
        public DateTime? BirthDate { set; get; }
        public UserState State { set; get; }

        //foreig keys
        public List<Rating> Ratings { set; get; }
    }

    public enum UserState
    {
        NormalUser,
        Admin,
        Inactive
    }

}
