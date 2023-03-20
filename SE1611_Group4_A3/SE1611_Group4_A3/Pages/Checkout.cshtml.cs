using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SE1607_Group4_A3.Models;

namespace SE1607_Group4_A3.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly MusicStoreContext _context;
        [BindProperty]
        public Order Order { get; set; }
        public decimal Total { get; set; }
        public User user { get; set; }
        public CheckoutModel(MusicStoreContext context)
        {
            _context = context;
            Total = 0;
        }
        public IActionResult OnGet()
        {
            string username = HttpContext.Session.GetString(Constant.userSessionKey);
            if (string.IsNullOrEmpty(username)) { return RedirectToPage("Login"); }
            user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user is null) { return RedirectToPage("Login"); }


            Total = _context.Carts.Where(c => c.CartId.Equals(username)).Include(c => c.Album).Sum(c => c.Album.Price * c.Count);
            return Page();
        }
        public IActionResult OnPostOrder()
        {
            string username = HttpContext.Session.GetString(Constant.userSessionKey);
            if (string.IsNullOrEmpty(username)) { return RedirectToPage("Login"); }
            Order.OrderDetails = _context.Carts.Where(c => c.CartId.Equals(username)).Include(c => c.Album).Select(c => new OrderDetail
            {
                AlbumId = c.AlbumId,
                Quantity = c.Count,
                UnitPrice = c.Album.Price,
            }).ToList();
            Order.OrderDate = DateTime.Now;
            _context.Orders.Add(Order);
            _context.Carts.RemoveRange(_context.Carts);
            _context.SaveChanges();
            return RedirectToPage("Shopping");
        }
    }
}
