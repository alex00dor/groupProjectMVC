using Microsoft.AspNetCore.Mvc;

namespace ChatProject.Controllers
{
    public class ErrorController : Controller
    {
        public ViewResult Error() => View();
    }
}