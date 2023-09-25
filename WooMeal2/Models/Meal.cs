using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WooMeal2.Models
{
    public class Meal
    {
        public enum MealType
        {
            Sandwich, Hamburger, Pizza, Drink, GlutenFree, LactoseFree, Dessert, Salad, Soup, Pasta, Vegetarian, Vegan, Plate, Other
        }

        public Meal()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        [Required]
        [Range(500, 100000, ErrorMessage = "minimum 500, maximum 100000")]
        public int Price { get; set; }

        [MaxLength(5000)]
        public string? Description { get; set; }

        [Required]
        [Range(0, 180, ErrorMessage = "maximum 180 perc")]
        public int MinutesToPrepare { get; set; }

        public virtual MealType Type { get; set; }

        public string? PhotoUrl { get; set; }

        public string OwnerId { get; set; }

        public Restaurant Owner { get; set; }

        public ICollection<OrderMeal> Orders { get; set; }
    }
}
