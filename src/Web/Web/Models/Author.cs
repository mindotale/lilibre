using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lilibre.Web.Models
{
    [Table("Authors", Schema = "dbo")]
    public partial class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int BirthYear { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }

    }
}