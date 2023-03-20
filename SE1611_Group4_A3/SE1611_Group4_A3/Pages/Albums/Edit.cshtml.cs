using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SE1607_Group4_A3.Models;

namespace SE1607_Group4_A3.Pages.Albums
{
    public class EditModel : PageModel
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private readonly SE1607_Group4_A3.Models.MusicStoreContext _context;
        [BindProperty]
        public IFormFile? AlbumImage { get; set; }
        public EditModel(SE1607_Group4_A3.Models.MusicStoreContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _context = context; _environment = environment;
        }

        [BindProperty]
        public Album Album { get; set; } = default!;

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
            Album = album;
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "Name");
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (AlbumImage != null) Album.AlbumUrl = $"/Images/{AlbumImage.FileName}";
            if (ModelState.ContainsKey("Album.Artist"))
            {
                ModelState["Album.Artist"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Skipped;

            }
            if (ModelState.ContainsKey("Album.Genre"))
            {
                ModelState["Album.Genre"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Skipped;

            }
            if (!ModelState.IsValid)
            {
                return await OnGetAsync(Album.AlbumId);
            }
            if (AlbumImage != null)
            {
                var file = Path.Combine(_environment.WebRootPath, "Images", AlbumImage.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await AlbumImage.CopyToAsync(fileStream);
                }
            }

            _context.Attach(Album).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(Album.AlbumId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.AlbumId == id);
        }
    }
}
