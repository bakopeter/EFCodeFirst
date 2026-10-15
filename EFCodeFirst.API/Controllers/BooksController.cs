using Microsoft.AspNetCore.Mvc;
using EFCodeFirst.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCodeFirst.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = _context.Books.Select(b => new BookDto
            {
                ISBN = b.ISBN,
                Title = b.Title,
                Author = b.Author,
                Language = b.Language,
                Pages = b.Pages,
                Description = b.Description,
                PublisherID = b.Publisher.ID,
                PublisherName = b.Publisher.Name
            }).ToList();

            return Ok(books);
        }

        // GET: api/books/{isbn}
        [HttpGet("{isbn}")]
        public async Task<ActionResult<BookDto>> GetBook(string isbn)
        {
            var book = _context.Books.FirstOrDefault(b => b.ISBN == isbn);

            if (book == null)
                return NotFound(new { message = "Könyv nem található" });

            var bookDto = new BookDto
            {
                ISBN = book.ISBN,
                Title = book.Title,
                Author = book.Author,
                Language = book.Language,
                Pages = book.Pages,
                Description = book.Description,
                PublisherID = book.Publisher.ID,
                PublisherName = book.Publisher.Name
            };

            return Ok(bookDto);
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto createBookDto)
        {
            var publisher = _context.Publishers.FirstOrDefault(p => p.ID == createBookDto.PublisherID);
            if (publisher == null)
                return BadRequest(new { message = "Kiadó nem található" });

            var book = new Book
            {
                ISBN = createBookDto.ISBN,
                Title = createBookDto.Title,
                Author = createBookDto.Author,
                Language = createBookDto.Language,
                Pages = createBookDto.Pages,
                Description = createBookDto.Description,
                Publisher = publisher
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var bookDto = new BookDto
            {
                ISBN = book.ISBN,
                Title = book.Title,
                Author = book.Author,
                Language = book.Language,
                Pages = book.Pages,
                Description = book.Description,
                PublisherID = book.Publisher.ID,
                PublisherName = book.Publisher.Name
            };

            return CreatedAtAction(nameof(GetBook), new { isbn = book.ISBN }, bookDto);
        }

        // PUT: api/books/{isbn}
        [HttpPut("{isbn}")]
        public async Task<IActionResult> UpdateBook(string isbn, UpdateBookDto updateBookDto)
        {
            var book = _context.Books.FirstOrDefault(b => b.ISBN == isbn);

            if (book == null)
                return NotFound(new { message = "Könyv nem található" });

            book.Title = updateBookDto.Title;
            book.Author = updateBookDto.Author;
            book.Language = updateBookDto.Language;
            if (updateBookDto.Pages.HasValue)
            {
                book.Pages = updateBookDto.Pages.Value;
            }
            book.Description = updateBookDto.Description;

            if (updateBookDto.PublisherID.HasValue)
            {
                var publisher = _context.Publishers.FirstOrDefault(p => p.ID == updateBookDto.PublisherID);
                if (publisher == null)
                    return BadRequest(new { message = "Kiadó nem található" });

                book.Publisher = publisher;
            }

            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Könyv sikeresen frissítve" });
        }

        // DELETE: api/books/{isbn}
        [HttpDelete("{isbn}")]
        public async Task<IActionResult> DeleteBook(string isbn)
        {
            var book = _context.Books.FirstOrDefault(b => b.ISBN == isbn);

            if (book == null)
                return NotFound(new { message = "Könyv nem található" });

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Könyv sikeresen törölve" });
        }
    }

    // DTOs
    public class BookDto
    {
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Language { get; set; }
        public int Pages { get; set; }
        public string? Description { get; set; }
        public int PublisherID { get; set; }        // ← NEM nullable (biztos van)
        public string? PublisherName { get; set; }
    }

        public class CreateBookDto
    {
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Language { get; set; }
        public int Pages { get; set; }              // ← NEM nullable (kötelező)
        public string? Description { get; set; }
        public int PublisherID { get; set; }        // ← NEM nullable (KÖTELEZŐ)
    }

        public class UpdateBookDto
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Language { get; set; }
        public int Pages { get; set; }              // ← NEM nullable (kötelező)
        public string? Description { get; set; }
        public int? PublisherID { get; set; }       // ← NULLABLE (opcionális módosítás)
    }
}
