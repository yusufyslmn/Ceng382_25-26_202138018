using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Week8LoginProject.Pages
{
    public class TableModel : PageModel
    {
        public string Username { get; set; }

        public IActionResult OnGet()
        {
            // Session ve Cookie kontrolü
            var sessionUsername = HttpContext.Session.GetString("username");
            var sessionToken = HttpContext.Session.GetString("token");
            var sessionId = HttpContext.Session.GetString("session_id");

            var cookieUsername = Request.Cookies["username"];
            var cookieToken = Request.Cookies["token"];
            var cookieSessionId = Request.Cookies["session_id"];

            if (string.IsNullOrEmpty(sessionUsername) || string.IsNullOrEmpty(sessionToken) || string.IsNullOrEmpty(sessionId)
                || string.IsNullOrEmpty(cookieUsername) || string.IsNullOrEmpty(cookieToken) || string.IsNullOrEmpty(cookieSessionId)
                || sessionUsername != cookieUsername || sessionToken != cookieToken || sessionId != cookieSessionId)
            {
                TempData["ErrorMessage"] = "You must log in to access this page.";
                return RedirectToPage("/Login");
            }

            Username = sessionUsername;
            return Page();
        }
    }
}