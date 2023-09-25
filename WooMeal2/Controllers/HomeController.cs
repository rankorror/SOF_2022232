using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using static System.Formats.Asn1.AsnWriter;
using System.Reflection.Metadata;
using System;
using Microsoft.EntityFrameworkCore;
using WooMeal2.Data;
using WooMeal2.Models;
using Azure.Storage.Blobs;
using WooMeal2.Repositories;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace WooMeal2.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUserRepository _userRepository;

        public HomeController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            ILogger<HomeController> logger, ApplicationDbContext db, IEmailSender emailSender, 
            IRestaurantRepository restaurantRepository, IUserRepository userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _restaurantRepository = restaurantRepository;
            _emailSender = emailSender;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RestaurantLister()
        {
            return View(_restaurantRepository.GetAll());
        }

        public IActionResult RestaurantMaker()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RestaurantMaker(Restaurant r, IFormFile pictureData)
        {
            using (var stream = pictureData.OpenReadStream())
            {
                byte[] buffer = new byte[pictureData.Length];
                stream.Read(buffer, 0, (int)pictureData.Length);
                string filename = r.Uid + "." + pictureData.FileName.Split('.')[1];
                r.PhotoUrl= filename;

                System.IO.File.WriteAllBytes(Path.Combine("wwwroot", "images/" + pictureData.FileName), buffer);
                r.Data= buffer;
                r.ContentType= pictureData.ContentType;
            }

            var user = await _userManager.GetUserAsync(this.User);
            r.Owner = user;

            _restaurantRepository.CreateRestaurant(r);
            _userRepository.AddRestaurantToUser(user.Id, r);

            await _emailSender.SendEmailAsync(user.Email, "Étterem hozzáadva", $"Étterem hozzáadva, ezzel a névvel: {r.Name} !");

            return RedirectToAction(nameof(Index));
        }

        public IActionResult UserList()
        {
            return View(_userRepository.GetAll());
        }

        public async Task<IActionResult> Privacy()
        {
            var principal = this.User;
            var user = await _userManager.GetUserAsync(principal);
            var role = new IdentityRole()
            {
                Name = "Admin"
            };
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult CreditError()
        {
            ViewBag.OrderingErrorMessage = "Ehhez nincs elég pénzed!";
            return View(nameof(Error));
        }
        public IActionResult EmptyCartError()
        {
            ViewBag.OrderingErrorMessage = "Üres a bevásárlókocsid. Pakolj bele valamit!";
            return View(nameof(Error));
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task<IActionResult> DelegateAdmin()
        {
            var principal = this.User;
            var user = await _userManager.GetUserAsync(principal);
            var role = new IdentityRole()
            {
                Name = "Admin"
            };
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DelegateRestaurantOwner()
        {
            var principal = this.User;
            var user = await _userManager.GetUserAsync(principal);
            var role = new IdentityRole()
            {
                Name = "RestaurantOwner"
            };
            if (!await _roleManager.RoleExistsAsync("RestaurantOwner"))
            {
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(user, "RestaurantOwner");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GrantRestaurant(string uid)
        {
            // lehet hogy itt is kéne role készítés

            AppUser user = _userManager.Users.FirstOrDefault(t => t.Id == uid);
            var role = new IdentityRole()
            {
                Name = "RestaurantOwner"
            };
            if (!await _roleManager.RoleExistsAsync("RestaurantOwner"))
            {
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(user, "RestaurantOwner");
            _userRepository.TokenChange(1, user.Id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GrantAdmin(string uid)
        {
            // lehet hogy itt is kéne role készítés

            var user = _userManager.Users.FirstOrDefault(t => t.Id == uid);
            var role = new IdentityRole()
            {
                Name = "Admin"
            };
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction(nameof(UserList));
        }

        public IActionResult GetImage(string id)
        {
            var resto = _restaurantRepository.GetAll().Where(x => x.Uid == id).FirstOrDefault();
            if (resto.ContentType.Length > 3) 
            {
                return new FileContentResult(resto.Data, resto.ContentType);
            }
            else { return BadRequest(); }
            
        }
    }
}