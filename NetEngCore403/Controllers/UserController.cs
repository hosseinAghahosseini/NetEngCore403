using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using NetEngCore403.Entities;
using NetEngCore403.Models;
using NetEngCore403.Services;
using Microsoft.AspNetCore.Authorization;

namespace NetEngCore403.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "User")]
        public IActionResult Index()
        {
            ClaimsPrincipal cp = this.User;
            string PhoneNumber = cp.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            ViewData["PhoneNumber"] = PhoneNumber;

            UserService us = new UserService(_context);
            var user = us.ReadSingleUser(PhoneNumber);
            if (user == null)
                return RedirectToAction("Login", "User", new { Msg = "No user was found." });
            ViewData["FullName"] = user.FirstName + " " + user.LastName;
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel Model)
        {
            if (Model.Password != Model.RePassword)
            {
                ViewData["Error"] = "Passwords don't Match.";
                return View();
            }
            if (Model.Password != null)
            {
                string ErrorMessage = "";
                var IsPasswordOK = ValidatePassword(Model.Password, out ErrorMessage);
                if (!IsPasswordOK)
                {
                    ViewData["Error"] = ErrorMessage;
                    return View();
                }
            }

            User u = new User
            {
                Address = Model.Address,
                BirthDate = Model.BirthDate,
                Email = Model.Email,
                FirstName = Model.FirstName,
                LastName = Model.LastName,
                PhoneNumber = Model.PhoneNumber,
                State = UserState.NormalUser,
                ImageUrl = "",
                CreationDate = DateTime.Now,
                Password = Model.Password
            };

            //image upload
            string nameOfFile = null;
            string addr = "";
            if (Model.ImageFile != null)
            {
                addr = "wwwroot/upload/users";
                var path = Path.Combine(Directory.GetCurrentDirectory(), addr, Model.ImageFile.FileName);
                nameOfFile = Model.ImageFile.FileName;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await Model.ImageFile.CopyToAsync(stream);
                }
            }
            addr = "/upload/users/";
            if (nameOfFile != null)
            {
                nameOfFile = addr + nameOfFile;
                u.ImageUrl = nameOfFile;
            }

            UserService US = new UserService(_context);
            var res = US.CreateUser(u);
            if (res > 0)
            {
                return RedirectToAction("Login", "User", new { Message = "Register successful" });
            }

            ViewData["Error"] = "An Error happened during user creation. Please try again.";
            return View();
        }

        [HttpGet]
        public IActionResult Login(string Message)
        {
            ViewData["Message"] = Message;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string phonenumber, string password)
        {
            UserService us = new UserService(_context);
            var user = us.LoginUser(phonenumber, password);
            if (user != null)
            {
                var claims = new List<Claim> {
                  new Claim(ClaimTypes.NameIdentifier, phonenumber),
                  new Claim(ClaimTypes.Role, "User"),
                };

                var claimsIdentity = new ClaimsIdentity(
                  claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { };

                await HttpContext.SignInAsync(
                  CookieAuthenticationDefaults.AuthenticationScheme,
                  new ClaimsPrincipal(claimsIdentity),
                  authProperties);

                return RedirectToAction("Index", "User"); //Goto User Homepage
            }
            return RedirectToAction("Login", "User", new { Message = "An Error happend while logging in. Please try again" });

        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult EditProfile(string Message)
        { 
            ViewData["Message"] = Message;
            UserService us = new UserService(_context);
            ClaimsPrincipal cp = this.User;
            string PhoneNumber = cp.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = us.ReadSingleUser(PhoneNumber);
            if (user == null)
                return RedirectToAction("Login", "User", new { Msg = "No user was found." });

            UserRegisterModel model = new UserRegisterModel
            {
                Address = user.Address,
                BirthDate = user.BirthDate,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageUrl = user.ImageUrl,
                PhoneNumber = user.PhoneNumber,
            };
            return View(model);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> EditProfile(UserRegisterModel Model)
        {
            UserService us = new UserService(_context);
            ClaimsPrincipal cp = this.User;
            string PhoneNumber = cp.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (PhoneNumber != Model.PhoneNumber) {
                return RedirectToAction("User", "Index", new { Message = "You don't have access to this user." });
            }
            var user = us.ReadSingleUser(PhoneNumber);
            if (user == null) {
                return RedirectToAction("User", "Index", new { Message = "No such user exists." });
            }

            //field update
            user.BirthDate = Model.BirthDate;
            user.Email = Model.Email;
            user.FirstName = Model.FirstName;
            user.LastName = Model.LastName;
            user.Address = Model.Address;

            UserRegisterModel model = new UserRegisterModel
            {
                Address = user.Address,
                BirthDate = user.BirthDate,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageUrl = user.ImageUrl,
                PhoneNumber = user.PhoneNumber,
            };

            //password update
            if (!String.IsNullOrWhiteSpace(Model.Password)) {
                if (Model.Password != Model.RePassword) {
                    ViewData["Error"] = "Passwords don't Match."; 
                    return View(model);
                }

                string ErrorMsg = "";
                if (ValidatePassword(Model.Password, out ErrorMsg)) {
                    user.Password = Model.Password;
                }
                else {
                    ViewData["Error"] = ErrorMsg;
                    return View(model);
                }
            }

            //image upload
            string nameOfFile = null;
            string addr = "";
            if (Model.ImageFile != null)
            {
                addr = "wwwroot/upload/users";
                var path = Path.Combine(Directory.GetCurrentDirectory(), addr, Model.ImageFile.FileName);
                nameOfFile = Model.ImageFile.FileName;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await Model.ImageFile.CopyToAsync(stream);
                }
            }
            addr = "/upload/users/";
            if (nameOfFile != null)
            {
                nameOfFile = addr + nameOfFile;
                user.ImageUrl = nameOfFile;
            }

            //user update
            UserService US = new UserService(_context);
            var res = US.UpdateUser(user);
            if (res > 0)
            {
                return RedirectToAction("EditProfile", "User", new { Message = "Update successful" });
            }

            ViewData["Error"] = "An Error happened during user update. Please try again.";
            return View(model);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
              CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User", new { Message = "Logout Successful" });
        }

        private bool ValidatePassword(string password, out string ErrorMessage)
        {
            var input = password;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception("Password should not be empty");
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one lower case letter.";
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one upper case letter.";
                return false;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                ErrorMessage = "Password should not be lesser than 8 or greater than 15 characters.";
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one numeric value.";
                return false;
            }

            else if (!hasSymbols.IsMatch(input))
            {
                ErrorMessage = "Password should contain at least one special case character.";
                return false;
            }
            else
            {
                return true;
            }
        }

        

    }
}

