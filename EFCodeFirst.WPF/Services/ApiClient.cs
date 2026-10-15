using System.Net.Http.Json;

namespace EFCodeFirst.WPF.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:5001/api";

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        // ===== BOOKS =====

        public async Task<List<BookDto>> GetBooksAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<BookDto>>("/api/books") ?? new List<BookDto>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a könyvek betöltésekor: {ex.Message}");
            }
        }

        public async Task<BookDto> GetBookAsync(string isbn)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<BookDto>($"/api/books/{isbn}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a könyv betöltésekor: {ex.Message}");
            }
        }

        public async Task<bool> CreateBookAsync(CreateBookDto book)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/books", book);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a könyv létrehozásakor: {ex.Message}");
            }
        }

        public async Task<bool> UpdateBookAsync(string isbn, UpdateBookDto book)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/books/{isbn}", book);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a könyv frissítésekor: {ex.Message}");
            }
        }

        public async Task<bool> DeleteBookAsync(string isbn)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/books/{isbn}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a könyv törlésekor: {ex.Message}");
            }
        }

        // ===== PUBLISHERS =====

        public async Task<List<PublisherDto>> GetPublishersAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<PublisherDto>>("/api/publishers") ?? new List<PublisherDto>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a kiadók betöltésekor: {ex.Message}");
            }
        }

        public async Task<PublisherDto> GetPublisherAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PublisherDto>($"/api/publishers/{id}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a kiadó betöltésekor: {ex.Message}");
            }
        }

        public async Task<bool> CreatePublisherAsync(CreatePublisherDto publisher)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/publishers", publisher);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a kiadó létrehozásakor: {ex.Message}");
            }
        }

        public async Task<bool> UpdatePublisherAsync(int id, UpdatePublisherDto publisher)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/api/publishers/{id}", publisher);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a kiadó frissítésekor: {ex.Message}");
            }
        }

        public async Task<bool> DeletePublisherAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/publishers/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a kiadó törlésekor: {ex.Message}");
            }
        }
    }

    // ===== DTOs =====

    public class BookDto
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Language { get; set; }
        public int Pages { get; set; }
        public string Description { get; set; }
        public int PublisherID { get; set; }
        public string PublisherName { get; set; }
    }

    public class CreateBookDto
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Language { get; set; }
        public int Pages { get; set; }
        public string Description { get; set; }
        public int PublisherID { get; set; }
    }

    public class UpdateBookDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Language { get; set; }
        public int Pages { get; set; }
        public string Description { get; set; }
        public int? PublisherID { get; set; }
    }

    public class PublisherDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int BookCount { get; set; }
    }

    public class CreatePublisherDto
    {
        public string Name { get; set; }
    }

    public class UpdatePublisherDto
    {
        public string Name { get; set; }
    }
}
