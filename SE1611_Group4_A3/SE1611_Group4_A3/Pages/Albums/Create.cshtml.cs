using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SE1607_Group4_A3.Models;

namespace SE1607_Group4_A3.Pages.Albums
{
    public class CreateModel : PageModel
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private readonly SE1607_Group4_A3.Models.MusicStoreContext _context;
        [BindProperty]
        public IFormFile AlbumImage { get; set; }
        public CreateModel(SE1607_Group4_A3.Models.MusicStoreContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }

        public IActionResult OnGet()
        {
            string username = HttpContext.Session.GetString(Constant.userSessionKey);
            if (string.IsNullOrEmpty(username)) return RedirectToPage("/Login");
            int role = int.Parse(HttpContext.Session.GetString(Constant.userSessionRoleKey) ?? "0");
            if (role == 0) return RedirectToPage("/Index");
            ViewData["ArtistId"] = new SelectList(_context.Artists, "ArtistId", "Name");
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "Name");
            return Page();
        }

        [BindProperty]
        public Album Album { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Album.AlbumUrl = $"/Images/{AlbumImage.FileName}";
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
                return OnGet();
            }
            var file = Path.Combine(_environment.WebRootPath, "Images", AlbumImage.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await AlbumImage.CopyToAsync(fileStream);
            }

            _context.Albums.Add(Album);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
