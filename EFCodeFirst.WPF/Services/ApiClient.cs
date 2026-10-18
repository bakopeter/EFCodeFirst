using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace EFCodeFirst.WPF
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5226/api";

        public ApiClient()
        {
            var handler = new HttpClientHandler();
            // SSL ellenõrzés kikapcsolása fejlesztéskor (Windows)
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        // BOOKS OPERATIONS
        public async Task<List<BookDto>> GetAllBooksAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/books");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<List<BookDto>>(json, options);
                }
                return new List<BookDto>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a könyvek letöltésekor: {ex.Message}");
            }
        }

        public async Task<BookDto> GetBookAsync(string isbn)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/books/{isbn}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<BookDto>(json, options);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a könyv letöltésekor: {ex.Message}");
            }
        }

        public async Task<bool> CreateBookAsync(CreateBookDto book)
        {
            try
            {
                var json = JsonSerializer.Serialize(book);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/books", content);
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
                var json = JsonSerializer.Serialize(book);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/books/{isbn}", content);
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

        // PUBLISHERS OPERATIONS
        public async Task<List<PublisherDto>> GetAllPublishersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/publishers");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<List<PublisherDto>>(json, options);
                }
                return new List<PublisherDto>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a kiadók letöltésekor: {ex.Message}");
            }
        }

        public async Task<bool> CreatePublisherAsync(CreatePublisherDto publisher)
        {
            try
            {
                var json = JsonSerializer.Serialize(publisher);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/publishers", content);
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
                var json = JsonSerializer.Serialize(publisher);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/publishers/{id}", content);
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

    // DTOs
    public class BookDto
    {
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Language { get; set; }
        public int Pages { get; set; }
        public string? Description { get; set; }
        public int PublisherID { get; set; }
        public string? PublisherName { get; set; }
    }

    public class CreateBookDto
    {
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Language { get; set; }
        public int Pages { get; set; }
        public string? Description { get; set; }
        public int PublisherID { get; set; }
    }

    public class UpdateBookDto
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Language { get; set; }
        public int? Pages { get; set; }
        public string? Description { get; set; }
        public int? PublisherID { get; set; }
    }

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