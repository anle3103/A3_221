namespace SE1607_Group4_A3.Models
{
    public partial class Cart
    {
        public int RecordId { get; set; }
        public string CartId { get; set; } = null!;
        public int AlbumId { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Album Album { get; set; } = null!;
    }
}
