using Microsoft.AspNetCore.Identity;

namespace WooMeal2.Models
{
    public enum UserType
    {
        Customer, Restaurant
    }

    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            this.Orders = new HashSet<Order>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Credit { get; set; }

        public string Address { get; set; }

        public UserType Type { get; set; }

        public string ContentType { get; set; }

        public byte[] Data { get; set; }

        public ICollection<Order> Orders { get; set; }

        public Restaurant Restaurant { get; set; }

        public int OwnerToken { get; set; }
    }
}
