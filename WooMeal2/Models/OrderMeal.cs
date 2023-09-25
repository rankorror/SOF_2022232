namespace WooMeal2.Models
{
    public class OrderMeal
    {
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }

        public string MealId { get; set; }
        public virtual Meal Meal { get; set; }

        public int Amount { get; set; }

        public int Price { get; set; }
    }
}
