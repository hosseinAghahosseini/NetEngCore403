using Microsoft.EntityFrameworkCore;

namespace NetEngCore403.Entities
{
    public class AppDbContext : DbContext
    {
        //public DbSet<ClassName1> TableName1 { get; set; }
        //public DbSet<ClassName2> TableName2 { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Director> Directors { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }

}
