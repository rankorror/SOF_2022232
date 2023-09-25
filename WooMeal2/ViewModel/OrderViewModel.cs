using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace WooMeal2.ViewModel
{
    public class OrderViewModel
    {
        [BindNever]
        [ScaffoldColumn(false)]
        public decimal OrderTotal { get; set; }

        public string UserId { get; set; }
        public string UserFullName { get; set; }
    }
}
