using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HelloWorld.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorld.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //为了能够记住显示登录之前的页面，我们需要保存来路，给Action添加上 returnUrl
        [HttpGet]
        public ViewResult Login(string returnUrl="")
        {
            var model = new LoginViewModel{ ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //检查 ModelState 是否有效。如果它有效，则通过调用 SignInManager 上的 PasswordSignInAsync 来登录用户
                //该方法接受四个参数，前三个分别为用户名、密码、和是否记住我三个参数
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {

                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
 
                }
         
            }
            if(string.IsNullOrEmpty(model.ReturnUrl))
            {
                model.ReturnUrl = "";
            }

            ModelState.AddModelError("", "Invalid Login Attemp");
            return View(model);
        }

        [HttpGet]
        public ViewResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                /*
                 * 如果 ModelState 有效，则使用 UserName 来创建一个 User 的实例，并使用 _userManager 异步保存用户名和密码。
                 * 如果保存成功，则使用 _signManager 直接登录然后跳回首页
                 * 如果保存失败，则告知用户，并让用户输入正确的数据
                 */

                //如果 ModelState 有效，则使用 UserName 来创建一个 User 的实例
                //但是我们并没有把密码和传递给 User ，因为 User 并没有属性来保存明文密码，所以我们只能直接将密码传递给 Identity 框架，框架会自动哈希密码
                var user = new User { UserName = model.Username };

                //为了使用 Identity 框架保存用户数据，我们使用 UserManager 的异步方法 CreateAsync，该方法接收用户名和明文密码，然后使用这些数据创建一个新的用户
                //异步方法 CreateAsync 会返回一个结构，告诉我们是成功创建了用户还是失败了，如果失败了，会返回一些失败的原因
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //如果结果是成功的，就登录刚刚创建帐户的用户，且使用 SignInManager 为该用户签名，
                    await _signInManager.SignInAsync(user, false);
                    //最后将用户重定向回主页
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //如果结果不成功，则告诉用户为什么，将 UserManager 返回的错误结果迭代添加到 ModelState 中。然后视图中就可以使用标签助手 ( 如验证标签助手 ) 显示这些错误信息
                    foreach (var item in result.Errors)
                    {
                        //ModelState.AddModelError 方法接收一个键值对参数，第一个参数为键，第二个参数为值，如果键是空的，那么就会把所有错误都放在一起
                        ModelState.AddModelError("",item.Description);
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            //注销操作需要与 Identity 框架进行交互，告诉 Identity 框架注销当前用户
            //这个操作通过调用 _signManager.SignOutAsync 方法来完成，因为这是一个异步方法，所以需要使用 await 来修饰等待
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }



    public class RegisterViewModel
    {
        [Required,MaxLength(64)]
        public string Username { get; set; }

        //DataType.Password用于设置渲染成html所使用的表单类型，这里就是password
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }

        //Compare(nameof(Password))用于验证ConfirmPassword的值和Password一致
        [DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required, MaxLength(64)]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name="Remember Me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}