using CoreFundamentals.Core;
using System.Collections.Generic;
using System.Linq;

namespace CoreFundamentals.Data
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetAll();
    }

    public class InMemoryRestaurantData : IRestaurantData
    {
        readonly List<Restaurant> restaurants;

        public InMemoryRestaurantData()
        {
            restaurants = new List<Restaurant>()
            {
                new Restaurant
                {
                    Id = 1,
                    Name = "Max's Pizza",
                    Location = "Ensenada",
                    Cuisine = CuisineType.Italian
                }, 
                new Restaurant
                {
                    Id = 2,
                    Name = "El Chile",
                    Location = "Tijuana",
                    Cuisine = CuisineType.Mexican
                }, 
                new Restaurant
                {
                    Id = 3,
                    Name = "Hot Curry",
                    Location = "Mexicali",
                    Cuisine = CuisineType.Indian
                }
            };
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return from r in restaurants
                   orderby r.Name
                   select r;
        }
    }
}
