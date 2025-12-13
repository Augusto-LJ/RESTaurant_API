using Microsoft.AspNetCore.Mvc;
using RESTaurant_API.Data;
using RESTaurant_API.Models;
using System.Net;

namespace RESTaurant_API.Controllers
{
    [Route("api/MenuItem")]
    [ApiController]
    public class MenuItemController(ApplicationDbContext db) : Controller
    {
        private readonly ApplicationDbContext _db = db;
        private readonly ApiResponse _response = new();

        [HttpGet]
        public IActionResult GetMenuItems()
        {
            _response.Result = _db.MenuItems.ToList();
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
