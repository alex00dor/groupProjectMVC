using Microsoft.AspNetCore.Mvc;

namespace ChatProject.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error")]
        public ViewResult Error() => View();
    }
}