using System.ComponentModel.DataAnnotations;

namespace SE1607_Group4_A3.Models
{
    public partial class Album
    {
        public Album()
        {
            Carts = new HashSet<Cart>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int AlbumId { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string? AlbumUrl { get; set; }
        public virtual Artist Artist { get; set; } = null!;
        public virtual Genre Genre { get; set; } = null!;
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
