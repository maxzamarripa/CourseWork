using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreFundamentals.Core;
using CoreFundamentals.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CoreFundamentals.Pages.Restaurants
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration config;
        private readonly IRestaurantData restaurantData;

        public string Message { get; set; }
        public IEnumerable<Restaurant> Restaurants { get; set; }

        public ListModel(IConfiguration _config, 
                         IRestaurantData _restaurantData)
        {
            config = _config;
            restaurantData = _restaurantData;
        }

        public void OnGet()
        {
            Message = config["Message"];
            Restaurants = restaurantData.GetAll();
        }
    }
}
