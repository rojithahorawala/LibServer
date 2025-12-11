using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

namespace MediaModel;

public partial class BooksContext : IdentityDbContext<MediaUserModel>
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


        //if (!optionsBuilder.IsConfigured)
        //{
        //    var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
        //        .AddJsonFile("appsettings.json", optional: true)
        //        .AddJsonFile("appsettings.Development.json", optional: true)
        //        .AddEnvironmentVariables()
        //        .Build();

        //    var cs = config.GetConnectionString("DefaultConnection")
        //                ?? "Server=(localdb)\\mssqllocaldb;Initial Catalog=booksGolden;TrustServerCertificate=True";

        //    optionsBuilder.UseSqlServer(cs);
        //}

        IConfigurationBuilder builder = new ConfigurationBuilder()
         .AddJsonFile("appsettings.json").AddJsonFile("appsettings.Development.json", optional: true);

        IConfiguration config = builder.Build();
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 
        modelBuilder.Entity<Audiobook>(entity =>
        {
            entity.Property(e => e.Author).IsFixedLength();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Language).IsFixedLength();
            entity.Property(e => e.Narrator).IsFixedLength();
            entity.Property(e => e.PublicationYear).IsFixedLength();
            entity.Property(e => e.Publisher).IsFixedLength();
            entity.Property(e => e.Title).IsFixedLength();

            entity.HasKey(a => a.Id);          
            entity.ToTable("audiobooks");      
            entity.Property(a => a.Id).ValueGeneratedOnAdd();
            entity.HasOne(a => a.Book)
                  .WithOne()                  
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
