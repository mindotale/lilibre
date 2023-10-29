using System.Linq.Dynamic.Core;
using System.Text.Encodings.Web;

using Lilibre.Web.Data;
using Lilibre.Web.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

using Radzen;

namespace Lilibre.Web;

public partial class DataService
{
    private readonly NavigationManager navigationManager;

    public DataService(DataContext context, NavigationManager navigationManager)
    {
        this.Context = context;
        this.navigationManager = navigationManager;
    }

    private DataContext Context { get; }

    public void Reset()
    {
        Context.ChangeTracker.Entries()
            .Where(e => e.Entity != null)
            .ToList()
            .ForEach(e => e.State = EntityState.Detached);
    }

    public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
    {
        if (query != null)
        {
            if (!string.IsNullOrEmpty(query.Filter))
            {
                if (query.FilterParameters != null)
                {
                    items = items.Where(query.Filter, query.FilterParameters);
                }
                else
                {
                    items = items.Where(query.Filter);
                }
            }

            if (!string.IsNullOrEmpty(query.OrderBy))
            {
                items = items.OrderBy(query.OrderBy);
            }

            if (query.Skip.HasValue)
            {
                items = items.Skip(query.Skip.Value);
            }

            if (query.Top.HasValue)
            {
                items = items.Take(query.Top.Value);
            }
        }
    }

