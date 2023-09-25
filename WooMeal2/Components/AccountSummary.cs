using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WooMeal2.Models;

namespace WooMeal2.Components
{
    public class AccountSummary : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountSummary(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        private AppUser _user;

        public IViewComponentResult Invoke()
        {
            GetUser().Wait();

            if (_user != null)
            {
                var model = new AccountSummaryModel
                {
                    //ImageUrl = _user.ImageUrl,
                    Name = $"{_user.FirstName} {_user.LastName}"
                };

                return View(model);
            }
            return View();
        }

        private async Task GetUser()
        {
            _user = await _userManager.FindByNameAsync(User.Identity.Name);
        }
    }
}
