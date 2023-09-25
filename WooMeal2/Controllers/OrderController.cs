using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WooMeal2.Data;
using WooMeal2.Models;
using WooMeal2.Repositories;
using WooMeal2.ViewModel;

namespace WooMeal2.Controllers
{
    public class OrderController : Controller
    {
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrderRepository _orderRepository;
        private static UserManager<AppUser> _userManager;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUserRepository _userRepository;

        public OrderController(ShoppingCart shoppingCart, IOrderRepository orderRepository, 
            UserManager<AppUser> userManager, ApplicationDbContext applicationDbContext, 
            IRestaurantRepository restaurantRepository, IUserRepository userRepository)
        {
            _shoppingCart = shoppingCart;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _restaurantRepository = restaurantRepository;
            _userRepository = userRepository;
        }

        public IActionResult Checkout(decimal vegosszeg)
        {
            var items = _shoppingCart.GetShoppingCartItems();

            if (items.Count() == 0)
            {
                return RedirectToAction("EmptyCartError", "Home");
            }

            ViewData["Vegosszeg"] = vegosszeg.ToString();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutPost(OrderViewModel model)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if (items.Count() == 0)
            {
                return RedirectToAction("EmptyCartError", "Home");
            }

            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            model.OrderTotal = _shoppingCart.GetShoppingCartTotal();

            if (model.OrderTotal > user.Credit)
            {
                return RedirectToAction("CreditError", "Home");
            }
            else
            {
                var order = new Order
                {
                    Id = Guid.NewGuid().ToString(),
                    TimeOfOrder = DateTime.Now,
                    EstimatedArriveTime = DateTime.Now.AddMinutes(items.Max(x => x.Meal.MinutesToPrepare) + 10),
                    MinutesToPrepare = items.Max(x => x.Meal.MinutesToPrepare),
                    TotalCost = (int)model.OrderTotal,
                    Customer = user,
                    OwnerId = userId
                };

                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();

                _userRepository.CreditChange(-(int)model.OrderTotal, userId);

                return RedirectToAction("CheckoutComplete");
            }

            return View(model);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Köszi a rendelést!";
            return View();
        }

        public IActionResult CheckoutIncomplete()
        {
            ViewBag.CheckoutCompleteMessage = "Nem sikerült a rendelés!";
            return View();
        }
    }
}
