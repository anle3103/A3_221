using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1607_Group4_A3.Models;
using SE1607_Group4_A3.Models.DTO;
using System.Text;

namespace SE1607_Group4_A3.Pages
{
    public class LoginModel : PageModel
    {
        private readonly MusicStoreContext _context;
        [BindProperty]
        public UserDTO User { get; set; }
        public LoginModel(MusicStoreContext musicStoreContext)
        {
            _context = musicStoreContext;
        }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) { return Page(); }
            if (User == null) { return Page(); }
            User u = null;
            if (!_context.Users.Any(u => u.UserName.Equals(User.UserName) && u.Password.Equals(User.Password)))
            {
                ModelState.AddModelError("LoginFailed", "Invalid username or password");
                return Page();
            }
            u = _context.Users.FirstOrDefault(u => u.UserName.Equals(User.UserName) && u.Password.Equals(User.Password));
            HttpContext.Session.Set(Constant.userSessionRoleKey, Encoding.UTF8.GetBytes(u.Role.ToString()));
            HttpContext.Session.Set(Constant.userSessionKey, Encoding.UTF8.GetBytes(User.UserName.ToString()));
            return RedirectToPage("Index");
        }
        public void OnGetLogout()
        {
            if (HttpContext.Session.Keys.Contains(Constant.userSessionKey))
            {
                HttpContext.Session.Remove(Constant.userSessionKey);
            }
        }
    }
}
