using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
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
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        private T DeserializeResponse<T>(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var responseContent = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        private HttpResponseMessage SendRequest(HttpMethod method, string endpoint, HttpContent content = null)
        {
            return method.Method switch
            {
                "GET" => _httpClient.GetAsync(endpoint).Result,
                "POST" => _httpClient.PostAsync(endpoint, content).Result,
                "PUT" => _httpClient.PutAsync(endpoint, content).Result,
                "DELETE" => _httpClient.DeleteAsync(endpoint).Result,
                _ => throw new Exception("Invalid HTTP method."),
            };
        }

        public IEnumerable<Author> GetAll()
        {
            var response = SendRequest(HttpMethod.Get, "authors");
            return DeserializeResponse<IEnumerable<Author>>(response) ?? throw new Exception("Failed to get authors.");
        }

        public Author? GetById(int id)
        {
            var response = SendRequest(HttpMethod.Get, $"authors/{id}");
            return response.IsSuccessStatusCode ? DeserializeResponse<Author>(response) : null;
        }

        public Author Add(Author author)
        {
            var authorJson = JsonConvert.SerializeObject(author);
            var content = new StringContent(authorJson, System.Text.Encoding.Default, "application/json");
            var response = SendRequest(HttpMethod.Post, "authors", content);
            return DeserializeResponse<Author>(response) ?? throw new Exception("Failed to add author.");
        }

        public Author Update(Author author)
        {
            var authorJson = JsonConvert.SerializeObject(author);
            var content = new StringContent(authorJson, System.Text.Encoding.Default, "application/json");
            var response = SendRequest(HttpMethod.Put, $"authors/{author.Id}", content);
            return author;
        }

        public void Remove(int id)
        {
            var response = SendRequest(HttpMethod.Delete, $"authors/{id}");
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
