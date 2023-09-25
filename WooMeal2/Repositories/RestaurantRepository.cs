using WooMeal2.Data;
using WooMeal2.Models;

namespace WooMeal2.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly ApplicationDbContext _context;

        public RestaurantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return _context.Restaurants;
        }

        public Restaurant GetById(string restaurantId)
        {
            return GetAll().FirstOrDefault(resto => resto.Uid == restaurantId);
        }

        public Restaurant GetByUserId(string userId)
        {
            return GetAll().FirstOrDefault(resto => resto.OwnerId == userId);
        }
    }
}
