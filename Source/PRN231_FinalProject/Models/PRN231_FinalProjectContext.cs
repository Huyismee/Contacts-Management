using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRN231_FinalProject.Models
{
    public partial class PRN231_FinalProjectContext : DbContext
    {
        public PRN231_FinalProjectContext()
        {
        }

        public PRN231_FinalProjectContext(DbContextOptions<PRN231_FinalProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<ContactEmail> ContactEmails { get; set; } = null!;
        public virtual DbSet<ContactPhone> ContactPhones { get; set; } = null!;
        public virtual DbSet<ContactsLabel> ContactsLabels { get; set; } = null!;
        public virtual DbSet<Label> Labels { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =HUYISME\\SQLEXPRESS; database = PRN231_FinalProject;uid=sa;pwd=123; trusted_connection = true; TrustServerCertificate=true ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.DeleteDate).HasColumnType("date");

                entity.Property(e => e.Firstname).HasMaxLength(50);

                entity.Property(e => e.History).HasColumnType("date");

                entity.Property(e => e.Lastname).HasMaxLength(50);

                entity.Property(e => e.Notes).HasMaxLength(500);

                entity.Property(e => e.ProfileImage).HasMaxLength(1000);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Contact_User");
            });

            modelBuilder.Entity<ContactEmail>(entity =>
            {
                entity.ToTable("ContactEmail");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Label).HasMaxLength(50);

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.ContactEmails)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("FK_ContactEmail_Contact");
            });

            modelBuilder.Entity<ContactPhone>(entity =>
            {
                entity.ToTable("ContactPhone");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Label).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.ContactPhones)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("FK_ContactPhone_Contact");
            });

            modelBuilder.Entity<ContactsLabel>(entity =>
            {
                entity.HasKey(e => e.ContactLabelId);

                entity.ToTable("ContactsLabel");

                entity.HasIndex(e => new { e.ContactId, e.LabelId }, "IX_ContactsLabel")
                    .IsUnique();

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.ContactsLabels)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("FK_ContactsLabel_Contact");

                entity.HasOne(d => d.Label)
                    .WithMany(p => p.ContactsLabels)
                    .HasForeignKey(d => d.LabelId)
                    .HasConstraintName("FK_ContactsLabel_Label");
            });

            modelBuilder.Entity<Label>(entity =>
            {
                entity.ToTable("Label");

                entity.Property(e => e.LabelName).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Labels)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Label_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
