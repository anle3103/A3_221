using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SE1607_Group4_A3.Models;

namespace SE1607_Group4_A3.Pages.Albums
{
    public class IndexModel : PageModel
    {
        private readonly SE1607_Group4_A3.Models.MusicStoreContext _context;

        public IndexModel(SE1607_Group4_A3.Models.MusicStoreContext context)
        {
            _context = context;
        }

        public IList<Album> Album { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            string username = HttpContext.Session.GetString(Constant.userSessionKey);
            if (string.IsNullOrEmpty(username)) return RedirectToPage("/Login");
            int role = int.Parse(HttpContext.Session.GetString(Constant.userSessionRoleKey) ?? "0");
            if (role == 0) return RedirectToPage("/Index");
           
            if (_context.Albums != null)
            {
                Album = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Genre).ToListAsync();
            }
            return Page();
        }
    }
}
