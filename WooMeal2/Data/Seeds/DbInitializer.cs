using WooMeal2.Models;

namespace WooMeal2.Data.Seeds
{
    public class DbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                ApplicationDbContext context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                // RESTAURANTS

                if (!context.Restaurants.Any(x => x.Name == "McDonalds"))
                {
                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                        "mcdo.jpg");

                    Restaurant r = new Restaurant() 
                    {
                        Name = "McDonalds",
                        Description = "I'm lovin' it",
                        Address = "1039 Budapest, Ipartelep utca 1.",
                        Rating= 0,
                        MinDeliverySum = 2000,
                        DeliveryCost = 800,
                        Owner = context.Users.FirstOrDefault(x => x.Email == "mcdonalds@mcdonalds.com"),
                        OwnerId = context.Users.FirstOrDefault(x => x.Email == "mcdonalds@mcdonalds.com").Id,
                        ContentType= "image/jpeg",
                        Data= System.IO.File.ReadAllBytes(path)
                };

                    context.Users.FirstOrDefault(x => x.Email == "mcdonalds@mcdonalds.com").Restaurant = r;

                    context.Restaurants.Add(r);
                    context.SaveChanges();
                }

                if (!context.Restaurants.Any(x => x.Name == "PadThai"))
                {
                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                        "padthaifooldal.jpg");

                    Restaurant r = new Restaurant()
                    {
                        Name = "PadThai",
                        Description = "Kedvenc padthaiod végre házhoz megy!",
                        Address = "1039 Budapest, Bécsi út 53-55 fsz.3",
                        Rating = 0,
                        MinDeliverySum = 2200,
                        DeliveryCost = 700,
                        Owner = context.Users.FirstOrDefault(x => x.Email == "padthai@padthai.com"),
                        OwnerId = context.Users.FirstOrDefault(x => x.Email == "padthai@padthai.com").Id,
                        ContentType = "image/jpeg",
                        Data = System.IO.File.ReadAllBytes(path)
                    };

                    context.Users.FirstOrDefault(x => x.Email == "padthai@padthai.com").Restaurant = r;

                    context.Restaurants.Add(r);
                    context.SaveChanges();
                }

                // MEALS

                if (!context.Meals.Any(x => x.Name == "BigMac"))
                {
                    var resto = context.Restaurants.FirstOrDefault(x => x.Name == "McDonalds");

                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                        "bigmac.jpg");

                    Meal m = new Meal()
                    {
                        Name = "BigMac",
                        Description = "Hamburger marhahúspogácsa (2db), ömlesztett cheddar sajt szelet, hagyma darabok, jégsaláta, kapros savanyú uborka, Big Mac® szósz, szezámmagos zsemlében. Allergének: (glutén, tojás, tej, mustár, szezámmag)",
                        Price = 1530,
                        Type = Meal.MealType.Hamburger,
                        MinutesToPrepare = 10,
                        Owner = resto,
                        OwnerId = resto.Uid
                    };

                    context.Meals.Add(m);
                    resto.Meals.Add(m);
                    context.SaveChanges();
                }

                if (!context.Meals.Any(x => x.Name == "Dupla Sajtburger"))
                {
                    var resto = context.Restaurants.FirstOrDefault(x => x.Name == "McDonalds");

                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                        "duplasajtburger.jpg");

                    Meal m = new Meal()
                    {
                        Name = "Dupla Sajtburger",
                        Description = "2 db marhahúspogácsa zsemlében, 2 db ömlesztett cheddar sajtszelettel, savanyú uborkával, hagymával, ketchuppal és mustárral. Allergének: (glutén,     tej, mustár, szezámmag)",
                        Price = 1360,
                        Type = Meal.MealType.Hamburger,
                        MinutesToPrepare = 8,
                        Owner = resto,
                        OwnerId = resto.Uid
                    };

                    context.Meals.Add(m);
                    resto.Meals.Add(m);
                    context.SaveChanges();
                }

                if (!context.Meals.Any(x => x.Name == "Dupla Sajtos McRoyal"))
                {
                    var resto = context.Restaurants.FirstOrDefault(x => x.Name == "McDonalds");

                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                        "duplasajtosmcroyal.jpg");

                    Meal m = new Meal()
                    {
                        Name = "Dupla Sajtos McRoyal",
                        Description = "2 db marhahúspogácsa szezámmagos zsemlében, 2 db ömlesztett cheddar sajtszelettel, savanyú uborkával, hagymával, ketchuppal és mustárral. Allergének: (glutén, tej, mustár, szezámmag)",
                        Price = 2290,
                        Type = Meal.MealType.Hamburger,
                        MinutesToPrepare = 12,
                        Owner = resto,
                        OwnerId = resto.Uid
                    };

                    context.Meals.Add(m);
                    resto.Meals.Add(m);
                    context.SaveChanges();
                }

                if (!context.Meals.Any(x => x.Name == "Filet-O-Fish"))
                {
                    var resto = context.Restaurants.FirstOrDefault(x => x.Name == "McDonalds");

                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                        "filetofish.jpg");

                    Meal m = new Meal()
                    {
                        Name = "Filet-O-Fish",
                        Description = "Panírozott, darabokból formázott tengeri tőkehalfilé zsemlében, ömlesztett cheddar sajtszelettel és tartárszósszal. Allergének: (glutén, tojás, hal, tej, zeller, mustár, szezámmag)",
                        Price = 1460,
                        Type = Meal.MealType.Hamburger,
                        MinutesToPrepare = 15,
                        Owner = resto,
                        OwnerId = resto.Uid
                    };

                    context.Meals.Add(m);
                    resto.Meals.Add(m);
                    context.SaveChanges();
                }

                if (!context.Meals.Any(x => x.Name == "Chickenburger"))
                {
                    var resto = context.Restaurants.FirstOrDefault(x => x.Name == "McDonalds");

                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                        "chickenburger.jpg");

                    Meal m = new Meal()
                    {
                        Name = "Chickenburger",
                        Description = "Csirkecombfilé darabokból formázva, pácolva és panírozva, zsemlében, jégsalátával és McChicken® szósszal.   Allergének: (glutén, tojás, tej,  mustár, szezámmag)",
                        Price = 700,
                        Type = Meal.MealType.Hamburger,
                        MinutesToPrepare = 5,
                        Owner = resto,
                        OwnerId = resto.Uid
                    };

                    context.Meals.Add(m);
                    resto.Meals.Add(m);
                    context.SaveChanges();
                }

                if (!context.Meals.Any(x => x.Name == "Sajtburger"))
                {
                    var resto = context.Restaurants.FirstOrDefault(x => x.Name == "McDonalds");

                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                        "sajtburger.jpg");

                    Meal m = new Meal()
                    {
                        Name = "Sajtburger",
                        Description = "Csirkecombfilé darabokból formázva, pácolva és panírozva, zsemlében, jégsalátával és McChicken® szósszal.   Allergének: (glutén, tojás, tej,  mustár, szezámmag)",
                        Price = 710,
                        Type = Meal.MealType.Hamburger,
                        MinutesToPrepare = 5,
                        Owner = resto,
                        OwnerId = resto.Uid
                    };

                    context.Meals.Add(m);
                    resto.Meals.Add(m);
                    context.SaveChanges();
                }

                if (!context.Meals.Any(x => x.Name == "Classic Padthai"))
                {
                    var resto = context.Restaurants.FirstOrDefault(x => x.Name == "PadThai");

                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                        "classicpadthai.jpg");

                    Meal m = new Meal()
                    {
                        Name = "Classic Padthai",
                        Description = "RIZSTÉSZTA ALAP, hús feltét, zöldség mix, Thailand-padthai szósz (enyhén csípős, édes szósz a tamarind fanyarságával), koriander, pörkölt mogyoró\r\n(az alap tartalmaz: tojás, hagyma, sárgarépa, káposzta, babcsíra, alapszósz -  glutént és állati eredetű hozzávalót tartalmaz)",
                        Price = 3850,
                        Type = Meal.MealType.Plate,
                        MinutesToPrepare = 15,
                        Owner = resto,
                        OwnerId = resto.Uid
                    };

                    context.Meals.Add(m);
                    resto.Meals.Add(m);
                    context.SaveChanges();
                }

                if (!context.Meals.Any(x => x.Name == "Classic Burma"))
                {
                    var resto = context.Restaurants.FirstOrDefault(x => x.Name == "PadThai");

                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                        "classicburma.jpg");

                    Meal m = new Meal()
                    {
                        Name = "Classic Burma",
                        Description = "BARNARIZS ALAP, hús feltét, ananász, bambuszrügy, Burma-green curry szósz (legzamatosabb curry, thai citromfűvel), koriander, bazsalikom- közepesen csípős\r\n\r\n(az alap tartalmaz: tojás, hagyma, sárgarépa, káposzta, babcsíra, alapszósz -  glutént és állati eredetű hozzávalót tartalmaz)",
                        Price = 4340,
                        Type = Meal.MealType.Plate,
                        MinutesToPrepare = 17,
                        Owner = resto,
                        OwnerId = resto.Uid
                    };

                    context.Meals.Add(m);
                    resto.Meals.Add(m);
                    context.SaveChanges();
                }

                if (!context.Meals.Any(x => x.Name == "Classic Malay"))
                {
                    var resto = context.Restaurants.FirstOrDefault(x => x.Name == "PadThai");

                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot", "images",
                        "classicmalay.jpg");

                    Meal m = new Meal()
                    {
                        Name = "Classic Malay",
                        Description = "UDON TÉSZTA ALAP, hús feltét, zöldség mix, shitake gomba, Malay coconut curry (pikáns kókuszos curry a lime levél frissességével)  (az alap tartalmaz: tojás, hagyma, sárgarépa, káposzta, babcsíra, alapszósz - glutént és állati eredetű hozzávalót tartalmaz)",
                        Price = 3760,
                        Type = Meal.MealType.Plate,
                        MinutesToPrepare = 13,
                        Owner = resto,
                        OwnerId = resto.Uid
                    };

                    context.Meals.Add(m);
                    resto.Meals.Add(m);
                    context.SaveChanges();
                }
            }
        }
    }
}
