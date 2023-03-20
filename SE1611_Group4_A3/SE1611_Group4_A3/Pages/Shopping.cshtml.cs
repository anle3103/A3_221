using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SE1607_Group4_A3.Models;

namespace SE1607_Group4_A3.Pages
{
    public class ShoppingModel : PageModel
    {
        private readonly MusicStoreContext _context;
        public List<SelectListItem> Genres { get; set; }
        public List<Album> Albums { get; set; }
        private const int PAGE_TAKE = 3;
        public int TotalPage { get; set; }
        public int Count { get; set; }
        [BindProperty(SupportsGet = true)]
        public new int PageNum { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Title { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? GenreId { get; set; }
        [BindProperty]
        public int AlbumId { get; set; }
        public ShoppingModel(MusicStoreContext musicStoreContext)
        {
            _context = musicStoreContext;
            Genres = new List<SelectListItem>();

            Genres = _context.Genres.Select(genre => new SelectListItem
            {
                Value = genre.GenreId.ToString(),
                Text = genre.Name,
            }).ToList();
            Genres.Add(new SelectListItem
            {
                Value = string.Empty,
                Text = "-------All------",
                Selected = true

            });
            Albums = _context.Albums.Include(a => a.Artist).Include(a => a.Genre).Skip(0).Take(3).ToList();
            Count = _context.Albums.Count();
            PageNum = 0;
            TotalPage = Count / PAGE_TAKE;
        }
        public IActionResult OnGet()
        {
            IQueryable<Album> albums = _context.Albums.AsQueryable();
            if (GenreId != null)
            {
                albums = albums.Where(a => a.GenreId == GenreId);
            }
            if (!string.IsNullOrEmpty(Title))
            {
                albums = albums.Where(a => a.Title.Equals(Title));
            }
            Count = albums.Count();
            TotalPage = Count / PAGE_TAKE;
            int skip = PageNum * PAGE_TAKE;
            if (skip > Count)
            {
                skip = TotalPage * PAGE_TAKE;
                PageNum = TotalPage;
            }
            Albums = albums.Include(a => a.Artist).Include(a => a.Genre).Skip(skip).Take(PAGE_TAKE).ToList();
            return Page();
        }
        public IActionResult OnPostAddCart()
        {
            string username = HttpContext.Session.GetString(Constant.userSessionKey) ?? string.Empty;
            if (string.IsNullOrEmpty(username)) { return RedirectToPage("Login"); }
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null) { return RedirectToPage("Login"); }
            var cart = _context.Carts.FirstOrDefault(c => c.AlbumId == AlbumId && c.CartId == username);
            if (cart != null)
            {
                cart.Count += 1;
                _context.Entry(cart).State = EntityState.Modified;
            }
            else
            {
                 cart = new()
                {
                    AlbumId = AlbumId,
                    DateCreated = DateTime.Now,
                    Count = 1,
                    CartId = user.UserName,
                };
                _context.Carts.Add(cart);
              
            }
            _context.SaveChanges();
            return RedirectToPage("Cart");
        }
    }
}
