using WooMeal2.Models;

namespace WooMeal2.ViewModel
{
    public class ShoppingCartViewModel
    {
        public ShoppingCart ShoppingCart { get; set; }

        public decimal ShoppingCartTotal { get; set; }

        public string ReturnUrl { get; set; }
    }
}
