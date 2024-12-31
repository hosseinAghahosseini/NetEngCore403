using NetEngCore403.Entities;

namespace NetEngCore403.Services
{
    public class RatingService
    {
        private readonly AppDbContext _context;
        public RatingService(AppDbContext context)
        {
            _context = context;
        }

        public int CreateRating(Rating u)
        {
            _context.Ratings.Add(u);
            int res = _context.SaveChanges();
            return res;
        }

        public int DeleteRating(int RatingId)
        {
            var p = _context.Ratings
              .First(c => c.Id == RatingId);
            _context.Ratings.Remove(p);
            var res = _context.SaveChanges();
            return res;
        }

        public int UpdateRating(Rating u)
        {
            _context.Ratings.Update(u);
            var res = _context.SaveChanges();
            return res;
        }

        public List<Rating> ReadRatingList()
        {
            var res = _context.Ratings
              .OrderByDescending(u => u.Score)
              .ToList();
            return res;
        }

        public Rating ReadSingleRating(int RatingId)
        {
            var p = _context.Ratings
              .First(u => u.Id == RatingId);
            return (p);
        }
    }
}