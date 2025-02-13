using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WalletHomework.Database.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAccount> TblAccounts { get; set; }

    public virtual DbSet<TblTransaction> TblTransactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=WINDOWS-1ISKG05\\SQLEXPRESS; Database=F9DB;Trusted_Connection=True;TrustServerCertificate=True;Integrated Security=SSPI;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId);

            entity.ToTable("Tbl_Account");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.MobileNo).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.PinNo).HasMaxLength(10);
            entity.Property(e => e.TransactionId)
                .HasMaxLength(20)
                .HasColumnName("TransactionID");
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId);

            entity.ToTable("Tbl_Transaction");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.About).HasMaxLength(50);
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Date).HasColumnType("date");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
