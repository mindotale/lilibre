using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Data.SqlClient;

const string connectionString =
    "Server=localhost,1433; Database=Lilibre; User Id=sa; Password=Password!; Encrypt=False; TrustServerCertificate=True; Connection Timeout=30;";

// Input pagination parameters

Console.Write("Enter offset: ");
var offsetInput = Console.ReadLine() ?? string.Empty;
if (!int.TryParse(offsetInput, out var offset))
{
    Console.WriteLine("Invalid offset input. Please enter a valid integer.");
    return;
}

Console.Write("Enter limit: ");
var limitInput = Console.ReadLine() ?? string.Empty;
if (!int.TryParse(limitInput, out var limit))
{
    Console.WriteLine("Invalid limit input. Please enter a valid integer.");
    return;
}

DisplayBooks(connectionString, offset, limit);

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

return;

static void DisplayBooks(string connectionString, int offset, int limit)
{
    try
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();

        var query = @"
            SELECT b.Id AS BookId, b.Title AS BookTitle, b.Description AS BookDescription, b.Isbn AS BookIsbn, 
                   b.Pages AS BookPages, b.Price AS BookPrice, b.Year AS BookYear,
                   a.Id AS AuthorId, a.Name AS AuthorName, a.BirthYear AS AuthorBirthYear,
                   g.Id AS GenreId, g.Name AS GenreName, g.Description AS GenreDescription,
                   p.Id AS PublisherId, p.Name AS PublisherName, p.Website AS PublisherWebsite, p.Email AS PublisherEmail
            FROM Books b
            LEFT JOIN BookAuthors ba ON b.Id = ba.BookId
            LEFT JOIN Authors a ON ba.AuthorsId = a.Id
            LEFT JOIN BookGenres bg ON b.Id = bg.BookId
            LEFT JOIN Genres g ON bg.GenresId = g.Id
            LEFT JOIN Publishers p ON b.PublisherId = p.Id
            ORDER BY b.Id OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Offset", offset);
        command.Parameters.AddWithValue("@Limit", limit);

        using var reader = command.ExecuteReader();

        var books = new List<Dictionary<string, object>>();

        while (reader.Read())
        {
            var bookId = (int)reader["BookId"];
            var book = books.FirstOrDefault(b => (int)b["Id"] == bookId);
            if (book == null)
            {
                book = new Dictionary<string, object>
                {
                    ["Id"] = reader["BookId"],
                    ["Title"] = reader["BookTitle"],
                    ["Description"] = reader["BookDescription"],
                    ["Isbn"] = reader["BookIsbn"],
                    ["Pages"] = reader["BookPages"],
                    ["Price"] = reader["BookPrice"],
                    ["Year"] = reader["BookYear"],
                    ["Authors"] = new List<Dictionary<string, object>>(),
                    ["Genres"] = new List<Dictionary<string, object>>(),
                    ["Publisher"] = new Dictionary<string, object>
                    {
                        ["Id"] = reader["PublisherId"],
                        ["Name"] = reader["PublisherName"],
                        ["Website"] = reader["PublisherWebsite"],
                        ["Email"] = reader["PublisherEmail"]
                    }
                };
                books.Add(book);
            }

            if (reader["AuthorId"] != DBNull.Value)
            {
                ((List<Dictionary<string, object>>)book["Authors"]).Add(new Dictionary<string, object>
                {
                    ["Id"] = reader["AuthorId"],
                    ["Name"] = reader["AuthorName"],
                    ["BirthYear"] = reader["AuthorBirthYear"]
                });
            }

            if (reader["GenreId"] != DBNull.Value)
            {
                ((List<Dictionary<string, object>>)book["Genres"]).Add(new Dictionary<string, object>
                {
                    ["Id"] = reader["GenreId"],
                    ["Name"] = reader["GenreName"],
                    ["Description"] = reader["GenreDescription"]
                });
            }
        }

        var json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(json);
    }
    catch (SqlException ex)
    {
        Console.WriteLine("SQL Error: " + ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
    }
}
