using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Week8LoginProject.Models;
using System.Text.Json;

namespace Week8LoginProject.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginInput Input { get; set; }

        public string ErrorMessage { get; set; }

        public class LoginInput
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/users.json");

            if (!System.IO.File.Exists(filePath))
            {
                ErrorMessage = "Kullanıcı verisi bulunamadı.";
                return Page();
            }

            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var users = JsonSerializer.Deserialize<List<User>>(json);

            var user = users.FirstOrDefault(u =>
                u.Username == Input.Username &&
                u.Password == Input.Password &&
                u.IsActive);

            if (user == null)
            {
                ErrorMessage = "Kullanıcı adı veya şifre hatalı.";
                return Page();
            }

            // Token oluştur
            var token = Guid.NewGuid().ToString();

            // Session'a yaz
            HttpContext.Session.SetString("username", user.Username);
            HttpContext.Session.SetString("token", token);
            HttpContext.Session.SetString("session_id", HttpContext.Session.Id);

            // Cookie ayarları
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("username", user.Username, options);
            Response.Cookies.Append("token", token, options);
            Response.Cookies.Append("session_id", HttpContext.Session.Id, options);

            // Başarılı login → yönlendirme
            return RedirectToPage("/Table");
        }
    }
}