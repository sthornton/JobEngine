using JobEngine.Web.Areas.SiteManagement.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace JobEngine.Web.Controllers
{
    public class LoginController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            return View(new LoginViewModel());
        }

        public async Task<ActionResult> Login(LoginViewModel viewModel)
        {
            await SetupDefaultUser();  

            if (ModelState.IsValid)
            {
                try
                {
                    // UNCOMMENT IN ORDER TO USE Active Directory and Asp Identity Login
                    //LdapAuthentication auth = new LdapAuthentication("LDAP://yourdomain.local");
                    //bool isAuth = auth.IsAuthenticated("ei", viewModel.Username, viewModel.Password);

                    //if (isAuth)
                    //{
                    //    ApplicationUser user = UserManager.Users.Where(x => x.UserName == viewModel.Username).FirstOrDefault();
                    //    var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                    //    ClaimsIdentity identity = signInManager.CreateUserIdentity(user);
                    //    await SignInManager.SignInAsync(user, false, true);

                    //    return RedirectToLocal(viewModel.ReturnUrl);
                    //}
                    //else // try to login using aps.net identity
                    //{
                        var aspNetLoginResult = await SignInManager.PasswordSignInAsync(viewModel.Username, viewModel.Password, viewModel.RememberMe, shouldLockout: false);
                        switch (aspNetLoginResult)
                        {
                            case SignInStatus.Failure:
                                ViewBag.Message = "Username or password is incorrect";
                                break;
                            case SignInStatus.LockedOut:
                                ViewBag.Message = "Account locked out.  Please contact the system administrator.";
                                break;
                            case SignInStatus.RequiresVerification:
                                break;
                            case SignInStatus.Success:
                                return RedirectToLocal(viewModel.ReturnUrl);  
                            default:
                                break;
                        }
                   // }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "An error occurred";
                }
            }
            return View("Index", viewModel);

          
        }

        private async Task SetupDefaultUser()
        {
            var roleManager = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            var adminRoleExist = await roleManager.RoleExistsAsync("Admin");
            if (!adminRoleExist)
            {
                var role = new ApplicationRole("Admin");
                var roleresult = await roleManager.CreateAsync(role);
            }

            var adminUser = UserManager.Users.Where(x => x.UserName.ToLower() == "admin").FirstOrDefault();
            if (adminUser == null)
            {
                var user = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com" };
                var result = await UserManager.CreateAsync(user, "Password1!#");
                await UserManager.AddToRolesAsync(1, new string[] { "Admin" });
            }       
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 3)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { area = "Dashboard" });
        }
	}
}