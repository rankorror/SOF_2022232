using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WooMeal2.Models
{
    public class Order
    {
        public Order()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        public DateTime TimeOfOrder { get; set; }

        public DateTime EstimatedArriveTime { get; set; }

        public int MinutesToPrepare { get; set; }

        public int TotalCost { get; set; }

        //public virtual ICollection<Meal> Meals { get; set; }

        public AppUser Customer { get; set; }

        public string OwnerId { get; set; }

        public virtual ICollection<OrderMeal> Meals { get; set; }
    }
}
