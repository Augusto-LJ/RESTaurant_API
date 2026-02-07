using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RESTaurant_API.Utility;

namespace RESTaurant_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthTestController : Controller
{
    [HttpGet]
    [Authorize]
    public ActionResult<string> GetSomething()
    {
        return "You are an authorized user";
    }

    [HttpGet("{someValue:int}")]
    [Authorize(Roles =StaticDetails.Role_Admin)]
    public ActionResult<string> GetSomething(int someValue)
    {
        return "You are an authorized user with role of admin";
    }
}
