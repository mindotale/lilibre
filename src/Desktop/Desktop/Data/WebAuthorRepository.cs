using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using Desktop.Models;

namespace Desktop.Data
{
    public sealed record WebAuthorRepositoryConfig(string BaseUrl);

    public sealed class WebAuthorRepository : IAuthorRepository, IDisposable
    {
        private readonly HttpClient _httpClient;

        public WebAuthorRepository(WebAuthorRepositoryConfig config)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(config.BaseUrl) };
        }

        public IEnumerable<Author> GetAll()
        {
            var response = _httpClient.GetAsync("authors").Result;
            response.EnsureSuccessStatusCode();
            using var responseStream = response.Content.ReadAsStream();
            var authorResult = JsonSerializer.Deserialize<IEnumerable<Author>>(responseStream);
            if (authorResult is null)
            {
                throw new Exception("Failed to get authors.");
            }

            return authorResult;
        }

        public Author? GetById(int id)
        {
            var response = _httpClient.GetAsync($"authors/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = response.Content.ReadAsStream();
                return JsonSerializer.Deserialize<Author>(responseStream);
            }   

            return null;
        }

        public Author Add(Author author)
        {
            var authorJson = JsonSerializer.Serialize(author);
            var content = new StringContent(authorJson, System.Text.Encoding.Default, "application/json");
            var response = _httpClient.PostAsync("authors", content).Result;
            response.EnsureSuccessStatusCode();
            using var responseStream = response.Content.ReadAsStream();
            var authorResult = JsonSerializer.Deserialize<Author>(responseStream);
            if (authorResult is null)
            {
                throw new Exception("Failed to add author.");
            }

            return authorResult;
        }

        public Author Update(Author author)
        {
            var authorJson = JsonSerializer.Serialize(author);
            var content = new StringContent(authorJson, System.Text.Encoding.Default, "application/json");
            var response = _httpClient.PutAsync($"authors/{author.Id}", content).Result;
            response.EnsureSuccessStatusCode();
            using var responseStream = response.Content.ReadAsStream();
            var authorResult = JsonSerializer.Deserialize<Author>(responseStream);
            if (authorResult is null)
            {
                throw new Exception("Failed to update author.");
            }

            return authorResult;
        }

        public void Remove(int id)
        {
            var response = _httpClient.DeleteAsync($"authors/{id}").Result;
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
