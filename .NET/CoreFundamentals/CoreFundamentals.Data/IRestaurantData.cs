﻿using CoreFundamentals.Core;
using System.Collections.Generic;
using System.Linq;

namespace CoreFundamentals.Data
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetRestaurantsByName(string name);
        Restaurant GetById(int id);
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

        public Restaurant GetById(int id)
        {
            return restaurants.SingleOrDefault(r => r.Id == id);
        }

        public IEnumerable<Restaurant> GetRestaurantsByName(string name = null)
        {
            return from r in restaurants
                   where string.IsNullOrEmpty(name) || r.Name.StartsWith(name)
                   orderby r.Name
                   select r;
        }
    }
}
