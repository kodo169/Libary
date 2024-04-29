using Microsoft.AspNetCore.Mvc;
using MailKit;
using MimeKit;
using Libary.Data;
using Libary.ViewModels;
namespace Libary.Controllers
{
    public class LoginController : Controller
    {
        private readonly LibaryContext _data;
        public LoginController(LibaryContext data)
        {
            _data = data;
        }
        [Route("/indexLogin")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignIn(string? username, string? password)
        {
            var data = _data.Users.Where(p => p.Username == username && p.PasswordHash == password);
            var result = data.Select(p => new DataUser_ViewModels
            {
                id = p.UserId,
                nameAcc =p.Username,
                email =p.Email,
                role =p.Role,
                name =p.Name,
            }).ToList();
            if (result.Count == 0) 
            {
                TempData["Message"] = "Name Account or Password not correct!";
                return Redirect("/indexLogin");
            }
            Global.id_User = result[0].id;
            Global.check_login = true;
            return Redirect("/mainIndex");

        }

        [Route("/forgotAcc")]
        public IActionResult forgotAccount()
        {
            return View();
        }
        public string RandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public IActionResult sendMail(string? queryEmail)
        {
            var data = _data.Users.SingleOrDefault(x => x.Email == queryEmail);
            if(data == null) 
            {
                TempData["Message"] = "Email not correct!";
                return Redirect("/forgotAcc");
            }
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Nguyên Vũ", "huynhnguyenvu@dtu.edu.vn"));
            email.To.Add(new MailboxAddress($"{data.Name}", $"{data.Email}"));
            email.Subject = "Forgot Password";
            Global.codeConfirm  = RandomString();
            string htmlBody = $@"
            <html>
                <head>
                    <style>
                        p {{
                            color: #333;
                        }}
                        .code {{
                            color: rgb(78, 164, 220);
                            font-size: 25px;
                            margin-bottom: 20px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container-mail'>
                        <p>Your code confirm is: <span class='code'>{Global.codeConfirm}</span></p>
                    </div>
                </body>
            </html>";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlBody
            };
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);
                smtp.Authenticate("huynhnguyenvu@dtu.edu.vn", "Kodo@169");

                smtp.Send(email);
                smtp.Disconnect(true);
            }
            Global.id_User= data.UserId;
            return Redirect("/confirmCode");
        }
        [Route("/confirmCode")]
        public IActionResult confirmCodeForgot(string? _codeConfirm)
        {
            if(_codeConfirm == null)
            {
                return View();
            }
            if(_codeConfirm == Global.codeConfirm)
            {
                Global.codeConfirm = null;
                return Redirect("/ChangePass");
            }
            else
            {
                TempData["Message"] = "Code Confirm not correct!";
                return View();
            }
        }
        [Route("/ChangePass")]
        public IActionResult changePass(string? pass, string? confirmPass) 
        {
            if(pass == null)
            {
                return View();
            }
            if(pass != confirmPass)
            {
                TempData["Message"] = "Confirm Password not same Your Password!";
                return Redirect("/ChangePass");
            }
            if(pass == confirmPass)
            {
                var data = _data.Users.SingleOrDefault(x => x.UserId == Global.id_User);
                data.PasswordHash = pass;
                _data.SaveChanges();
            }
            return Redirect("/SignInWhenChangePass");
        }
        [Route("/SignInWhenChangePass")]
        public IActionResult SignInWhenChangePass() 
        {
            Global.check_login = true;
            var data = _data.Users.Where(x => x.UserId == Global.id_User);
            var result = data.Select(p => new DataUser_ViewModels
            {
                id = p.UserId,
                nameAcc = p.Username,
                email = p.Email,
                role = p.Role,
                name = p.Name,
            }).ToList();
            return Redirect("/mainIndex");
        }
        public IActionResult logout()
        {
            Global.check_login = false;
            return Redirect("/mainIndex");
        }
    }
}
