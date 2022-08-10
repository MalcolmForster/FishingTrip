using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static FishingTrip.Pages.Shared._Common;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

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
            if (Request.Form["addFavourite"] == "true" && User.Claims.ElementAtOrDefault(0) != null)
            {
                string favSpot = Request.Form["favSpot"];
                string user = User.Claims.ElementAtOrDefault(0).Value;
                add_Favourite(favSpot, user);
            }
            else if (Request.Form["rmvFavourite"] == "true" && User.Claims.ElementAtOrDefault(0) != null)
            {
                string favSpot = Request.Form["rmvSpot"];
                string user = User.Claims.ElementAtOrDefault(0).Value;
                remove_Favourite(favSpot, user);
            }
            else if (Request.Form["searchSpot"]=="true")
            {
                add_Search(Request.Form["newSpotInput"], User.Claims.ElementAtOrDefault(0).Value);
            }
        }

        //public void OnPostPythonForecasts(List<string> spotsToForecast)
        //{
        //    if(spotsToForecast.Count > 0)
        //    {
        //        foreach (string spot in spotsToForecast)
        //        {
        //            spot_Forecast_Script(spot);
        //        }
        //    }
        //}

        //public void
        public void OnPostRemoveSearch()
        {
            string whichSpot = Request.Form["whichSpot"];
            delSearch(whichSpot, User.Claims.ElementAtOrDefault(0).Value);
        }
    }
}