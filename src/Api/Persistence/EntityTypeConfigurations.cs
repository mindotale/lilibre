using Lilibre.Application;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lilibre.Persistence;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres");
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Name).IsRequired().HasMaxLength(255);
        builder.Property(g => g.Description).IsRequired().HasMaxLength(1000);
    }
}

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).IsRequired().HasMaxLength(255);
        builder.Property(a => a.Description).IsRequired().HasMaxLength(1000);
        builder.Property(a => a.BirthYear).IsRequired();
    }
}

public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder.ToTable("Publishers");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(255);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(1000);
    }
}

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Title).IsRequired().HasMaxLength(255);
        builder.Property(b => b.Description).IsRequired().HasMaxLength(1000);
        builder.Property(b => b.Isbn).IsRequired().HasMaxLength(20);
        builder.Property(b => b.Price).IsRequired();
        builder.Property(b => b.Pages).IsRequired();
        builder.Property(b => b.Year).IsRequired();

        builder
            .HasMany(b => b.Authors)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "BookAuthors",
                j => j.HasOne<Author>().WithMany(),
                j => j.HasOne<Book>().WithMany()
            );

        builder
            .HasMany(b => b.Genres)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "BookGenres",
                j => j.HasOne<Genre>().WithMany(),
                j => j.HasOne<Book>().WithMany()
            );

        builder.HasOne(b => b.Publisher)
            .WithMany().IsRequired();
    }
}

