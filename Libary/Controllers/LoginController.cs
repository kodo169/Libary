using Microsoft.AspNetCore.Mvc;
using MailKit;
using MimeKit;
using Libary.Data;
namespace Libary.Controllers
{
    public class LoginController : Controller
    {
        private readonly LibaryContext _data;
        public LoginController(LibaryContext data)
        {
            _data = data;
        }
        public IActionResult Index()
        {
            return View();
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
        public static string codeConfirm ;
        public IActionResult sendMail(string? nameAccount)
        {
            var data = _data.Users.SingleOrDefault(x => x.Username == nameAccount);
            if(data == null) 
            {
                TempData["Message"] = "Name Account not correct!";
                return Redirect("/forgotAcc");
            }
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Nguyên Vũ", "huynhnguyenvu@dtu.edu.vn"));
            email.To.Add(new MailboxAddress($"{data.Name}", $"{data.Email}"));
            email.Subject = "Forgot Password";
            codeConfirm = RandomString();
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
                        <p>Your code confirm is: <span class='code'>{codeConfirm}</span></p>
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
            return View();
        }

        public IActionResult changePass() 
        {
            return View();
        }
    }
}
