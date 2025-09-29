using Microsoft.AspNetCore.Mvc;

namespace OnePlanPetJourney.Controllers
{
    [Route("[controller]/[action]")]
    public class AlertController : Controller
    {
       [HttpPost]
        public IActionResult Dismiss([FromQuery] string key)
        {
            HttpContext.Session.SetString("HideAlert_" + key, "true");
            return Ok();
        }
    }
}
