using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MediaModel;

[Table("books")]
public partial class Book
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    [StringLength(50)]
    public string Title { get; set; } = null!;

    [Column("author")]
    [StringLength(25)]
    public string Author { get; set; } = null!;

    [Column("coAuthor")]
    [StringLength(25)]
    public string? CoAuthor { get; set; } = null!;

    [Column("language")]
    [StringLength(3)]
    public string Language { get; set; } = null!;

    [Column("publisher")]
    [StringLength(20)]
    public string Publisher { get; set; } = null!;

    [Column("publicationYear")]
    public int PublicationYear { get; set; }
    //public string Name { get; set; }
    //public string Name { get; set; }
}
