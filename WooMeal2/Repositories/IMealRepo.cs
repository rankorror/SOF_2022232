using WooMeal2.Models;

namespace WooMeal2.Repositories
{
    public interface IMealRepo
    {
        IEnumerable<Meal> GetAll();
        Meal GetById(string id);
        void DeleteFood(string id);
        void NewMeal(Meal meal);
        void EditMeal(Meal meal);
    }
}
