using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using WooMeal2.Data;

namespace WooMeal2.Models
{
    public class ShoppingCart
    {
        private readonly ApplicationDbContext _appDbContext;

        public ShoppingCart(ApplicationDbContext applicationDbContext)
        {
            this._appDbContext = applicationDbContext;
        }

        public string ShoppingCartId { get; set; }

        public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; }

        //[NotMapped]
        //public Restaurant Resto { get; set; }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = services.GetService<ApplicationDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);
            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public bool AddToCart(Meal meal, int amount)
        {
            var cartItems = this.GetShoppingCartItems();

            if (cartItems.Count() == 0 || cartItems.FirstOrDefault().Meal.OwnerId == meal.OwnerId)
            {
                var shoppingCartItem =
                _appDbContext.ShoppingCartItems.SingleOrDefault(
                    s => s.Meal.Id == meal.Id && s.ShoppingCartId == ShoppingCartId);

                if (shoppingCartItem == null)
                {
                    shoppingCartItem = new ShoppingCartItem
                    {
                        ShoppingCartId = ShoppingCartId,
                        Meal = meal,
                        Amount = 1
                    };

                    _appDbContext.ShoppingCartItems.Add(shoppingCartItem);
                }
                else
                {
                    shoppingCartItem.Amount++;
                }
                _appDbContext.SaveChanges();

                return true;
            }

            return false;
                
        }

        public int RemoveFromCart(Meal meal)
        {
            var shoppingCartItem =
                _appDbContext.ShoppingCartItems.SingleOrDefault(
                    s => s.Meal.Id == meal.Id && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _appDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            _appDbContext.SaveChanges();

            return localAmount;
        }

        public IEnumerable<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??
                   (ShoppingCartItems = _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == this.ShoppingCartId).Include(s => s.Meal));
        }

        public void ClearCart()
        {
            var cartItems = _appDbContext
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            _appDbContext.ShoppingCartItems.RemoveRange(cartItems);

            _appDbContext.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Meal.Price * c.Amount).Sum();

            if (total != 0)
            {
                var restoId = _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId).Select(x => x.Meal.OwnerId).FirstOrDefault();
                total += _appDbContext.Restaurants.Where(x => x.Uid == restoId).FirstOrDefault().DeliveryCost;
            }            

            return total;
        }
    }
}
