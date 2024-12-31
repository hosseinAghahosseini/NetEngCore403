using NetEngCore403.Entities;

namespace NetEngCore403.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }
        //Service CRUD Functions

        public int CreateUser(User u)
        {
            try
            {
                _context.Users.Add(u);
                int res = _context.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -2;
            }
        }

        public int UpdateUser(User u)
        {
            _context.Users.Update(u);
            var res = _context.SaveChanges();
            return res;
        }


        public User ReadSingleUser(string PhoneNumber)
        {
            var p = _context.Users
              .FirstOrDefault(a => a.PhoneNumber == PhoneNumber);

            return (p);
        }


        public int DeleteUser(string PhoneNumber)
        {
            var p = _context.Users
              .First(c => c.PhoneNumber == PhoneNumber);
            _context.Users.Remove(p);
            var res = _context.SaveChanges();
            return res;
        }

        public List<User> ReadUserList()
        {
            var res = _context.Users
              .OrderBy(u => u.LastName)
              .ToList();
            return res;
        }

        public User LoginUser(string PhoneNumber, string password)
        {
            var usr = _context.Users
                .FirstOrDefault(c => c.PhoneNumber == PhoneNumber && c.Password == password);
            return usr;
        }


    }

}
