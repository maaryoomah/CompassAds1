using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CompassAds1.Models;
using System.Net.Mail;
using System.Web.Helpers;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CompassAds1.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {


        AompassAdsEntities4 db = new AompassAdsEntities4();
        ApplicationDbContext Adb = new ApplicationDbContext();


        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))

            if (Request.Cookies["Login"] != null && Session["Email"] != null) // اذا الكوكيز معمولة خلص خذ المعلومات الدخول منها ودخلو مباشرة بدون ما يقعد هو يدخل معلوماتو ...يعني اخر سطر ملوش داعي ارجعلو فيو لتعبئة معلومات الدخول
            {
               String Email = Request.Cookies["Login"].Values["Email"];
               String  Password = Request.Cookies["Login"].Values["Password"];
                return RedirectToAction("index", "Home");
            }



            // ViewBag.ReturnUrl = returnUrl;

            // return View(); // view(model)ما في داعي ارجعلو فيو فيها فورم ليدخل معلومات الدخول ...او برجعلوياها بس معبية ببياناتو مش هو يعبيهن انا برجعلو فيو مع المعلومات اللي اخدتها من الكوكيز
            // بكون عاملة  اوبجكت اسمو مودل من كلاس المودل لوج ان فيو مودل وحاطة قيم للبروبرتي الايميل والباس .. وبالفيو بعملهن فل هدول الفيلد من المودل معروف كيف من تاسكات تانية وبهيك اليوز بس عليه يضغط ع لوج ان
            //وبالفيو بكون رابطة الفيو بكلاس المودل هاد شان اعبي الفيلدز باسلوب المودل بايندنج 
            // معمول متلها بمشروع الانشورنس مودل مربوطة سترونجلي بفيو ومعبية حقولها من هاي المودل بروبرتي المودل

            //else

            return View();




        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id))) // مشان نتاكد انو ترو فيلد الكونفيرم ايميل يعني اليوز هاد عامل الكونفيرم ايميل
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = db.AspNetUsers.Single(u=>u.Email==model.Email);
            var username = user.UserName;



            if (model.RememberMe)
            {
                HttpCookie cookie = new HttpCookie("Login");
                cookie.Values.Add("Email", model.Email);
                cookie.Values.Add("Password", model.Password);
                cookie.Expires = DateTime.Now.AddDays(15);
                Response.Cookies.Add(cookie);



                Session["Email"] = model.Email;
                Session.Timeout = 15;


            }


            // var user = await UserManager.FindAsync(UserName, Password);


            // var user = await UserManager.FindAsync(UserName, Password);
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(username, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }


           


        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(FormCollection formCollection)
        {//async Task<ActionResult>
            string str = "";
            var UserName = "";
            var Name = "";
            var email = "";
            var country = "";
            var mobile = "";
            var dateofbirthday = "";
            

            foreach (var key in formCollection.AllKeys)
            {
                var value = formCollection[key];
                str += value + ",";
            }
            // ff,mar@hotmail.com,123,pal,1291995,

           

           
            string f = ",";
            char[] ch = f.ToCharArray();
            String[] splitString = str.Split(ch);
            Name = splitString[0];
            email = splitString[1];
            mobile = splitString[2];
            country = splitString[3];
            dateofbirthday = splitString[4];


            //DateTimeOffset thisDate2 = new DateTimeOffset(2011, 6, 10, 15, 24, 16,TimeSpan.Zero);
            DateTime thisDate1 = new DateTime(2011, 7, 10);

            // AspNetUsers table>>> ApplicationUser class
            var user = new ApplicationUser {Email = email,Fname1=Name,PhoneNumber=mobile,country1= country, DateBirthday1= dateofbirthday, Flag1="ADV"};

            user.UserName = Guid.NewGuid().ToString();

            var result = await UserManager.CreateAsync(user, "testTEST123@");// pasword will be hashed on the database

            if (result.Succeeded)

            {


                // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                // For more information on how to enable account confirmation and password reset please visithttps://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------

              System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                 new System.Net.Mail.MailAddress("alamermar00@gmail.com", "Compass Ads"),
                 new System.Net.Mail.MailAddress(user.Email));
                 var Token = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);//GenerateEmailConfirmationTokenAsync(user.Id);
                 m.Subject = "Email confirmation";


                 m.Body = string.Format("Dear"+" "+user.Fname1+""+ "Thank you for your registration, please click on " +
                  "the below link to comlete your registration<br> <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>",
                  user.UserName, Url.Action("ConfirmEmail", "Account",
                  new { userId = user.Id, code = Token }, Request.Url.Scheme)); //Request.Url.Scheme)  http://compass-ads.azurewebsites.net مشان تخلي الرابط تكملة ع السكيما الحالية 
                                                                                // // here send  2 parameter with the request url will recived as parameters on ConfirmEmail method not httppost  its httpget


                //<a href=\"" + callbackUrl + "\">

                m.IsBodyHtml = true;
                 System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");

                 smtp.UseDefaultCredentials = false;


                 smtp.Credentials = new System.Net.NetworkCredential("alamermar00@gmail.com", "testTEST123@");
                 smtp.EnableSsl = true;
                 smtp.Send(m);
                 return RedirectToAction("Confirm", "Account", new { Email = user.Email });   // here send parameter with the request url will recived as parameter on confirm method not httppost  its httpget
                                                                                              //  AddErrors(result);

                /*


                    <div class="form-group">
                   @Html.LabelFor(m => m.Password, new { @class = "col-md-2 control-label" })
                   <div class="col-md-10">
                       @Html.PasswordFor(m => m.Password, new { @class = "form-control" })
                   </div>
               </div>
               <div class="form-group">
                   @Html.LabelFor(m => m.ConfirmPassword, new { @class = "col-md-2 control-label" })
                   <div class="col-md-10">
                       @Html.PasswordFor(m => m.ConfirmPassword, new { @class = "form-control" })
                   </div>
               </div>    




                        */
                //------------------------------------------------------------------------------------------------------------------------------------



                /*      MailMessage msg = new MailMessage();
                      msg.To.Add(new System.Net.Mail.MailAddress(user.Email, "SomeOne"));
                      msg.From = new System.Net.Mail.MailAddress("mariam.amer83@hotmail.com", "You");
                      msg.Subject = "This is a Test Mail";
                      msg.Body = "This is a test message using Exchange OnLine";
                      msg.IsBodyHtml = true;

                      System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                      client.UseDefaultCredentials = false;
                      client.Credentials = new System.Net.NetworkCredential("mariam.amer83@hotmail.com", "testTEST123@");
                      client.Port = 587; // You can use Port 25 if 587 is blocked (mine is!)
                      client.Host = "smtp.gmail.com";
                      client.DeliveryMethod = SmtpDeliveryMethod.Network;
                      client.EnableSsl = true;

                          client.Send(msg);






                      return RedirectToAction("Confirm", "Account", new { Email = user.Email });

          */





                // return RedirectToAction("Index", "Home");

            }




            // If we got this far, something failed, redisplay form
            return RedirectToAction("Fail", "Home"); 


      }



        // GET: /Account/Confirm/data....on url
        [AllowAnonymous]
        public ActionResult Confirm(string Email)
        {
            ViewBag.Email = Email;
            return View();
        }




        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
               {


            ViewBag.userId = userId;
            //var isidexsist = db.AspNetUsers.Any(c => c.Id == userId);
            var user = db.AspNetUsers.Single(r => r.Id.ToString() == userId);
            var Email = user.Email;
            var Name = user.Fname1;
            var UserName = user.UserName;
            ViewBag.Email = Email;
            ViewBag.Name = Name;
            ViewBag.UserName = UserName;




            if (userId == null || code == null)
                {
                    return View("Error");
                }
                var result = await UserManager.ConfirmEmailAsync(userId, code); // will set the email confirmed feild on data base for this email to true
                

                return View(result.Succeeded ? "ConfirmEmail" : "Error");
            
        

            }

        //------------------------------------------------------------------------------------------------------------------------------------------

        [HttpPost]
        [AllowAnonymous]

        public  ActionResult ResetPasswordM(FormCollection formCollection)
        {

            string Name = (TempData["Data1"]).ToString();
            string UserName = (TempData["Data2"]).ToString();

            string str = "";
           
            var email = "";
            var password = "";
            var confirmpassword = "";
            


            foreach (var key in formCollection.AllKeys)
            {
                var value = formCollection[key];
                str += value + ",";
            }


            string f = ",";
            char[] ch = f.ToCharArray();
            String[] splitString = str.Split(ch);
            email = splitString[0];
            password = splitString[1];
            confirmpassword = splitString[2];



            // السطر الللي تحت هاد ننغيرو مشان نخلي الاسلوب بوست لانو غيرت الدالة فاينل ريست لاسلوب بوست 
            //  return RedirectToAction("FinalReset", "Account" , new { UserName = UserName, password= password }); // HTTPGET NOT POST WE SEND DATA WITH URL....after ( ? )

            //او لليش خلص مباشرة بكمل الشغل هون بدون دالة جديدة بس رح اخلي الدالتان فاينل ريسيت كومناتات لحتى نستفيد من الافكار 

            var user = db.AspNetUsers.Single(u => u.UserName == UserName);
            //var user = await UserManager.FindByNameAsync(UserName);
            String hashedNewPassword = UserManager.PasswordHasher.HashPassword(password);
            user.PasswordHash = hashedNewPassword;
            db.SaveChanges(); // veerrrrrry important

            return RedirectToAction("Index","Home");




        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        // [HTTPPOST] NO NEED NOT COME TO THIS METHOD DATA FEOM HTML FORM AFTER SUBMITTING IT,
        //وانما من الرابط بعد علامة ؟ في معاملان بتمررو للدالة هاي اذن اسلوب جت 
        //لو او لازم اخلي اسلوب بوست لانو غلط يظهر اليوزر نيم والباسوورد بالرابط لهذا رح اعدل عالكود تاع الدالة هاي واعمل دالة جديدة تحتها بنفس الاسم واخلي هاي كومنت
        // GET: /Account/FinalReset/data....on url
        /*  [AllowAnonymous]
          public  ActionResult  FinalReset(string UserName, string password)
          {



            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
            // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
            // return RedirectToAction("ForgotPasswordConfirmation", "Account");

            var user = db.AspNetUsers.Single(u=>u.UserName==UserName);
            //var user = await UserManager.FindByNameAsync(UserName);
           String hashedNewPassword = UserManager.PasswordHasher.HashPassword(password);
            user.PasswordHash = hashedNewPassword;
            db.SaveChanges(); // veerrrrrry important


            /*UserManager<IdentityUser> userManager =
                new UserManager<IdentityUser>(new UserStore<IdentityUser>());

            userManager.RemovePassword(user.Id);

            userManager.AddPassword(user.Id, password);*/

            /*if (user!=null) {
                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
               
                var result = await UserManager.ResetPasswordAsync(user.Id, code, password); // to resrt and hash
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            //var useranotherway = db.AspNetUsers.Single(u=>u.UserName== UserName);

            */








            //best way

            /*
             فوق لازم يكون واصلني معلومة عن اليوزر الحالي ومنها افوت عالداتا بيس اطول الايدي تاعو واكمل شغل وهيكا .... مش متل تحت بستخدم اشياء جاهزة بالاي اس بي منجر 
             var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId() or عفكرة هاي لا لانو بكون مفروض اكون عاملة اوبجكت من اليوزر الحالي بحيث يكون واصلني من الفيو اسمو متلا ومنها اوصل لل الايدي وهيكا ...اما هاي تاعت الجت هي لحالها بتندل ع اي دي اليوزر الحالي اللي عامل لوج ان من الريكوست هاد من الجهاز او المتصفح الفلاني user.Id, model.OldPassword بنوصلو من فورم بعبيه اليوزر, وهاد كمانNewPassword);
                        if (result.Succeeded)
                        {
                            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                            if (user != null)
                            {
                                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            } */












       

/*
            return RedirectToAction("Fail", "Home");
        }
        */

        //-----------------------------------------------------------------------------------------------------------


            //تغيير اسلوب الدالة اللي فوق ل بوست







     /*   // POST: /Account/FinalReset
        [HttpPost] // اذن هون لازم هاي مش متل الدالة اللي فوق
        [AllowAnonymous]
        public ActionResult FinalReset (FormCollection fc)
        {
            String str = "";

            foreach (var key in fc.AllKeys)
            {
                var value = fc[key];
                str += value + ",";
            }
           

            string f = ",";
            char[] ch = f.ToCharArray();
            String[] splitString = str.Split(ch);
            string UserName = splitString[0];
           String password = splitString[1];
          
            
            var user = db.AspNetUsers.Single(u => u.UserName == UserName);
        //var user = await UserManager.FindByNameAsync(UserName);
        String hashedNewPassword = UserManager.PasswordHasher.HashPassword(password);
        user.PasswordHash = hashedNewPassword;
            db.SaveChanges(); // veerrrrrry important

   return RedirectToAction("Fail", "Home");
    }

*/



    //-------------------------------------------------------------------------------------------------------------------------
    //
    // GET: /Account/ForgotPassword
    [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {

            //remove cookies for my login test cookies اللي انا ضفتها عاللوج ان 
            //دخلي فترة صلاحيتها راحت بحط وقت بالماضي
            if (Request.Cookies["Login"] != null && Session["Email"]!=null)

            {

                var c = new HttpCookie("Login");

                c.Expires = DateTime.Now.AddDays(-1);

                Response.Cookies.Add(c);

                Session.Remove("Email");

            }

            // for asp frame work remove session and so on 
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("About", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}