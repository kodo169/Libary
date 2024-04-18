using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Libary.Data;

public partial class LibaryContext : DbContext
{
    public LibaryContext()
    {
    }

    public LibaryContext(DbContextOptions<LibaryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<BillInfo> BillInfos { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:SQLServerConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PK__Authors__70DAFC147138037C");

            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.Idbill).HasName("PK__bill__57AF6205A9D39D20");

            entity.ToTable("bill");

            entity.Property(e => e.Idbill).HasColumnName("idbill");
        });

        modelBuilder.Entity<BillInfo>(entity =>
        {
            entity.HasKey(e => e.IdbillInfo).HasName("PK__billInfo__872F5E43A205DE9E");

            entity.ToTable("billInfo");

            entity.Property(e => e.IdbillInfo).HasColumnName("idbillInfo");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.CountBook)
                .HasDefaultValue(1)
                .HasColumnName("count_book");
            entity.Property(e => e.Idbill).HasColumnName("idbill");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Book).WithMany(p => p.BillInfos)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__billInfo__BookID__6BAEFA67");

            entity.HasOne(d => d.IdbillNavigation).WithMany(p => p.BillInfos)
                .HasForeignKey(d => d.Idbill)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_billInfo_bill");

            entity.HasOne(d => d.User).WithMany(p => p.BillInfos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__billInfo__UserID__6CA31EA0");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C227B7485959");

            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Content)
                .HasMaxLength(1000)
                .HasColumnName("content");
            entity.Property(e => e.NumberBook).HasColumnName("numberBook");
            entity.Property(e => e.PictureBook)
                .HasMaxLength(300)
                .HasColumnName("pictureBook");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_Authors");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_Categories");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B3C6E3195");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC58DFFF74");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E40B02F0CF").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105342ABEDD93").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
