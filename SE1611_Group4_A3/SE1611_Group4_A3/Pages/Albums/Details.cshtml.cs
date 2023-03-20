using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SE1607_Group4_A3.Models;

namespace SE1607_Group4_A3.Pages.Albums
{
    public class DetailsModel : PageModel
    {
        private readonly SE1607_Group4_A3.Models.MusicStoreContext _context;

        public DetailsModel(SE1607_Group4_A3.Models.MusicStoreContext context)
        {
            _context = context;
        }

      public Album Album { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string username = HttpContext.Session.GetString(Constant.userSessionKey);
            if (string.IsNullOrEmpty(username)) return RedirectToPage("/Login");
            int role = int.Parse(HttpContext.Session.GetString(Constant.userSessionRoleKey) ?? "0");
            if (role == 0) return RedirectToPage("/Index");
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var album = await _context.Albums.FirstOrDefaultAsync(m => m.AlbumId == id);
            if (album == null)
            {
                return NotFound();
            }
            else 
            {
                Album = album;
            }
            return Page();
        }
    }
}
