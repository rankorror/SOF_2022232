using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WooMeal2.Data;
using WooMeal2.Models;
using WooMeal2.Repositories;
using WooMeal2.ViewModel;

namespace WooMeal2.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMealRepo _mealRepo;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(ApplicationDbContext db, 
            UserManager<AppUser> userManager, 
            ShoppingCart shoppingCart, IMealRepo mealRepo)
        {
            _userManager = userManager;
            _shoppingCart = shoppingCart;
            _mealRepo = mealRepo;
        }

        public IActionResult Index()
        {
            _shoppingCart.GetShoppingCartItems();

            var sCVM = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View("Index", sCVM);
        }

        [HttpGet]
        public async Task<IActionResult> AddToShoppingCart(string mealId, int? amount = 1)
        {
            var meal = _mealRepo.GetById(mealId);

            bool sameResto = _shoppingCart.AddToCart(meal, 1);

            if (sameResto)
            {
                // hozzá tudtuk adni
            }
            else
            {
                // vmi hibaüzenet kéne
            }

            var restoId = _mealRepo.GetAll().FirstOrDefault(x => x.Id == mealId).OwnerId;

            return RedirectToAction("MealSelectionBack", "Meal");
        }

        public RedirectToActionResult RemoveFromShoppingCart(string mealId) 
        {
            var selectedMeal = _mealRepo.GetAll().FirstOrDefault(p => p.Id == mealId);
            if (selectedMeal != null)
            {
                _shoppingCart.RemoveFromCart(selectedMeal);
            }
            return RedirectToAction("Index");
        }
    }
}
