using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace JwtLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAdminController : ControllerBase
    {


        // Sadece admin rolüne sahip olanlar bu endpoint'e erişebilir
        [HttpGet("adminpanel")]
        [Authorize(Roles = "Admin")]  // Sadece Admin rolüne sahip kullanıcılar erişebilir
        public IActionResult AdminPanel()
        {
            return Ok("Welcome to the admin panel!");
        }
    }
}