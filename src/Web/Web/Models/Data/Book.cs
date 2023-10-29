using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lilibre.Web.Models.Data
{
    [Table("Books", Schema = "dbo")]
    public partial class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Isbn { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Pages { get; set; }

        [Required]
        public int Year { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }

        public ICollection<BookGenre> BookGenres { get; set; }

    }
}