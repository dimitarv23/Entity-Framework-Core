namespace Blog.Controllers
{
    using System.Text;
    using System.Threading.Tasks;

    using Blog.Services.Data;
    using Blog.Web.ViewModels.ApplicationUser;

    using Microsoft.AspNetCore.Mvc;

    public class ApplicationUserController : Controller
    {
        private readonly IApplicationUserService userService;

        public ApplicationUserController(IApplicationUserService _userService)
        {
            this.userService = _userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View(new RegisterUserInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            bool usernameOrEmailTaken = 
                await this.userService.UsernameExistsAsync(model.Username) ||
                await this.userService.EmailExistsAsync(model.Email);

            if (usernameOrEmailTaken)
            {
                return this.RedirectToAction("Login");
            }

            if (model.Password != model.PasswordConfirmation)
            {
                return this.View(model);
            }

            await this.userService.CreateUserAsync(model);

            return this.RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return this.View(new LoginInputModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            string userId = await this.userService.GetIdByUsernameAsync(model.Username);
            if (userId == null)
            {
                this.ModelState.AddModelError("Username", "Invalid username!");
                return this.View(model);
            }

            bool isLoginValid = await this.userService.ValidateLoginInfoAsync(model);
            if (!isLoginValid)
            {
                this.ModelState.AddModelError("Password", "Invalid password!");
                return this.View(model);
            }

            this.HttpContext.Session.Set("userId", Encoding.UTF8.GetBytes(userId));
            return this.RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            this.HttpContext.Session.Clear();
            return this.RedirectToAction("Index", "Home");
        }
    }
}
