using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task_Management_System.Models;

namespace Task_Management_System.Controllers
{
    [ApiController]
    [Route("api/UserController")]
 
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /User/Register
        
        
        // POST: /User/Register
        [HttpPost("register")]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByNameAsync(model.Username);

                if(existingUser!=null)
                {
                    return BadRequest("User already Exist");
                }

                ApplicationUser user = new ApplicationUser()
                {
                    UserName = model.Username
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok("User created successfully !");
     
                }
                else
                {
                    return BadRequest("User already Exist");
                }
            }
            return BadRequest("Invalid Model");
        }

        // POST: /User/Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                var result = await _signInManager.PasswordSignInAsync(user, model.Password,false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Ok("Login Successfull");
                }
                    
                return BadRequest("Invalid login attempt.");
            }

            return BadRequest("Invalid Model");
        }


        [HttpPost("logout")]
        [Authorize] // Adding the [Authorize] attribute to ensure the user is authenticated
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logout successful.");
        }
    }
}
