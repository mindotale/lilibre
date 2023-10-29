using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lilibre.Web.Models.Data
{
    [Table("BookAuthors", Schema = "dbo")]
    public partial class BookAuthor
    {
        [Key]
        [Required]
        public int AuthorsId { get; set; }

        [Key]
        [Required]
        public int BookId { get; set; }

        public Author Author { get; set; }

        public Book Book { get; set; }

    }
}