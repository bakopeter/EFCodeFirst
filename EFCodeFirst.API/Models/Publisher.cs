using System.Collections.Generic;

namespace EFCodeFirst.API.Models
{
    public class Publisher
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}