using System;
using ChatProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatProject.Controllers
{
    [Authorize]
    [Route("/Chat/")]
    public class RoomController : Controller
    {
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
        
        [Route("Room")]
        public IActionResult Room(string roomId)
        {
            if (roomId == null)
            {
                return RedirectToRoute("/Error");
            }

            return View();
        }
        
        [Route("/Room/{?roomId}/Enter")]
        public IActionResult Enter(String roomId)
        {
            return View();
        }

        [Route("List")]
        public IActionResult List()
        {
            return View();
        }

        [HttpGet("/Room/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("/Room/Create")]
        public IActionResult Create(CreateRoomModel model)
        {
            return View();
        }
    }
}