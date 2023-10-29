using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lilibre.Web.Models
{
    [Table("BookGenres", Schema = "dbo")]
    public partial class BookGenre
    {
        [Key]
        [Required]
        public int BookId { get; set; }

        [Key]
        [Required]
        public int GenresId { get; set; }

        public Book Book { get; set; }

        public Genre Genre { get; set; }

    }
}