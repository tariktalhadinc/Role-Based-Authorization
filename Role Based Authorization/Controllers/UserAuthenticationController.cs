using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Role_Based_Authorization.Models.DTO;
using Role_Based_Authorization.Repositories.Abstract;

namespace Role_Based_Authorization.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _authservice;
        public UserAuthenticationController(IUserAuthenticationService authservice)
        {
            this._authservice = authservice;
        }
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _authservice.LoginAsync(model);
            if (result.StatusCode == 1)
            {
                return RedirectToAction("Display", "Dashboard");
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }

        public IActionResult Registration() 
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid) { return View(model); }
            model.Role = "user";
            var result = await this._authservice.RegistrationAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Registration));
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this._authservice.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        //public async Task<IActionResult> Reg()
        //{
        //    var model = new RegistrationModel
        //    {
        //        Username = "admin2",
        //        Name = "Dinç",
        //        Email = "dinc@gmail.com",
        //        Password = "Admin@12345#"
        //    };
        //    model.Role = "admin";
        //    var result = await _authservice.RegistrationAsync(model);
        //    return Ok(result);
        //}
    }
}
