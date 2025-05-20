using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Week8LoginProject.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnPost()
        {
            // Session temizle
            HttpContext.Session.Clear();

            // Cookie'leri sil
            Response.Cookies.Delete("username");
            Response.Cookies.Delete("token");
            Response.Cookies.Delete("session_id");

            // Login sayfasına yönlendir
            return RedirectToPage("/Login");
        }
    }
}