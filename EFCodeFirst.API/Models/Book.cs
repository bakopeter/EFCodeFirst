using System.Collections.Generic;

namespace EFCodeFirst.API.Models
{
    public class Book
    {
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Language { get; set; }
        public int Pages { get; set; }              // ← int, NE string!
        public string? Description { get; set; }
        public Publisher? Publisher { get; set; }
    }
}