using WooMeal2.Data;
using WooMeal2.Models;

namespace WooMeal2.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddRestaurantToUser(string userId, Restaurant restaurant)
        {
            this.GetById(userId).Restaurant = restaurant;
            this.TokenChange(-1, userId);
            _context.Entry(this.GetById(userId)).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public void CreditChange(int creditPlusMinus, string id)
        {
            var user = this.GetById(id);
            user.Credit += creditPlusMinus;
            _context.SaveChanges();
        }

        public void DeleteUser(string id)
        {
            var user = GetById(id);
            if (user == null)
            {
                throw new ArgumentException();
            }
            _context.Remove(user);
            _context.SaveChanges();
        }

        public IEnumerable<AppUser> GetAll()
        {
            return _context.Users;
        }

        public AppUser GetById(string id)
        {
            return GetAll().FirstOrDefault(user => user.Id == id);
        }

        public void TokenChange(int tokenPlusMinusm, string id)
        {
            var user = this.GetById(id);
            user.OwnerToken += tokenPlusMinusm;
            _context.SaveChanges();
        }
    }
}
