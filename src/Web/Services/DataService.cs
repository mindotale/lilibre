using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using Lilibre.Web.Data;

namespace Lilibre.Web
{
    public partial class DataService
    {
        DataContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly DataContext context;
        private readonly NavigationManager navigationManager;

        public DataService(DataContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

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
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/data/authors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/data/authors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAuthorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/data/authors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/data/authors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAuthorsRead(ref IQueryable<Lilibre.Web.Models.Data.Author> items);

        public async Task<IQueryable<Lilibre.Web.Models.Data.Author>> GetAuthors(Query query = null)
        {
            var items = Context.Authors.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAuthorsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAuthorGet(Lilibre.Web.Models.Data.Author item);
        partial void OnGetAuthorById(ref IQueryable<Lilibre.Web.Models.Data.Author> items);


        public async Task<Lilibre.Web.Models.Data.Author> GetAuthorById(int id)
        {
            var items = Context.Authors
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetAuthorById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAuthorGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAuthorCreated(Lilibre.Web.Models.Data.Author item);
        partial void OnAfterAuthorCreated(Lilibre.Web.Models.Data.Author item);

        public async Task<Lilibre.Web.Models.Data.Author> CreateAuthor(Lilibre.Web.Models.Data.Author author)
        {
            OnAuthorCreated(author);

            var existingItem = Context.Authors
                              .Where(i => i.Id == author.Id)
                              .FirstOrDefault();

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

        public async Task<Lilibre.Web.Models.Data.Author> CancelAuthorChanges(Lilibre.Web.Models.Data.Author item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAuthorUpdated(Lilibre.Web.Models.Data.Author item);
        partial void OnAfterAuthorUpdated(Lilibre.Web.Models.Data.Author item);

        public async Task<Lilibre.Web.Models.Data.Author> UpdateAuthor(int id, Lilibre.Web.Models.Data.Author author)
        {
            OnAuthorUpdated(author);

            var itemToUpdate = Context.Authors
                              .Where(i => i.Id == author.Id)
                              .FirstOrDefault();

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

        partial void OnAuthorDeleted(Lilibre.Web.Models.Data.Author item);
        partial void OnAfterAuthorDeleted(Lilibre.Web.Models.Data.Author item);

        public async Task<Lilibre.Web.Models.Data.Author> DeleteAuthor(int id)
        {
            var itemToDelete = Context.Authors
                              .Where(i => i.Id == id)
                              .Include(i => i.BookAuthors)
                              .FirstOrDefault();

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
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/data/bookauthors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/data/bookauthors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBookAuthorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/data/bookauthors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/data/bookauthors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBookAuthorsRead(ref IQueryable<Lilibre.Web.Models.Data.BookAuthor> items);

        public async Task<IQueryable<Lilibre.Web.Models.Data.BookAuthor>> GetBookAuthors(Query query = null)
        {
            var items = Context.BookAuthors.AsQueryable();

            items = items.Include(i => i.Author);
            items = items.Include(i => i.Book);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnBookAuthorsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBookAuthorGet(Lilibre.Web.Models.Data.BookAuthor item);
        partial void OnGetBookAuthorByAuthorsIdAndBookId(ref IQueryable<Lilibre.Web.Models.Data.BookAuthor> items);


        public async Task<Lilibre.Web.Models.Data.BookAuthor> GetBookAuthorByAuthorsIdAndBookId(int authorsid, int bookid)
        {
            var items = Context.BookAuthors
                              .AsNoTracking()
                              .Where(i => i.AuthorsId == authorsid && i.BookId == bookid);

            items = items.Include(i => i.Author);
            items = items.Include(i => i.Book);
 
            OnGetBookAuthorByAuthorsIdAndBookId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnBookAuthorGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBookAuthorCreated(Lilibre.Web.Models.Data.BookAuthor item);
        partial void OnAfterBookAuthorCreated(Lilibre.Web.Models.Data.BookAuthor item);

        public async Task<Lilibre.Web.Models.Data.BookAuthor> CreateBookAuthor(Lilibre.Web.Models.Data.BookAuthor bookauthor)
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

        public async Task<Lilibre.Web.Models.Data.BookAuthor> CancelBookAuthorChanges(Lilibre.Web.Models.Data.BookAuthor item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBookAuthorUpdated(Lilibre.Web.Models.Data.BookAuthor item);
        partial void OnAfterBookAuthorUpdated(Lilibre.Web.Models.Data.BookAuthor item);

        public async Task<Lilibre.Web.Models.Data.BookAuthor> UpdateBookAuthor(int authorsid, int bookid, Lilibre.Web.Models.Data.BookAuthor bookauthor)
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

        partial void OnBookAuthorDeleted(Lilibre.Web.Models.Data.BookAuthor item);
        partial void OnAfterBookAuthorDeleted(Lilibre.Web.Models.Data.BookAuthor item);

        public async Task<Lilibre.Web.Models.Data.BookAuthor> DeleteBookAuthor(int authorsid, int bookid)
        {
            var itemToDelete = Context.BookAuthors
                              .Where(i => i.AuthorsId == authorsid && i.BookId == bookid)
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
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/data/bookgenres/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/data/bookgenres/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBookGenresToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/data/bookgenres/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/data/bookgenres/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBookGenresRead(ref IQueryable<Lilibre.Web.Models.Data.BookGenre> items);

        public async Task<IQueryable<Lilibre.Web.Models.Data.BookGenre>> GetBookGenres(Query query = null)
        {
            var items = Context.BookGenres.AsQueryable();

            items = items.Include(i => i.Book);
            items = items.Include(i => i.Genre);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnBookGenresRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBookGenreGet(Lilibre.Web.Models.Data.BookGenre item);
        partial void OnGetBookGenreByBookIdAndGenresId(ref IQueryable<Lilibre.Web.Models.Data.BookGenre> items);


        public async Task<Lilibre.Web.Models.Data.BookGenre> GetBookGenreByBookIdAndGenresId(int bookid, int genresid)
        {
            var items = Context.BookGenres
                              .AsNoTracking()
                              .Where(i => i.BookId == bookid && i.GenresId == genresid);

            items = items.Include(i => i.Book);
            items = items.Include(i => i.Genre);
 
            OnGetBookGenreByBookIdAndGenresId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnBookGenreGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBookGenreCreated(Lilibre.Web.Models.Data.BookGenre item);
        partial void OnAfterBookGenreCreated(Lilibre.Web.Models.Data.BookGenre item);

        public async Task<Lilibre.Web.Models.Data.BookGenre> CreateBookGenre(Lilibre.Web.Models.Data.BookGenre bookgenre)
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

        public async Task<Lilibre.Web.Models.Data.BookGenre> CancelBookGenreChanges(Lilibre.Web.Models.Data.BookGenre item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBookGenreUpdated(Lilibre.Web.Models.Data.BookGenre item);
        partial void OnAfterBookGenreUpdated(Lilibre.Web.Models.Data.BookGenre item);

        public async Task<Lilibre.Web.Models.Data.BookGenre> UpdateBookGenre(int bookid, int genresid, Lilibre.Web.Models.Data.BookGenre bookgenre)
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

        partial void OnBookGenreDeleted(Lilibre.Web.Models.Data.BookGenre item);
        partial void OnAfterBookGenreDeleted(Lilibre.Web.Models.Data.BookGenre item);

        public async Task<Lilibre.Web.Models.Data.BookGenre> DeleteBookGenre(int bookid, int genresid)
        {
            var itemToDelete = Context.BookGenres
                              .Where(i => i.BookId == bookid && i.GenresId == genresid)
                              .FirstOrDefault();

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
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/data/books/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/data/books/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBooksToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/data/books/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/data/books/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBooksRead(ref IQueryable<Lilibre.Web.Models.Data.Book> items);

        public async Task<IQueryable<Lilibre.Web.Models.Data.Book>> GetBooks(Query query = null)
        {
            var items = Context.Books.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnBooksRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBookGet(Lilibre.Web.Models.Data.Book item);
        partial void OnGetBookById(ref IQueryable<Lilibre.Web.Models.Data.Book> items);


        public async Task<Lilibre.Web.Models.Data.Book> GetBookById(int id)
        {
            var items = Context.Books
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetBookById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnBookGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBookCreated(Lilibre.Web.Models.Data.Book item);
        partial void OnAfterBookCreated(Lilibre.Web.Models.Data.Book item);

        public async Task<Lilibre.Web.Models.Data.Book> CreateBook(Lilibre.Web.Models.Data.Book book)
        {
            OnBookCreated(book);

            var existingItem = Context.Books
                              .Where(i => i.Id == book.Id)
                              .FirstOrDefault();

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

        public async Task<Lilibre.Web.Models.Data.Book> CancelBookChanges(Lilibre.Web.Models.Data.Book item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBookUpdated(Lilibre.Web.Models.Data.Book item);
        partial void OnAfterBookUpdated(Lilibre.Web.Models.Data.Book item);

        public async Task<Lilibre.Web.Models.Data.Book> UpdateBook(int id, Lilibre.Web.Models.Data.Book book)
        {
            OnBookUpdated(book);

            var itemToUpdate = Context.Books
                              .Where(i => i.Id == book.Id)
                              .FirstOrDefault();

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

        partial void OnBookDeleted(Lilibre.Web.Models.Data.Book item);
        partial void OnAfterBookDeleted(Lilibre.Web.Models.Data.Book item);

        public async Task<Lilibre.Web.Models.Data.Book> DeleteBook(int id)
        {
            var itemToDelete = Context.Books
                              .Where(i => i.Id == id)
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
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/data/genres/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/data/genres/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportGenresToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/data/genres/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/data/genres/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGenresRead(ref IQueryable<Lilibre.Web.Models.Data.Genre> items);

        public async Task<IQueryable<Lilibre.Web.Models.Data.Genre>> GetGenres(Query query = null)
        {
            var items = Context.Genres.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnGenresRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnGenreGet(Lilibre.Web.Models.Data.Genre item);
        partial void OnGetGenreById(ref IQueryable<Lilibre.Web.Models.Data.Genre> items);


        public async Task<Lilibre.Web.Models.Data.Genre> GetGenreById(int id)
        {
            var items = Context.Genres
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetGenreById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnGenreGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnGenreCreated(Lilibre.Web.Models.Data.Genre item);
        partial void OnAfterGenreCreated(Lilibre.Web.Models.Data.Genre item);

        public async Task<Lilibre.Web.Models.Data.Genre> CreateGenre(Lilibre.Web.Models.Data.Genre genre)
        {
            OnGenreCreated(genre);

            var existingItem = Context.Genres
                              .Where(i => i.Id == genre.Id)
                              .FirstOrDefault();

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

        public async Task<Lilibre.Web.Models.Data.Genre> CancelGenreChanges(Lilibre.Web.Models.Data.Genre item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnGenreUpdated(Lilibre.Web.Models.Data.Genre item);
        partial void OnAfterGenreUpdated(Lilibre.Web.Models.Data.Genre item);

        public async Task<Lilibre.Web.Models.Data.Genre> UpdateGenre(int id, Lilibre.Web.Models.Data.Genre genre)
        {
            OnGenreUpdated(genre);

            var itemToUpdate = Context.Genres
                              .Where(i => i.Id == genre.Id)
                              .FirstOrDefault();

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

        partial void OnGenreDeleted(Lilibre.Web.Models.Data.Genre item);
        partial void OnAfterGenreDeleted(Lilibre.Web.Models.Data.Genre item);

        public async Task<Lilibre.Web.Models.Data.Genre> DeleteGenre(int id)
        {
            var itemToDelete = Context.Genres
                              .Where(i => i.Id == id)
                              .Include(i => i.BookGenres)
                              .FirstOrDefault();

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
}