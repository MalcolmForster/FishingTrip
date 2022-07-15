using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static FishingTrip.Pages.Shared._Common;
using Microsoft.AspNetCore.Identity;

namespace FishingTrip.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            //Console.WriteLine(Request.Form["addFavourite"]);
            if (Request.Form["addFavourite"] == "true")
            {                
                string favSpot = Request.Form["favSpot"];
                string user = User.Claims.ElementAtOrDefault(0).Value;
                add_Favourite(favSpot,user);
            } 
            else
            {
                Console.WriteLine("Something went wrong");
            }
            
        }
    }
}