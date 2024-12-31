using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetEngCore403.Entities
{
    public class Movie
    {
        [Key]
        public int Id { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public MovieGenre Genre { set; get; }
        public string ImageUrl { set; get; }
        public DateTime ReleaseDate { set; get; }
        public string? ImdbUrl { set; get; }

        //foreign keys
        [ForeignKey("DirectorId")]
        public int DirectorId { set; get; }
        public Director Director { set; get; }

        public List<Rating> Ratings { set; get; }

    }

    public enum MovieGenre
    {
        Comedy, Drama,
        Thriller, Fantasy
    }


}
