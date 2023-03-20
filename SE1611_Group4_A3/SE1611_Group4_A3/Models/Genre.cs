﻿namespace SE1607_Group4_A3.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Albums = new HashSet<Album>();
        }

        public int GenreId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}
