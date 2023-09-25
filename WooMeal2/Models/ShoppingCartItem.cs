namespace WooMeal2.Models
{
    public class ShoppingCartItem
    {
        public ShoppingCartItem()
        {
            ShoppingCartItemId = Guid.NewGuid().ToString();
        }

        public string ShoppingCartItemId { get; set; }

        public Meal Meal { get; set; }

        public int Amount { get; set; }

        public string ShoppingCartId { get; set; }
    }
}
