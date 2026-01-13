using Microsoft.AspNetCore.Mvc;
using RESTaurant_API.Data;
using RESTaurant_API.Models;
using RESTaurant_API.Models.DTO;
using System.Net;

namespace RESTaurant_API.Controllers
{
    [Route("api/MenuItem")]
    [ApiController]
    public class MenuItemController(ApplicationDbContext db, IWebHostEnvironment env) : Controller
    {
        private readonly ApplicationDbContext _db = db;
        private readonly ApiResponse _response = new();
        private readonly IWebHostEnvironment _env = env;

        [HttpGet]
        public IActionResult GetMenuItems()
        {
            _response.Result = _db.MenuItems.ToList();
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name="GetMenuItemById")]
        public IActionResult GetMenuItemById(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            MenuItem? menuItem = _db.MenuItems.FirstOrDefault(x => x.Id == id);
            _response.Result = menuItem;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateMenuItem([FromForm]ManuItemCreateDTO menuItem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (menuItem.File is null || menuItem.File.Length == 0)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages = ["File is required"];
                        return BadRequest(_response);
                    }

                    var imagesPath = Path.Combine(_env.WebRootPath, "images");

                    if (!Directory.Exists(imagesPath))
                        Directory.CreateDirectory(imagesPath);

                    var filePath = Path.Combine(imagesPath, menuItem.File.FileName);

                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    using var stream = new FileStream(filePath, FileMode.Create);

                    await menuItem.File.CopyToAsync(stream);

                    MenuItem newMenuItem = new()
                    {
                        Name = menuItem.Name,
                        Description = menuItem.Description,
                        Price = menuItem.Price,
                        Category = menuItem.Category,
                        SpecialTag = menuItem.SpecialTag,
                        Image = "images/" + menuItem.File.FileName
                    };

                    _db.MenuItems.Add(newMenuItem);
                    await _db.SaveChangesAsync();

                    _response.Result = menuItem;
                    _response.StatusCode = HttpStatusCode.Created;

                    return CreatedAtRoute("GetMenuItemById", new { id = newMenuItem.Id }, _response);
                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
            }

            return BadRequest(_response);
        }
    }
}
