﻿using CoreFundamentals.Core;
using System.Collections.Generic;
using System.Linq;

namespace CoreFundamentals.Data
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetRestaurantsByName(string name);
        Restaurant GetById(int id);
        Restaurant Update(Restaurant updatedRestaurant);
        Restaurant Create(Restaurant newRestaurant);
        int Commit();
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

        public int Commit()
        {
            return 0;
        }

        public Restaurant Create(Restaurant newRestaurant)
        {
            newRestaurant.Id = restaurants.Max(r => r.Id) + 1;
            restaurants.Add(newRestaurant);
            return newRestaurant;
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

        public Restaurant Update(Restaurant updatedRestaurant)
        {
            var restaurant = restaurants.SingleOrDefault(r => r.Id == updatedRestaurant.Id);
            if(restaurant != null)
            {
                restaurant.Name = updatedRestaurant.Name;
                restaurant.Location = updatedRestaurant.Location;
                restaurant.Cuisine = updatedRestaurant.Cuisine; 
            }
            return restaurant;
        }
    }
}
