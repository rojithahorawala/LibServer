using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MediaModel;

[Keyless]
[Table("audiobooks")]
public partial class Audiobook
{
    [Column("id")]
    public int Id { get; set; }

    [Column("bookid")]
    public int Bookid { get; set; }

    [Column("title")]
    [StringLength(30)]
    public string Title { get; set; } = null!;

    [Column("author")]
    [StringLength(10)]
    public string Author { get; set; } = null!;

    [Column("language")]
    [StringLength(3)]
    public string Language { get; set; } = null!;

    [Column("narrator")]
    [StringLength(20)]
    public string Narrator { get; set; } = null!;

    [Column("publisher")]
    [StringLength(20)]
    public string Publisher { get; set; } = null!;

    [Column("publicationYear")]
    [StringLength(4)]
    public string PublicationYear { get; set; } = null!;

    [ForeignKey("Bookid")]
    public virtual Book Book { get; set; } = null!;
}
