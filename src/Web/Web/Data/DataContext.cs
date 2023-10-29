using System;
using System.Linq;
using System.Collections.Generic;

using Lilibre.Web.Models;

using Microsoft.EntityFrameworkCore;

namespace Lilibre.Web.Data
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BookAuthor>().HasKey(table => new {
                table.AuthorsId, table.BookId
            });

            builder.Entity<BookGenre>().HasKey(table => new {
                table.BookId, table.GenresId
            });

            builder.Entity<BookAuthor>()
              .HasOne(i => i.Author)
              .WithMany(i => i.BookAuthors)
              .HasForeignKey(i => i.AuthorsId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<BookAuthor>()
              .HasOne(i => i.Book)
              .WithMany(i => i.BookAuthors)
              .HasForeignKey(i => i.BookId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<BookGenre>()
              .HasOne(i => i.Book)
              .WithMany(i => i.BookGenres)
              .HasForeignKey(i => i.BookId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<BookGenre>()
              .HasOne(i => i.Genre)
              .WithMany(i => i.BookGenres)
              .HasForeignKey(i => i.GenresId)
              .HasPrincipalKey(i => i.Id);
            this.OnModelBuilding(builder);
        }

        public DbSet<Author> Authors { get; set; }

        public DbSet<BookAuthor> BookAuthors { get; set; }

        public DbSet<BookGenre> BookGenres { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Genre> Genres { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}