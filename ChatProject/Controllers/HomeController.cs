using Microsoft.AspNetCore.Mvc;

namespace ChatProject.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index() => View();
    }
}