using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

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
    {


        if (!optionsBuilder.IsConfigured)
        {
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var cs = config.GetConnectionString("DefaultConnection")
                        ?? "Server=(localdb)\\mssqllocaldb;Initial Catalog=booksGolden;TrustServerCertificate=True";

            optionsBuilder.UseSqlServer(cs);
        }
    }


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

            entity.HasKey(a => a.Id);           // explicit, in case naming isn’t standard
            entity.ToTable("audiobooks");       // match your actual table
            entity.Property(a => a.Id).ValueGeneratedOnAdd();
            entity.HasOne(a => a.Book)
                  .WithOne()                    // or .WithMany(b => b.Audiobooks) if 1:N
                  .HasForeignKey<Audiobook>(a => a.Bookid);

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
