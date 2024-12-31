using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NetEngCore403.Entities;
using System.ComponentModel;

namespace NetEngCore403.Models
{
    [NotMapped]
    public class UserLoginModel
    {
        public string PhoneNumber { set; get; }
    
        [StringLength(128, MinimumLength = 8)]
        [PasswordPropertyText]
        public string Password { set; get; }
    }

}
