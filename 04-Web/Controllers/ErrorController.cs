using Microsoft.AspNetCore.Mvc;

namespace _04_Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HandleError(int statusCode)
        {
            return statusCode switch
            {
                404 => View("404"),
                403 => View("403"),
                _ => View("500")
            };
        }
    }
}
