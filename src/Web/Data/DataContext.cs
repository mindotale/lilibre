using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Lilibre.Web.Models.Data;

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

            builder.Entity<Lilibre.Web.Models.Data.BookAuthor>().HasKey(table => new {
                table.AuthorsId, table.BookId
            });

            builder.Entity<Lilibre.Web.Models.Data.BookGenre>().HasKey(table => new {
                table.BookId, table.GenresId
            });

            builder.Entity<Lilibre.Web.Models.Data.BookAuthor>()
              .HasOne(i => i.Author)
              .WithMany(i => i.BookAuthors)
              .HasForeignKey(i => i.AuthorsId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Lilibre.Web.Models.Data.BookAuthor>()
              .HasOne(i => i.Book)
              .WithMany(i => i.BookAuthors)
              .HasForeignKey(i => i.BookId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Lilibre.Web.Models.Data.BookGenre>()
              .HasOne(i => i.Book)
              .WithMany(i => i.BookGenres)
              .HasForeignKey(i => i.BookId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<Lilibre.Web.Models.Data.BookGenre>()
              .HasOne(i => i.Genre)
              .WithMany(i => i.BookGenres)
              .HasForeignKey(i => i.GenresId)
              .HasPrincipalKey(i => i.Id);
            this.OnModelBuilding(builder);
        }

        public DbSet<Lilibre.Web.Models.Data.Author> Authors { get; set; }

        public DbSet<Lilibre.Web.Models.Data.BookAuthor> BookAuthors { get; set; }

        public DbSet<Lilibre.Web.Models.Data.BookGenre> BookGenres { get; set; }

        public DbSet<Lilibre.Web.Models.Data.Book> Books { get; set; }

        public DbSet<Lilibre.Web.Models.Data.Genre> Genres { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}