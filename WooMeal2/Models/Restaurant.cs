using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WooMeal2.Models
{
    public class Restaurant
    {
        public Restaurant()
        {
            Uid= Guid.NewGuid().ToString();
        }

        [Key]
        public string Uid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Helyette lekérem majd mindenhol az ide tartozó ételeket és abból listázok type-okat
        /// </summary>
        // public List<string> Types { get; set; }

        public float? Rating { get; set; }

        [Range(500, int.MaxValue, ErrorMessage = "Minimum 500")]
        [Required]
        public int MinDeliverySum { get; set; }

        [Range(300, 2000, ErrorMessage = "Minimum 300, maximum 2000")]
        [Required]
        public int DeliveryCost { get; set; }

        public string? PhotoUrl { get; set; }

        public string? ContentType { get; set; }

        public byte[]? Data { get; set; }

        public virtual ICollection<Meal> Meals{ get; set; }

        public virtual AppUser Owner { get; set; }

        public string OwnerId { get; set; }
    }
}
