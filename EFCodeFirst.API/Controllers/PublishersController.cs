using Microsoft.AspNetCore.Mvc;
using EFCodeFirst.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCodeFirst.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublishersController : ControllerBase
    {
        private readonly LibraryContext _context;

        public PublishersController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/publishers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherDto>>> GetPublishers()
        {
            var publishers = _context.Publishers.Select(p => new PublisherDto
            {
                ID = p.ID,
                Name = p.Name,
                BookCount = p.Books.Count
            }).ToList();

            return Ok(publishers);
        }

        // GET: api/publishers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDto>> GetPublisher(int id)
        {
            var publisher = _context.Publishers.FirstOrDefault(p => p.ID == id);

            if (publisher == null)
                return NotFound(new { message = "Kiadó nem található" });

            var publisherDto = new PublisherDto
            {
                ID = publisher.ID,
                Name = publisher.Name,
                BookCount = publisher.Books.Count
            };

            return Ok(publisherDto);
        }

        // POST: api/publishers
        [HttpPost]
        public async Task<ActionResult<PublisherDto>> CreatePublisher(CreatePublisherDto createPublisherDto)
        {
            var publisher = new Publisher
            {
                Name = createPublisherDto.Name,
                Books = new List<Book>()
            };

            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            var publisherDto = new PublisherDto
            {
                ID = publisher.ID,
                Name = publisher.Name,
                BookCount = 0
            };

            return CreatedAtAction(nameof(GetPublisher), new { id = publisher.ID }, publisherDto);
        }

        // PUT: api/publishers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublisher(int id, UpdatePublisherDto updatePublisherDto)
        {
            var publisher = _context.Publishers.FirstOrDefault(p => p.ID == id);

            if (publisher == null)
                return NotFound(new { message = "Kiadó nem található" });

            publisher.Name = updatePublisherDto.Name;

            _context.Publishers.Update(publisher);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Kiadó sikeresen frissítve" });
        }

        // DELETE: api/publishers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var publisher = _context.Publishers.FirstOrDefault(p => p.ID == id);

            if (publisher == null)
                return NotFound(new { message = "Kiadó nem található" });

            if (publisher.Books.Count > 0)
                return BadRequest(new { message = "Nem lehet törölni egy kiadót, aminek még könyvei vannak" });

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Kiadó sikeresen törölve" });
        }
    }

    // DTOs
    public class PublisherDto
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public int BookCount { get; set; }
    }

    public class CreatePublisherDto
    {
        public string? Name { get; set; }
    }

    public class UpdatePublisherDto
    {
        public string? Name { get; set; }
    }
}
