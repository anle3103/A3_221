using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SE1607_Group4_A3.Models;

namespace SE1607_Group4_A3.Pages
{
    public class CartModel : PageModel
    {
        private readonly MusicStoreContext _context;
        public List<Cart> Carts { get; set; }
        public decimal GrandTotal { get; set; }
        public CartModel(MusicStoreContext context)
        {
            _context = context;
            Carts = new List<Cart>();
            GrandTotal = 0;

        }
        public IActionResult OnGet()
        {
            string username = HttpContext.Session.GetString(Constant.userSessionKey) ?? string.Empty;
            if (string.IsNullOrEmpty(username)) { return RedirectToPage("Login"); }
            Carts = _context.Carts.Include(c => c.Album).Where(c => c.CartId.Equals(username)).ToList();
            GrandTotal = Carts.Sum(c => c.Album.Price * c.Count);
            return Page();
        }
        public void OnGetRemove(int id)
        {
            var cart = _context.Carts.Find(id);
            if (cart != null)
            {
                if (cart.Count > 1)
                {
                    cart.Count -= 1;
                    _context.Carts.Update(cart);
                }
                else
                {
                    _context.Carts.Remove(cart);
                }
            }
            _context.SaveChanges();
            OnGet();
        }
        public void OnGetClickTitle(int id)
        {
            var cart = _context.Carts.Find(id);
            if (cart != null)
            {
                cart.Count += 1;
                _context.Carts.Update(cart);
            }
            _context.SaveChanges();
            OnGet();
        }
    }
}