    public async Task ExportAuthorsToExcel(Query query = null, string fileName = null)
    {
        navigationManager.NavigateTo(
            query != null
                ? query.ToUrl(
                    $"export/data/authors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
                : $"export/data/authors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
            true);
    }

    public async Task ExportAuthorsToCSV(Query query = null, string fileName = null)
    {
        navigationManager.NavigateTo(
            query != null
                ? query.ToUrl(
                    $"export/data/authors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
                : $"export/data/authors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
            true);
    }

    partial void OnAuthorsRead(ref IQueryable<Author> items);

    public async Task<IQueryable<Author>> GetAuthors(Query query = null)
    {
        var items = Context.Authors.AsQueryable();

        if (query != null)
        {
            if (!string.IsNullOrEmpty(query.Expand))
            {
                var propertiesToExpand = query.Expand.Split(',');
                foreach (var p in propertiesToExpand)
                {
                    items = items.Include(p.Trim());
                }
            }

            ApplyQuery(ref items, query);
        }

        OnAuthorsRead(ref items);

        return await Task.FromResult(items);
    }

    partial void OnAuthorGet(Author item);

    partial void OnGetAuthorById(ref IQueryable<Author> items);

    public async Task<Author> GetAuthorById(int id)
    {
        var items = Context.Authors.AsNoTracking().Where(i => i.Id == id);

        OnGetAuthorById(ref items);

        var itemToReturn = items.FirstOrDefault();

        OnAuthorGet(itemToReturn);

        return await Task.FromResult(itemToReturn);
    }

    partial void OnAuthorCreated(Author item);

    partial void OnAfterAuthorCreated(Author item);

    public async Task<Author> CreateAuthor(Author author)
    {
        OnAuthorCreated(author);

        var existingItem = Context.Authors.Where(i => i.Id == author.Id).FirstOrDefault();

        if (existingItem != null)
        {
            throw new Exception("Item already available");
        }

        try
        {
            Context.Authors.Add(author);
            Context.SaveChanges();
        }
        catch
        {
            Context.Entry(author).State = EntityState.Detached;
            throw;
        }

        OnAfterAuthorCreated(author);

        return author;
    }

    public async Task<Author> CancelAuthorChanges(Author item)
    {
        var entityToCancel = Context.Entry(item);
        if (entityToCancel.State == EntityState.Modified)
        {
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;
        }

        return item;
    }

    partial void OnAuthorUpdated(Author item);

    partial void OnAfterAuthorUpdated(Author item);

    public async Task<Author> UpdateAuthor(int id, Author author)
    {
        OnAuthorUpdated(author);

        var itemToUpdate = Context.Authors.Where(i => i.Id == author.Id).FirstOrDefault();

        if (itemToUpdate == null)
        {
            throw new Exception("Item no longer available");
        }

        var entryToUpdate = Context.Entry(itemToUpdate);
        entryToUpdate.CurrentValues.SetValues(author);
        entryToUpdate.State = EntityState.Modified;

        Context.SaveChanges();

        OnAfterAuthorUpdated(author);

        return author;
    }

    partial void OnAuthorDeleted(Author item);

    partial void OnAfterAuthorDeleted(Author item);

    public async Task<Author> DeleteAuthor(int id)
    {
        var itemToDelete = Context.Authors.Where(i => i.Id == id).Include(i => i.BookAuthors).FirstOrDefault();

        if (itemToDelete == null)
        {
            throw new Exception("Item no longer available");
        }

        OnAuthorDeleted(itemToDelete);

        Context.Authors.Remove(itemToDelete);

        try
        {
            Context.SaveChanges();
        }
        catch
        {
            Context.Entry(itemToDelete).State = EntityState.Unchanged;
            throw;
        }

        OnAfterAuthorDeleted(itemToDelete);

        return itemToDelete;
    }

    public async Task ExportBookAuthorsToExcel(Query query = null, string fileName = null)
    {
        navigationManager.NavigateTo(
            query != null
                ? query.ToUrl(
                    $"export/data/bookauthors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
                : $"export/data/bookauthors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
            true);
    }

    public async Task ExportBookAuthorsToCSV(Query query = null, string fileName = null)
    {
        navigationManager.NavigateTo(
            query != null
                ? query.ToUrl(
                    $"export/data/bookauthors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
                : $"export/data/bookauthors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
            true);
    }

    partial void OnBookAuthorsRead(ref IQueryable<BookAuthor> items);

    public async Task<IQueryable<BookAuthor>> GetBookAuthors(Query query = null)
    {
        var items = Context.BookAuthors.AsQueryable();

        items = items.Include(i => i.Author);
        items = items.Include(i => i.Book);

        if (query != null)
        {
            if (!string.IsNullOrEmpty(query.Expand))
            {
                var propertiesToExpand = query.Expand.Split(',');
                foreach (var p in propertiesToExpand)
                {
                    items = items.Include(p.Trim());
                }
            }

            ApplyQuery(ref items, query);
        }

        OnBookAuthorsRead(ref items);

        return await Task.FromResult(items);
    }

    partial void OnBookAuthorGet(BookAuthor item);

    partial void OnGetBookAuthorByAuthorsIdAndBookId(ref IQueryable<BookAuthor> items);

    public async Task<BookAuthor> GetBookAuthorByAuthorsIdAndBookId(int authorsid, int bookid)
    {
        var items = Context.BookAuthors.AsNoTracking().Where(i => i.AuthorsId == authorsid && i.BookId == bookid);

        items = items.Include(i => i.Author);
        items = items.Include(i => i.Book);

        OnGetBookAuthorByAuthorsIdAndBookId(ref items);

        var itemToReturn = items.FirstOrDefault();

        OnBookAuthorGet(itemToReturn);

        return await Task.FromResult(itemToReturn);
    }

    partial void OnBookAuthorCreated(BookAuthor item);

    partial void OnAfterBookAuthorCreated(BookAuthor item);

    public async Task<BookAuthor> CreateBookAuthor(BookAuthor bookauthor)
    {
        OnBookAuthorCreated(bookauthor);

        var existingItem = Context.BookAuthors
            .Where(i => i.AuthorsId == bookauthor.AuthorsId && i.BookId == bookauthor.BookId)
            .FirstOrDefault();

        if (existingItem != null)
        {
            throw new Exception("Item already available");
        }

        try
        {
            Context.BookAuthors.Add(bookauthor);
            Context.SaveChanges();
        }
        catch
        {
            Context.Entry(bookauthor).State = EntityState.Detached;
            throw;
        }

        OnAfterBookAuthorCreated(bookauthor);

        return bookauthor;
    }

    public async Task<BookAuthor> CancelBookAuthorChanges(BookAuthor item)
    {
        var entityToCancel = Context.Entry(item);
        if (entityToCancel.State == EntityState.Modified)
        {
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;
        }

        return item;
    }

    partial void OnBookAuthorUpdated(BookAuthor item);

    partial void OnAfterBookAuthorUpdated(BookAuthor item);

    public async Task<BookAuthor> UpdateBookAuthor(int authorsid, int bookid, BookAuthor bookauthor)
    {
        OnBookAuthorUpdated(bookauthor);

        var itemToUpdate = Context.BookAuthors
            .Where(i => i.AuthorsId == bookauthor.AuthorsId && i.BookId == bookauthor.BookId)
            .FirstOrDefault();

        if (itemToUpdate == null)
        {
            throw new Exception("Item no longer available");
        }

        var entryToUpdate = Context.Entry(itemToUpdate);
        entryToUpdate.CurrentValues.SetValues(bookauthor);
        entryToUpdate.State = EntityState.Modified;

        Context.SaveChanges();

        OnAfterBookAuthorUpdated(bookauthor);

        return bookauthor;
    }

    partial void OnBookAuthorDeleted(BookAuthor item);

    partial void OnAfterBookAuthorDeleted(BookAuthor item);

    public async Task<BookAuthor> DeleteBookAuthor(int authorsid, int bookid)
    {
        var itemToDelete = Context.BookAuthors.Where(i => i.AuthorsId == authorsid && i.BookId == bookid)
            .FirstOrDefault();

        if (itemToDelete == null)
        {
            throw new Exception("Item no longer available");
        }

        OnBookAuthorDeleted(itemToDelete);

        Context.BookAuthors.Remove(itemToDelete);

        try
        {
            Context.SaveChanges();
        }
        catch
        {
            Context.Entry(itemToDelete).State = EntityState.Unchanged;
            throw;
        }

        OnAfterBookAuthorDeleted(itemToDelete);

        return itemToDelete;
    }

    public async Task ExportBookGenresToExcel(Query query = null, string fileName = null)
    {
        navigationManager.NavigateTo(
            query != null
                ? query.ToUrl(
                    $"export/data/bookgenres/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
                : $"export/data/bookgenres/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
            true);
    }

    public async Task ExportBookGenresToCSV(Query query = null, string fileName = null)
    {
        navigationManager.NavigateTo(
            query != null
                ? query.ToUrl(
                    $"export/data/bookgenres/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
                : $"export/data/bookgenres/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
            true);
    }

    partial void OnBookGenresRead(ref IQueryable<BookGenre> items);

    public async Task<IQueryable<BookGenre>> GetBookGenres(Query query = null)
    {
        var items = Context.BookGenres.AsQueryable();

        items = items.Include(i => i.Book);
        items = items.Include(i => i.Genre);

        if (query != null)
        {
            if (!string.IsNullOrEmpty(query.Expand))
            {
                var propertiesToExpand = query.Expand.Split(',');
                foreach (var p in propertiesToExpand)
                {
                    items = items.Include(p.Trim());
                }
            }

            ApplyQuery(ref items, query);
        }

        OnBookGenresRead(ref items);

        return await Task.FromResult(items);
    }

    partial void OnBookGenreGet(BookGenre item);

    partial void OnGetBookGenreByBookIdAndGenresId(ref IQueryable<BookGenre> items);

    public async Task<BookGenre> GetBookGenreByBookIdAndGenresId(int bookid, int genresid)
    {
        var items = Context.BookGenres.AsNoTracking().Where(i => i.BookId == bookid && i.GenresId == genresid);

        items = items.Include(i => i.Book);
        items = items.Include(i => i.Genre);

        OnGetBookGenreByBookIdAndGenresId(ref items);

        var itemToReturn = items.FirstOrDefault();

        OnBookGenreGet(itemToReturn);

        return await Task.FromResult(itemToReturn);
    }

    partial void OnBookGenreCreated(BookGenre item);

    partial void OnAfterBookGenreCreated(BookGenre item);

    public async Task<BookGenre> CreateBookGenre(BookGenre bookgenre)
    {
        OnBookGenreCreated(bookgenre);

        var existingItem = Context.BookGenres
            .Where(i => i.BookId == bookgenre.BookId && i.GenresId == bookgenre.GenresId)
            .FirstOrDefault();

        if (existingItem != null)
        {
            throw new Exception("Item already available");
        }

        try
        {
            Context.BookGenres.Add(bookgenre);
            Context.SaveChanges();
        }
        catch
        {
            Context.Entry(bookgenre).State = EntityState.Detached;
            throw;
        }

        OnAfterBookGenreCreated(bookgenre);

        return bookgenre;
    }

    public async Task<BookGenre> CancelBookGenreChanges(BookGenre item)
    {
        var entityToCancel = Context.Entry(item);
        if (entityToCancel.State == EntityState.Modified)
        {
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;
        }

        return item;
    }

    partial void OnBookGenreUpdated(BookGenre item);

    partial void OnAfterBookGenreUpdated(BookGenre item);

    public async Task<BookGenre> UpdateBookGenre(int bookid, int genresid, BookGenre bookgenre)
    {
        OnBookGenreUpdated(bookgenre);

        var itemToUpdate = Context.BookGenres
            .Where(i => i.BookId == bookgenre.BookId && i.GenresId == bookgenre.GenresId)
            .FirstOrDefault();

        if (itemToUpdate == null)
        {
            throw new Exception("Item no longer available");
        }

        var entryToUpdate = Context.Entry(itemToUpdate);
        entryToUpdate.CurrentValues.SetValues(bookgenre);
        entryToUpdate.State = EntityState.Modified;

        Context.SaveChanges();

        OnAfterBookGenreUpdated(bookgenre);

        return bookgenre;
    }

    partial void OnBookGenreDeleted(BookGenre item);

    partial void OnAfterBookGenreDeleted(BookGenre item);

    public async Task<BookGenre> DeleteBookGenre(int bookid, int genresid)
    {
        var itemToDelete = Context.BookGenres.Where(i => i.BookId == bookid && i.GenresId == genresid).FirstOrDefault();

        if (itemToDelete == null)
        {
            throw new Exception("Item no longer available");
        }

        OnBookGenreDeleted(itemToDelete);

        Context.BookGenres.Remove(itemToDelete);

        try
        {
            Context.SaveChanges();
        }
        catch
        {
            Context.Entry(itemToDelete).State = EntityState.Unchanged;
            throw;
        }

        OnAfterBookGenreDeleted(itemToDelete);

        return itemToDelete;
    }

    public async Task ExportBooksToExcel(Query query = null, string fileName = null)
    {
        navigationManager.NavigateTo(
            query != null
                ? query.ToUrl(
                    $"export/data/books/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
                : $"export/data/books/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
            true);
    }

    public async Task ExportBooksToCSV(Query query = null, string fileName = null)
    {
        navigationManager.NavigateTo(
            query != null
                ? query.ToUrl(
                    $"export/data/books/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
                : $"export/data/books/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
            true);
    }

    partial void OnBooksRead(ref IQueryable<Book> items);

    public async Task<IQueryable<Book>> GetBooks(Query query = null)
    {
        var items = Context.Books.AsQueryable();

        if (query != null)
        {
            if (!string.IsNullOrEmpty(query.Expand))
            {
                var propertiesToExpand = query.Expand.Split(',');
                foreach (var p in propertiesToExpand)
                {
                    items = items.Include(p.Trim());
                }
            }

            ApplyQuery(ref items, query);
        }

        OnBooksRead(ref items);

        return await Task.FromResult(items);
    }

    partial void OnBookGet(Book item);

    partial void OnGetBookById(ref IQueryable<Book> items);

    public async Task<Book> GetBookById(int id)
    {
        var items = Context.Books.AsNoTracking().Where(i => i.Id == id);

        OnGetBookById(ref items);

        var itemToReturn = items.FirstOrDefault();

        OnBookGet(itemToReturn);

        return await Task.FromResult(itemToReturn);
    }

    partial void OnBookCreated(Book item);

    partial void OnAfterBookCreated(Book item);

    public async Task<Book> CreateBook(Book book)
    {
        OnBookCreated(book);

        var existingItem = Context.Books.Where(i => i.Id == book.Id).FirstOrDefault();

        if (existingItem != null)
        {
            throw new Exception("Item already available");
        }

        try
        {
            Context.Books.Add(book);
            Context.SaveChanges();
        }
        catch
        {
            Context.Entry(book).State = EntityState.Detached;
            throw;
        }

        OnAfterBookCreated(book);

        return book;
    }

    public async Task<Book> CancelBookChanges(Book item)
    {
        var entityToCancel = Context.Entry(item);
        if (entityToCancel.State == EntityState.Modified)
        {
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;
        }

        return item;
    }

    partial void OnBookUpdated(Book item);

    partial void OnAfterBookUpdated(Book item);

    public async Task<Book> UpdateBook(int id, Book book)
    {
        OnBookUpdated(book);

        var itemToUpdate = Context.Books.Where(i => i.Id == book.Id).FirstOrDefault();

        if (itemToUpdate == null)
        {
            throw new Exception("Item no longer available");
        }

        var entryToUpdate = Context.Entry(itemToUpdate);
        entryToUpdate.CurrentValues.SetValues(book);
        entryToUpdate.State = EntityState.Modified;

        Context.SaveChanges();

        OnAfterBookUpdated(book);

        return book;
    }

    partial void OnBookDeleted(Book item);

    partial void OnAfterBookDeleted(Book item);

    public async Task<Book> DeleteBook(int id)
    {
        var itemToDelete = Context.Books.Where(i => i.Id == id)
            .Include(i => i.BookAuthors)
            .Include(i => i.BookGenres)
            .FirstOrDefault();

        if (itemToDelete == null)
        {
            throw new Exception("Item no longer available");
        }

        OnBookDeleted(itemToDelete);

        Context.Books.Remove(itemToDelete);

        try
        {
            Context.SaveChanges();
        }
        catch
        {
            Context.Entry(itemToDelete).State = EntityState.Unchanged;
            throw;
        }

        OnAfterBookDeleted(itemToDelete);

        return itemToDelete;
    }

    public async Task ExportGenresToExcel(Query query = null, string fileName = null)
    {
        navigationManager.NavigateTo(
            query != null
                ? query.ToUrl(
                    $"export/data/genres/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
                : $"export/data/genres/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
            true);
    }

    public async Task ExportGenresToCSV(Query query = null, string fileName = null)
    {
        navigationManager.NavigateTo(
            query != null
                ? query.ToUrl(
                    $"export/data/genres/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')")
                : $"export/data/genres/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
            true);
    }

    partial void OnGenresRead(ref IQueryable<Genre> items);

    public async Task<IQueryable<Genre>> GetGenres(Query query = null)
    {
        var items = Context.Genres.AsQueryable();

        if (query != null)
        {
            if (!string.IsNullOrEmpty(query.Expand))
            {
                var propertiesToExpand = query.Expand.Split(',');
                foreach (var p in propertiesToExpand)
                {
                    items = items.Include(p.Trim());
                }
            }

            ApplyQuery(ref items, query);
        }

        OnGenresRead(ref items);

        return await Task.FromResult(items);
    }

    partial void OnGenreGet(Genre item);

    partial void OnGetGenreById(ref IQueryable<Genre> items);

    public async Task<Genre> GetGenreById(int id)
    {
        var items = Context.Genres.AsNoTracking().Where(i => i.Id == id);

        OnGetGenreById(ref items);

        var itemToReturn = items.FirstOrDefault();

        OnGenreGet(itemToReturn);

        return await Task.FromResult(itemToReturn);
    }

    partial void OnGenreCreated(Genre item);

    partial void OnAfterGenreCreated(Genre item);

    public async Task<Genre> CreateGenre(Genre genre)
    {
        OnGenreCreated(genre);

        var existingItem = Context.Genres.Where(i => i.Id == genre.Id).FirstOrDefault();

        if (existingItem != null)
        {
            throw new Exception("Item already available");
        }

        try
        {
            Context.Genres.Add(genre);
            Context.SaveChanges();
        }
        catch
        {
            Context.Entry(genre).State = EntityState.Detached;
            throw;
        }

        OnAfterGenreCreated(genre);

        return genre;
    }

    public async Task<Genre> CancelGenreChanges(Genre item)
    {
        var entityToCancel = Context.Entry(item);
        if (entityToCancel.State == EntityState.Modified)
        {
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;
        }

        return item;
    }

    partial void OnGenreUpdated(Genre item);

    partial void OnAfterGenreUpdated(Genre item);

    public async Task<Genre> UpdateGenre(int id, Genre genre)
    {
        OnGenreUpdated(genre);

        var itemToUpdate = Context.Genres.Where(i => i.Id == genre.Id).FirstOrDefault();

        if (itemToUpdate == null)
        {
            throw new Exception("Item no longer available");
        }

        var entryToUpdate = Context.Entry(itemToUpdate);
        entryToUpdate.CurrentValues.SetValues(genre);
        entryToUpdate.State = EntityState.Modified;

        Context.SaveChanges();

        OnAfterGenreUpdated(genre);

        return genre;
    }

    partial void OnGenreDeleted(Genre item);

    partial void OnAfterGenreDeleted(Genre item);

    public async Task<Genre> DeleteGenre(int id)
    {
        var itemToDelete = Context.Genres.Where(i => i.Id == id).Include(i => i.BookGenres).FirstOrDefault();

        if (itemToDelete == null)
        {
            throw new Exception("Item no longer available");
        }

        OnGenreDeleted(itemToDelete);

        Context.Genres.Remove(itemToDelete);

        try
        {
            Context.SaveChanges();
        }
        catch
        {
            Context.Entry(itemToDelete).State = EntityState.Unchanged;
            throw;
        }

        OnAfterGenreDeleted(itemToDelete);

        return itemToDelete;
    }
}
