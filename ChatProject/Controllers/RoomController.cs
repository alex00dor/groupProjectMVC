using System;
using System.Linq;
using System.Threading.Tasks;
using ChatProject.Models.Repositories;
using ChatProject.Models.ViewModels;
using ChatProject.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatProject.Controllers
{
    [Authorize]
    [Route("/Chat/")]
    public class RoomController : Controller
    {
        private IRoomRepository _repository;
        private UserManager<User> _userManager;
        private static int PAGE_SIZE = 5;

        public RoomController(IRoomRepository repository, UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [Route("")]
        [Route("Index")]
        public IActionResult Index(int page = 1)
        {
            return View(new RoomsListModel
            {
                Rooms = _repository.Rooms
                    .OrderBy(r => r.Name)
                    .Skip((page - 1) * PAGE_SIZE)
                    .Take(PAGE_SIZE),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PAGE_SIZE,
                    TotalItems = _repository.Rooms.Count()
                }
            });
        }

        [Route("Room")]
        public async Task<IActionResult> Room(int roomId)
        {
            User user = await CurrentUser;
            if (user == null)
                return Redirect("/Error");
            Room room = _repository.GetRoom(roomId, user);
            return View(new RoomModel
            {
                Room = room,
                CurrentUserId = user.Id,
                UserRooms = _repository
                    .GetAllRoomsByUser(user)
                    .OrderBy(r => r.Name)
            });
        }

        [Route("Room/Enter")]
        public async Task<IActionResult> Enter(int roomId)
        {
            User user = await CurrentUser;
            if (user == null)
                return Redirect("/Error");
            _repository.AddUserToRoom(roomId, user);
            return RedirectToAction("Room", new {roomId});
        }

        [Route("Room/Leave")]
        public async Task<IActionResult> Leave(int roomId)
        {
            User user = await CurrentUser;
            if (user == null)
                return Redirect("/Error");
            _repository.RemoveUserFromRoom(roomId, user);
            return RedirectToAction("List");
        }


        [Route("List")]
        public async Task<IActionResult> List(int page = 1)
        {
            User user = await CurrentUser;

            if (user == null)
                return Redirect("/Error");

            return View(new RoomsListModel
                {
                    Rooms = _repository
                        .GetAllRoomsByUser(user)
                        .OrderBy(r => r.Name)
                        .Skip((page - 1) * PAGE_SIZE)
                        .Take(PAGE_SIZE),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PAGE_SIZE,
                        TotalItems = _repository.GetAllRoomsByUser(user).Count()
                    }
                }
            );
        }

        [HttpGet("Room/Create")]
        public IActionResult Create()
        {
            return View(new CreateRoomModel());
        }

        [HttpPost("Room/Create")]
        public async Task<IActionResult> Create(CreateRoomModel model)
        {
            User user = await CurrentUser;
            if (ModelState.IsValid && user != null)
            {
                if (_repository.isNameUniq(model.Name))
                {
                    Room room = new Room
                    {
                        Name = model.Name,
                        Creator = user,
                        Private = false
                    };
                    _repository.CreateRoom(room, user);
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("notUniq", "Room with this name already exist.");
            }

            return View(model);
        }

        //TODO Create chat person to person
//        [HttpPost("Room/Create/Private")]
//        public async Task<IActionResult> CreatePrivate(string user)
//        {
//            User currentUser = await CurrentUser;
//            
//        }

        [Route("Room/Create/Success")]
        public IActionResult CreateSuccess()
        {
            return View();
        }

        private Task<User> CurrentUser =>
            _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
    }
}