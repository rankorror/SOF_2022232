using Microsoft.EntityFrameworkCore;
using WooMeal2.Data;
using WooMeal2.Models;

namespace WooMeal2.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ShoppingCart _shoppingCart;

        public OrderRepository(ApplicationDbContext context, ShoppingCart shoppingCart)
        {
            _context = context;
            _shoppingCart = shoppingCart;
        }

        public void CreateOrder(Order order)
        {
            order.TimeOfOrder = DateTime.Now;

            _context.Add(order);

            var orderDetails = new List<OrderMeal>(_shoppingCart.ShoppingCartItems.Count());

            foreach (var item in _shoppingCart.ShoppingCartItems)
            {
                orderDetails.Add(
                    new OrderMeal
                    {
                        OrderId = order.Id,
                        MealId = item.Meal.Id,
                        Amount = item.Amount,
                        Price = item.Meal.Price,
                        Meal = item.Meal
                    });
                _context.Update(item.Meal);
            }

            _context.OrderMeals.AddRange(orderDetails);
            _context.SaveChanges();
        }

        public Order GetById(string orderId)
        {
            return GetAll().FirstOrDefault(order => order.Id == orderId);
        }

        public IEnumerable<Order> GetByUserId(string userId)
        {
            return GetAll()
                .Where(order => order.Customer.Id == userId);
        }

        public IEnumerable<Order> GetAll()
        {
            return _context.Orders
                .AsNoTracking()
                .Include(order => order.Customer);
        }

        public IEnumerable<Order> GetUserLatestOrders(int count, string userId)
        {
            return GetByUserId(userId)
                .OrderByDescending(order => order.TimeOfOrder)
                .Take(count);
        }
    }
}
