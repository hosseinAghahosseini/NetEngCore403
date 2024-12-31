using System.ComponentModel.DataAnnotations;

namespace NetEngCore403.Entities
{
    public enum Gender
    {
        Male,
        Female
    }

    public class Director
    {
        [Key]
        public int Id { set; get; }
        public string FullName { set; get; }
        public string ImageUrl { set; get; } = ".";
        public Gender Gender { set; get; }

        public List<Movie> Movies { set; get; }
    }

}
