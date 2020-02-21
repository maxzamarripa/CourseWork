using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreFundamentals.Core;
using CoreFundamentals.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreFundamentals
{
    public class EditModel : PageModel
    {
        private readonly IRestaurantData _restaurantData;
        private readonly IHtmlHelper _htmlHelper;

        [BindProperty]
        public Restaurant Restaurant { get; set; }
        public IEnumerable<SelectListItem> Cuisines { get; set; }

        public EditModel(IRestaurantData restaurantData,
            IHtmlHelper htmlHelper)
        {
            _restaurantData = restaurantData;
            _htmlHelper = htmlHelper;
        }

        public IActionResult OnGet(int? restaurantId)
        {
            Cuisines = _htmlHelper.GetEnumSelectList<CuisineType>();
            if (restaurantId.HasValue)
            {
                Restaurant = _restaurantData.GetById(restaurantId.Value);

                if (Restaurant == null)
                {
                    return RedirectToPage("./NotFound");
                }
            } 
            else
            {
                Restaurant = new Restaurant();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            
            if (!ModelState.IsValid)
            {
                Cuisines = _htmlHelper.GetEnumSelectList<CuisineType>();
                return Page();
            }

            //Post re-direct patern
            if (Restaurant.Id > 0)
            {
                TempData["Message"] = "Restaurant saved!";
                _restaurantData.Update(Restaurant);
            } 
            else
            {
                TempData["Message"] = "Restaurant added!";
                _restaurantData.Create(Restaurant);
            }
            _restaurantData.Commit();
            
            return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id });
        }
    }
}