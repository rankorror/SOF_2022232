using WooMeal2.Models;

namespace WooMeal2.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<AppUser> GetAll();
        AppUser GetById(string id);
        void DeleteUser(string id);

        void AddRestaurantToUser(string userId, Restaurant restaurant);

        void TokenChange(int tokenPlusMinus, string id);

        void CreditChange(int creditPlusMinus, string id);
    }
}
