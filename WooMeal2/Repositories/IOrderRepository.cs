using WooMeal2.Models;

namespace WooMeal2.Repositories
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
        Order GetById(string orderId);
        IEnumerable<Order> GetAll();
        IEnumerable<Order> GetByUserId(string userId);
        IEnumerable<Order> GetUserLatestOrders(int count, string userId);

    }
}
