using WooMeal2.Models;

namespace WooMeal2.Repositories
{
    public interface IRestaurantRepository
    {
        void CreateRestaurant(Restaurant restaurant);
        Restaurant GetById(string restaurantId);
        IEnumerable<Restaurant> GetAll();
        Restaurant GetByUserId(string userId);
    }
}
