using Microsoft.EntityFrameworkCore;
using NetEngCore403.Entities;

namespace NetEngCore403.Services
{
    public class DirectorService
    {
        private readonly AppDbContext _context;
        public DirectorService(AppDbContext context)
        {
            _context = context;
        }

        public int CreateDirector(Director u)
        {
            _context.Directors.Add(u);
            int res = _context.SaveChanges();
            return res;
        }

        public int DeleteDirector(int DirectorId)
        {
            var p = _context.Directors
              .First(c => c.Id == DirectorId);
            _context.Directors.Remove(p);
            var res = _context.SaveChanges();
            return res;
        }

        public int UpdateDirector(Director u)
        {
            _context.Directors.Update(u);
            var res = _context.SaveChanges();
            return res;
        }

        public List<Director> ReadDirectorList()
        {
            var res = _context.Directors
              .OrderBy(u => u.FullName)
              .ToList();
            return res;
        }

        public Director ReadSingleDirector(int DirectorId)
        {
            var p = _context.Directors
              .First(u => u.Id == DirectorId);
            return (p);
        }

        public List<Director> ListDirectorsWithXOrMoreMovies(int DirectorId, int X)
        {
            var p = _context.Directors
              .Include(u => u.Movies)
              .Where(u => u.Movies.Count() >= X)
              .ToList();

            return (p);
        }
    }
}
