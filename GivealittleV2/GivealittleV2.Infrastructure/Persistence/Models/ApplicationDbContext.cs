using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GivealittleV2.Infrastructure.Persistence.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuthLoginAudit> AuthLoginAudits { get; set; }

    public virtual DbSet<AuthRefreshToken> AuthRefreshTokens { get; set; }

    public virtual DbSet<AuthRole> AuthRoles { get; set; }

    public virtual DbSet<AuthUser> AuthUsers { get; set; }

    public virtual DbSet<AuthUserRole> AuthUserRoles { get; set; }

    public virtual DbSet<BankAccount> BankAccounts { get; set; }

    public virtual DbSet<BankAccountDocument> BankAccountDocuments { get; set; }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<Cause> Causes { get; set; }

    public virtual DbSet<Charity> Charities { get; set; }

    public virtual DbSet<Entity> Entities { get; set; }

    public virtual DbSet<EntityAddress> EntityAddresses { get; set; }

    public virtual DbSet<EntityDocument> EntityDocuments { get; set; }

    public virtual DbSet<EntityDocumentType> EntityDocumentTypes { get; set; }

    public virtual DbSet<EntityEmail> EntityEmails { get; set; }

    public virtual DbSet<EntityIdentification> EntityIdentifications { get; set; }

    public virtual DbSet<EntityPhone> EntityPhones { get; set; }

    public virtual DbSet<EntityRelationship> EntityRelationships { get; set; }

    public virtual DbSet<EntityRelationshipRole> EntityRelationshipRoles { get; set; }

    public virtual DbSet<EntityRole> EntityRoles { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Individual> Individuals { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<SchoolClass> SchoolClasses { get; set; }

    public virtual DbSet<SchoolStudent> SchoolStudents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthLoginAudit>(entity =>
        {
            entity.ToTable("AuthLoginAudit");

            entity.HasIndex(e => new { e.AuthUserId, e.OccurredAtUtc }, "IX_AuthLoginAudit_AuthUserId_OccurredAtUtc");

            entity.HasIndex(e => e.OccurredAtUtc, "IX_AuthLoginAudit_OccurredAtUtc");

            entity.Property(e => e.Email).HasMaxLength(320);
            entity.Property(e => e.EventType).HasMaxLength(50);
            entity.Property(e => e.FailureReason).HasMaxLength(200);
            entity.Property(e => e.IpAddress).HasMaxLength(45);
            entity.Property(e => e.OccurredAtUtc)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.UserAgent).HasMaxLength(512);
        });

        modelBuilder.Entity<AuthRefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshTokenId);

            entity.HasIndex(e => new { e.AuthUserId, e.ExpiresAtUtc }, "IX_AuthRefreshTokens_AuthUserId_ExpiresAtUtc");

            entity.HasIndex(e => new { e.AuthUserId, e.RevokedAtUtc }, "IX_AuthRefreshTokens_AuthUserId_RevokedAtUtc");

            entity.HasIndex(e => e.TokenHash, "UX_AuthRefreshTokens_TokenHash").IsUnique();

            entity.Property(e => e.RefreshTokenId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatedByIp).HasMaxLength(45);
            entity.Property(e => e.ExpiresAtUtc).HasPrecision(0);
            entity.Property(e => e.RevokedAtUtc).HasPrecision(0);
            entity.Property(e => e.TokenHash).HasMaxLength(32);
            entity.Property(e => e.UserAgent).HasMaxLength(512);

            entity.HasOne(d => d.AuthUser).WithMany(p => p.AuthRefreshTokens)
                .HasForeignKey(d => d.AuthUserId)
                .HasConstraintName("FK_AuthRefreshTokens_AuthUsers");

            entity.HasOne(d => d.ReplacedByRefreshToken).WithMany(p => p.InverseReplacedByRefreshToken)
                .HasForeignKey(d => d.ReplacedByRefreshTokenId)
                .HasConstraintName("FK_AuthRefreshTokens_ReplacedBy");
        });

        modelBuilder.Entity<AuthRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "UX_AuthRoles_NormalizedName").IsUnique();

            entity.Property(e => e.AuthRoleId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name).HasMaxLength(128);
            entity.Property(e => e.NormalizedName).HasMaxLength(128);
        });

        modelBuilder.Entity<AuthUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "UX_AuthUsers_NormalizedEmail").IsUnique();

            entity.Property(e => e.AuthUserId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(320);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastLoginAtUtc).HasPrecision(0);
            entity.Property(e => e.LockoutUntilUtc).HasPrecision(0);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(320);
            entity.Property(e => e.PasswordChangedAtUtc).HasPrecision(0);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.UpdatedAtUtc)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<AuthUserRole>(entity =>
        {
            entity.HasKey(e => new { e.AuthUserId, e.AuthRoleId });

            entity.HasIndex(e => e.AuthRoleId, "IX_AuthUserRoles_AuthRoleId");

            entity.Property(e => e.AssignedAtUtc)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.AuthRole).WithMany(p => p.AuthUserRoles)
                .HasForeignKey(d => d.AuthRoleId)
                .HasConstraintName("FK_AuthUserRoles_AuthRoles");

            entity.HasOne(d => d.AuthUser).WithMany(p => p.AuthUserRoles)
                .HasForeignKey(d => d.AuthUserId)
                .HasConstraintName("FK_AuthUserRoles_AuthUsers");
        });

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.ToTable("BankAccount");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.EntityId).HasColumnName("EntityID");
            entity.Property(e => e.Reference).HasMaxLength(50);

            entity.HasOne(d => d.Entity).WithMany(p => p.BankAccounts)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("FK_BankAccount_Entity");
        });

        modelBuilder.Entity<BankAccountDocument>(entity =>
        {
            entity.ToTable("BankAccountDocument");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.BankAccountId).HasColumnName("BankAccountID");

            entity.HasOne(d => d.BankAccount).WithMany(p => p.BankAccountDocuments)
                .HasForeignKey(d => d.BankAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BankAccountDocument_BankAccount");
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.ToTable("Business");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Business)
                .HasForeignKey<Business>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Business_NonIndividual1");
        });

        modelBuilder.Entity<Cause>(entity =>
        {
            entity.ToTable("Cause");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Charity>(entity =>
        {
            entity.ToTable("Charity");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Ccnumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("CCNumber");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Charity)
                .HasForeignKey<Charity>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Charity_Entity");
        });

        modelBuilder.Entity<Entity>(entity =>
        {
            entity.ToTable("Entity");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Irdnumber)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("IRDNumber");
        });

        modelBuilder.Entity<EntityAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ContactAddress");

            entity.ToTable("EntityAddress");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.EntityId).HasColumnName("EntityID");

            entity.HasOne(d => d.Entity).WithMany(p => p.EntityAddresses)
                .HasForeignKey(d => d.EntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EntityAddress_Entity");
        });

        modelBuilder.Entity<EntityDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_EntityDocuments");

            entity.ToTable("EntityDocument");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.EntityDocumentTypeId).HasColumnName("EntityDocumentTypeID");
            entity.Property(e => e.EntityId).HasColumnName("EntityID");

            entity.HasOne(d => d.EntityDocumentType).WithMany(p => p.EntityDocuments)
                .HasForeignKey(d => d.EntityDocumentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EntityDocuments_EntityDocumentTypes");

            entity.HasOne(d => d.Entity).WithMany(p => p.EntityDocuments)
                .HasForeignKey(d => d.EntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EntityDocuments_Entity");
        });

        modelBuilder.Entity<EntityDocumentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_EntityDocumentTypes");

            entity.ToTable("EntityDocumentType");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.DocumentType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EntityEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ContactEmail");

            entity.ToTable("EntityEmail");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.EmailAddress).HasMaxLength(50);
            entity.Property(e => e.EntityId).HasColumnName("EntityID");

            entity.HasOne(d => d.Entity).WithMany(p => p.EntityEmails)
                .HasForeignKey(d => d.EntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EntityEmail_Entity");
        });

        modelBuilder.Entity<EntityIdentification>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("EntityIdentification");

            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.DrivingLicenseNumer).HasMaxLength(4096);
            entity.Property(e => e.DrivingLicenseVersion).HasMaxLength(4096);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PassportExpiryDate).HasMaxLength(4096);
            entity.Property(e => e.PassportNumber).HasMaxLength(4096);

            entity.HasOne(d => d.Document).WithMany()
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EntityIdentification_EntityDocuments");

            entity.HasOne(d => d.IdNavigation).WithMany()
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EntityIdentification_Entity");
        });

        modelBuilder.Entity<EntityPhone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ContactPhone");

            entity.ToTable("EntityPhone");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.EntityId).HasColumnName("EntityID");
            entity.Property(e => e.Number).HasMaxLength(50);

            entity.HasOne(d => d.Entity).WithMany(p => p.EntityPhones)
                .HasForeignKey(d => d.EntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EntityPhone_Entity");
        });

        modelBuilder.Entity<EntityRelationship>(entity =>
        {
            entity.ToTable("EntityRelationship");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.EntityId).HasColumnName("EntityID");
            entity.Property(e => e.RelatedEntityId).HasColumnName("RelatedEntityID");

            entity.HasOne(d => d.Entity).WithMany(p => p.EntityRelationshipEntities)
                .HasForeignKey(d => d.EntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EntityRelationship_Individual");

            entity.HasOne(d => d.RelatedEntity).WithMany(p => p.EntityRelationshipRelatedEntities)
                .HasForeignKey(d => d.RelatedEntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EntityRelationship_NonIndividual");
        });

        modelBuilder.Entity<EntityRelationshipRole>(entity =>
        {
            entity.ToTable("EntityRelationshipRole");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntityRelationshipId).HasColumnName("EntityRelationshipID");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.EntityRelationship).WithMany(p => p.EntityRelationshipRoles)
                .HasForeignKey(d => d.EntityRelationshipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EntityRelationshipRole_EntityRelationship");
        });

        modelBuilder.Entity<EntityRole>(entity =>
        {
            entity.ToTable("EntityRole");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.EntityId).HasColumnName("EntityID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Entity).WithMany(p => p.EntityRoles)
                .HasForeignKey(d => d.EntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EntityRole_Entity");

            entity.HasOne(d => d.Role).WithMany(p => p.EntityRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EntityRole_Role");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.ToTable("Group");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Group)
                .HasForeignKey<Group>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Group_Entity");
        });

        modelBuilder.Entity<Individual>(entity =>
        {
            entity.ToTable("Individual");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Individual)
                .HasForeignKey<Individual>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Individual_Entity");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.ToTable("School");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.School)
                .HasForeignKey<School>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_School_Entity");
        });

        modelBuilder.Entity<SchoolClass>(entity =>
        {
            entity.ToTable("SchoolClass");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<SchoolStudent>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SchoolStudent");

            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.IndividualId).HasColumnName("IndividualID");
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");

            entity.HasOne(d => d.Class).WithMany()
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SchoolStudent_SchoolClass");

            entity.HasOne(d => d.Individual).WithMany()
                .HasForeignKey(d => d.IndividualId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SchoolStudent_Individual");

            entity.HasOne(d => d.School).WithMany()
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SchoolStudent_School");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
