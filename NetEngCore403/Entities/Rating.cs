using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetEngCore403.Entities
{
    public class Rating
    {
        [Key]
        public int Id { set; get; }
        public int Score { set; get; }
        public string? Review { set; get; }

        public int MovieId { set; get; }
        public Movie Movie { set; get; }


        [ForeignKey("UserPhoneNumber")]
        public string UserPhoneNumber { set; get; }
        public User User { set; get; }

    }

}
