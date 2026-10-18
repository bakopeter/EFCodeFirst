using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCodeFirst.API.Models;
using System.Linq.Expressions;

namespace EFCodeFirst.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(LibraryContext context) : ControllerBase
{
    // Újrahasznosítható projekció a GET lekérdezésekhez
    private static readonly Expression<Func<Book, BookDto>> ToDto = b => new BookDto
    {
        ISBN = b.ISBN,
        Title = b.Title,
        Author = b.Author,
        Language = b.Language,
        Pages = b.Pages,
        Description = b.Description,
        PublisherID = b.Publisher.ID,
        PublisherName = b.Publisher.Name
    };

    // GET: api/books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks() =>
    Ok(await context.Books
        .Include(b => b.Publisher)  // ✅ Ez kell!
        .Select(ToDto)
        .ToListAsync());

    // GET: api/books/{isbn}
    [HttpGet("{isbn}")]
    public async Task<ActionResult<BookDto>> GetBook(string isbn)
    {
        var book = await context.Books
            .Include(b => b.Publisher)  // ✅ Ez kell!
            .Where(b => b.ISBN == isbn)
            .Select(ToDto)
            .FirstOrDefaultAsync();

        return book is null ? NotFound("Könyv nem található") : Ok(book);
    }

    // POST: api/books
    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto dto)
    {
        var publisher = await context.Publishers.FindAsync(dto.PublisherID);
        if (publisher is null)
            return BadRequest("Kiadó nem található");

        var book = new Book
        {
            ISBN = dto.ISBN,
            Title = dto.Title,
            Author = dto.Author,
            Language = dto.Language,
            Pages = dto.Pages,
            Description = dto.Description,
            Publisher = publisher // Közvetlenül a Publisher objektumot adjuk át
        };

        context.Books.Add(book);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBook), new { isbn = book.ISBN }, dto);
    }

    // PUT: api/books/{isbn}
    [HttpPut("{isbn}")]
    public async Task<IActionResult> UpdateBook(string isbn, UpdateBookDto dto)
    {
        var book = await context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn);
        if (book is null) return NotFound("Könyv nem található");

        if (dto.PublisherID.HasValue)
        {
            var publisher = await context.Publishers.FindAsync(dto.PublisherID.Value);
            if (publisher is null) return BadRequest("Kiadó nem található");
            book.Publisher = publisher;
        }

        book.Title = dto.Title ?? book.Title;
        book.Author = dto.Author ?? book.Author;
        book.Language = dto.Language ?? book.Language;
        book.Pages = dto.Pages ?? book.Pages;
        book.Description = dto.Description ?? book.Description;

        await context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/books/{isbn}
    [HttpDelete("{isbn}")]
    public async Task<IActionResult> DeleteBook(string isbn)
    {
        var book = await context.Books.FirstOrDefaultAsync(b => b.ISBN == isbn);
        if (book is null) return NotFound("Könyv nem található");

        context.Books.Remove(book);
        await context.SaveChangesAsync();

        return NoContent();
    }
}

// DTO-k Rekordként (C# 9+)
public class BookDto
{
    public string? ISBN { get; init; }
    public string? Title { get; init; }
    public string? Author { get; init; }
    public string? Language { get; init; }
    public int Pages { get; init; }
    public string? Description { get; init; }
    public int PublisherID { get; init; }
    public string? PublisherName { get; init; }
}

public record CreateBookDto(string? ISBN, string? Title, string? Author, string? Language, int Pages, string? Description, int PublisherID);
public record UpdateBookDto(string? Title, string? Author, string? Language, int? Pages, string? Description, int? PublisherID);