using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MediaModel;

public partial class BooksContext : DbContext
{
    public BooksContext()
    {
    }

    public BooksContext(DbContextOptions<BooksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Audiobook> Audiobooks { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source=(localdb)\\mssqllocaldb; initial catalog=booksGolden");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Audiobook>(entity =>
        {
            entity.Property(e => e.Author).IsFixedLength();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Language).IsFixedLength();
            entity.Property(e => e.Narrator).IsFixedLength();
            entity.Property(e => e.PublicationYear).IsFixedLength();
            entity.Property(e => e.Publisher).IsFixedLength();
            entity.Property(e => e.Title).IsFixedLength();

            entity.HasOne(d => d.Book).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_audiobooks_books");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(e => e.Author).IsFixedLength();
            entity.Property(e => e.CoAuthor).IsFixedLength();
            entity.Property(e => e.Language).IsFixedLength();
            entity.Property(e => e.Publisher).IsFixedLength();
            entity.Property(e => e.Title).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
