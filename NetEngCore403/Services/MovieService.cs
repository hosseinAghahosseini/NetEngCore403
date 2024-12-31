using Microsoft.EntityFrameworkCore;
using NetEngCore403.Entities;
using Microsoft.EntityFrameworkCore;

namespace NetEngCore403.Services
{
    public class MovieService
    {
        private readonly AppDbContext _context;
        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        public int CreateMovie(Movie u)
        {
            _context.Movies.Add(u);
            int res = _context.SaveChanges();
            return res;
        }

        public int DeleteMovie(int MovieId)
        {
            var p = _context.Movies
              .First(c => c.Id == MovieId);
            _context.Movies.Remove(p);
            var res = _context.SaveChanges();
            return res;
        }

        public int UpdateMovie(Movie u)
        {
            _context.Movies.Update(u);
            var res = _context.SaveChanges();
            return res;
        }

        public List<Movie> ReadMovieList()
        {
            var res = _context.Movies
              .OrderBy(u => u.Title)
              .ToList();
            return res;
        }

        public Movie ReadSingleMovie(int MovieId)
        {
            var p = _context.Movies
              .First(u => u.Id == MovieId);
            return (p);
        }

        public Movie SearchMovieByTitle(string MovieTitle)
        {
            var p = _context.Movies
              .Include(c => c.Director)
              .FirstOrDefault(a => a.Title.Contains(MovieTitle) && a.ReleaseDate.Year > 2000);
            return (p);
        }

        public List<Movie> ReadTop250()
        {
            var res = 
               _context.Movies
               .Include( m => m.Ratings )
               .OrderByDescending( u => u.Ratings.Average( r => r.Score ) )
               .Take(250)
               .ToList();
            return res;
        }

        public List<Movie> ReadLatest10Years()
        {
            var res =
               _context.Movies
               .Where(m => m.ReleaseDate.Year > (DateTime.Now.Year - 10) )
               .ToList();
            return res;
        }

    }
}
