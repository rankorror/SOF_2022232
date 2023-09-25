using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WooMeal2.Data;
using WooMeal2.Models;
using WooMeal2.Repositories;

namespace WooMeal2.Controllers
{
    public class MealController : Controller
    {
        private readonly IMealRepo repository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ShoppingCart _shoppingCart;
        private readonly IRestaurantRepository _restaurantRepository;

        BlobServiceClient serviceClient;
        BlobContainerClient containerClient;

        public MealController(IMealRepo repository, UserManager<AppUser> userManager, 
            ApplicationDbContext db, ShoppingCart shoppingCart, IRestaurantRepository restaurantRepository)
        {
            this.repository = repository;
            this._userManager = userManager;

            this._shoppingCart = shoppingCart;

            serviceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=woltcopystorage;EndpointSuffix=core.windows.net");
            containerClient = serviceClient.GetBlobContainerClient("ujphotos");
            _restaurantRepository = restaurantRepository;
        }

        public IActionResult AddMeal()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMeal(Meal m, IFormFile pictureData)
        {
            try
            {
                BlobClient blobClient = containerClient.GetBlobClient(m.Id + "_" + m.Name);
                using (var uploadFileStream = pictureData.OpenReadStream())
                {
                    await blobClient.UploadAsync(uploadFileStream, true);
                }

                blobClient.SetAccessTier(AccessTier.Cool);

                var user = await _userManager.GetUserAsync(this.User);
                var restaurant = _restaurantRepository.GetAll().FirstOrDefault(x => x.Owner.Id == user.Id);
                if (restaurant != null)
                {
                    m.Owner = restaurant;
                    m.OwnerId = restaurant.Uid;

                    var url = blobClient.Uri.AbsoluteUri;
                    m.PhotoUrl= url;

                    repository.NewMeal(m);
                }
                else
                {
                    TempData["ErrorMessage"] = "Kell lennie egy éttermednek, hogy ételt adj hozzá!";
                }
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = "Hiba lépett fel étterem hozzáadása közben.";
            }

            return RedirectToAction("AddingComplete", "Meal");
        }

        public async Task<IActionResult> MealSelection(string uid)
        {
            var restaurant = _restaurantRepository.GetAll().FirstOrDefault(x => x.Uid == uid);
            var meals = repository.GetAll().Where(x => x.OwnerId == uid);
            return View("MealSelection", meals);
        }

        public IActionResult MealSelectionBack()
        {
            var restoId = _shoppingCart.GetShoppingCartItems().FirstOrDefault().Meal.OwnerId;
            var meals = repository.GetAll().Where(x => x.OwnerId == restoId);
            return View("MealSelection", meals);
        }

        public IActionResult AddingComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Sikeresen hozzáadtad az ételt!";
            return View();
        }
    }
}
