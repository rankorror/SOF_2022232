using Microsoft.EntityFrameworkCore;
using WooMeal2.Data;
using WooMeal2.Models;

namespace WooMeal2.Repositories
{
    public class MealRepository : IMealRepo
    {
        private readonly ApplicationDbContext _context;

        public MealRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void DeleteFood(string id)
        {
            var meal = GetById(id);
            if (meal == null)
            {
                throw new ArgumentException();
            }
            _context.Remove(meal);
            _context.SaveChanges();
        }

        public void EditMeal(Meal meal)
        {
            var model = _context.Meals.First(f => f.Id == meal.Id);
            _context.Entry<Meal>(model).State = EntityState.Detached;
            _context.Update(meal);
            _context.SaveChanges();
        }

        public Meal GetById(string id)
        {
            return GetAll().FirstOrDefault(food => food.Id == id);
        }

        public IEnumerable<Meal> GetAll()
        {
            return _context.Meals;
        }

        public void NewMeal(Meal meal)
        {
            _context.Meals.Add(meal);
            _context.Restaurants.FirstOrDefault(x => x.Uid == meal.Owner.Uid).Meals.Add(meal);
            _context.SaveChanges();
        }
    }
}
