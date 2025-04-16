using System;
using System.Collections.Generic;
using System.Reflection;
using Api_1.Entity.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Tourism_Api.model.Context;

public partial class TourismContext : IdentityDbContext<User, UserRole, string>
{
    public TourismContext()
    {
    }

    public TourismContext(DbContextOptions<TourismContext> options)
        : base(options)
    {
    }

    public DbSet<PlaceRate> placeRates { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Governorate> Governorates { get; set; }

    public virtual DbSet<Place> Places { get; set; }
    public virtual DbSet<UserProgram> UserProgram { get; set; }

    public virtual DbSet<Program> Programs { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<ProgramsPhoto> ProgramsPhotos { get; set; }

    public virtual DbSet<TypeOfTourism> TypeOfTourisms { get; set; }

    public virtual DbSet<TourguidAndPlaces> TourguidAndPlaces { get; set; }

    public virtual DbSet<FavoritePlace> FavoritePlaces { get; set; }
    
    public virtual DbSet<TripsPlaces> TripsPlaces { get; set; }

    public virtual DbSet<Type_of_Tourism_Places> Type_of_Tourism_Places { get; set; }


    public virtual DbSet<Trips> Trips { get; set; }
    
    public virtual DbSet<Tourguid_Rate> Tourguid_Rates { get; set; }


    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.

        optionsBuilder.ConfigureWarnings(warnings =>
        warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

       // optionsBuilder.UseSqlServer
       // ("Server = DESKTOP-GNTBPMJ ; Database = Tourism2 ; User ID=Tourism_User ; Password=Q31KIewm5s7Ldp1w ; Encrypt=False ");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comments__3214EC07BE7AB12A");

            entity.Property(e => e.Content).IsUnicode(false);
            entity.Property(e => e.PlaceName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Place_Name");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Comments__UserI__2A4B4B5E");

            entity.HasOne(d => d.PlaceNameNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PlaceName)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Comments__Place___2B3F6F97");
        });

        modelBuilder.Entity<TourguidAndPlaces>()
          .HasKey(up => new { up.TouguidId });

        modelBuilder.Entity<PlaceRate>()
           .HasKey(up => new { up.UserId, up.PlaceName });

        modelBuilder.Entity<Tourguid_Rate>(entity =>
        {
            entity.HasKey(up => new { up.tourguidId, up.userId });

            entity.HasOne(r => r.User)
            .WithMany(u => u.Tourguid_Rates)
            .HasForeignKey(r => r.userId)
            .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.rate)
                  .IsRequired();
            entity.HasCheckConstraint("CK_YourEntity_Rate_Range", "[rate] >= 1 AND [rate] <= 5");
        });

        modelBuilder.Entity<TripsPlaces>()
           .HasKey(up => new { up.TripName, up.PlaceName });

        modelBuilder.Entity<FavoritePlace>()
           .HasKey(up => new { up.UserId, up.PlaceName });

        modelBuilder.Entity<Type_of_Tourism_Places>()
           .HasKey(up => new { up.Tourism_Name, up.Place_Name });

        modelBuilder.Entity<Governorate>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PK__Governor__737584F7DFCB6029");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Photo)
                .HasMaxLength(355)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Place>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PK__Places__737584F77499CA45");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.GovernmentName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Government_name");
            entity.Property(e => e.Location)
                .HasMaxLength(355)
                .IsUnicode(false);
            entity.Property(e => e.Photo)
                .HasMaxLength(355)
                .IsUnicode(false);
            entity.Property(e => e.Rate).HasColumnType("decimal(5, 2)");

            entity.Property(e => e.GoogleRate).HasColumnType("decimal(5,2)");

            entity.Property(p => p.Rate)
            .HasDefaultValue(0.0m);

            entity.Property(p => p.GoogleRate)
            .HasDefaultValue(0.0m);

            entity.HasOne(d => d.GovernmentNameNavigation).WithMany(p => p.Places)
                .HasForeignKey(d => d.GovernmentName)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Places__Governme__21B6055D");
        });

        modelBuilder.Entity<Program>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PK__Programs__737584F73BE6336D");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Activities).HasColumnType("text");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasMany(d => d.PlaceNames).WithMany(p => p.ProgramNames)
                .UsingEntity<Dictionary<string, object>>(
                    "ProgramPlace",
                    r => r.HasOne<Place>().WithMany()
                        .HasForeignKey("PlaceName")
                        .HasConstraintName("FK__Program_P__Place__403A8C7D"),
                    l => l.HasOne<Program>().WithMany()
                        .HasForeignKey("ProgramName")
                        .HasConstraintName("FK__Program_P__Progr__3F466844"),
                    j =>
                    {
                        j.HasKey("ProgramName", "PlaceName").HasName("PK__Program___3088689D29880B4F");
                        j.ToTable("Program_Places");
                        j.IndexerProperty<string>("ProgramName")
                            .HasMaxLength(255)
                            .IsUnicode(false)
                            .HasColumnName("Program_Name");
                        j.IndexerProperty<string>("PlaceName")
                            .HasMaxLength(255)
                            .IsUnicode(false)
                            .HasColumnName("Place_Name");
                    });
        });

        modelBuilder.Entity<ProgramsPhoto>(entity =>
        {
            entity.HasKey(e => new { e.ProgramName , e.Photo }).HasName("PK__Programs__983F9C28780D659C");

            entity.ToTable("Programs_Photo");

            entity.Property(e => e.ProgramName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Program_Name");
            entity.Property(e => e.Photo)
                .HasMaxLength(355)
                .IsUnicode(false);

            entity.HasOne(d => d.ProgramNameNavigation).WithMany(p => p.ProgramsPhotos)
                .HasForeignKey(d => d.ProgramName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Programs___Progr__3C69FB99");
        });

        modelBuilder.Entity<TypeOfTourism>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PK__Type_of___737584F7C5CDEEEF");

            entity.ToTable("Type_of_Tourism");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Photo)
                .HasMaxLength(255)
                .IsUnicode(false);

            //entity.HasMany(d => d.PlaceNames).WithMany(p => p.TourismNames)
            //    .UsingEntity<Dictionary<string, object>>(
            //        "Type_of_Tourism_Places",
            //        r => r.HasOne<Place>().WithMany()
            //            .HasForeignKey("PlaceName")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK__Type_of_T__Place__33D4B598"),
            //        l => l.HasOne<TypeOfTourism>().WithMany()
            //            .HasForeignKey("TourismName")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK__Type_of_T__Touri__32E0915F"),
            //        j =>
            //        {
            //            j.HasKey("TourismName", "PlaceName").HasName("PK__Type_of___C27BE45AEE2DFDFA");
            //            j.ToTable("Type_of_Tourism_Places");
            //            j.IndexerProperty<string>("TourismName")
            //                .HasMaxLength(255)
            //                .IsUnicode(false)
            //                .HasColumnName("Tourism_Name");
            //            j.IndexerProperty<string>("PlaceName")
            //                .HasMaxLength(255)
            //                .IsUnicode(false)
            //                .HasColumnName("Place_Name");
            //        });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07EBFEDA1A");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534F4661017").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Photo)
                .HasMaxLength(355)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TourguidId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Tourguidid");


            entity
            .OwnsMany(x => x.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");

            entity.HasOne(d => d.Tourguid).WithMany(p => p.InverseTourguid)
                .HasForeignKey(d => d.TourguidId)
                .HasConstraintName("FK__Users__Tourguid___1CF15040");

            //entity.HasMany(d => d.PlaceNames).WithMany(p => p.Tourguids)
            //    .UsingEntity<Dictionary<string, object>>(
            //        "TourguidPlace",
            //        r => r.HasOne<Place>().WithMany()
            //            .HasForeignKey("PlaceName")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK__Tourguid___Place__286302EC"),
            //        l => l.HasOne<User>().WithMany()
            //            .HasForeignKey("TourguidId")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK__Tourguid___Tourg__276EDEB3")

            //        //j =>
            //        //{
            //        //    j.HasKey("TourguidId", "PlaceName").HasName("PK__Tourguid__88E9D762BAF1713A");
            //        //    j.ToTable("Tourguid_Places");
            //        //    j.IndexerProperty<string>("TourguidId")
            //        //        .HasMaxLength(255)
            //        //        .IsUnicode(false)
            //        //        .HasColumnName("Tourguidid");
            //        //    j.IndexerProperty<string>("PlaceName")
            //        //        .HasMaxLength(255)
            //        //        .IsUnicode(false)
            //        //        .HasColumnName("Place_Name");
            //        //}
            //    );
            var passwordHasher = new PasswordHasher<User>();

            entity.HasData(new User
            {
                Id = DefaultUsers.AdminId,
                Name = "Admin",
                Country = "Egypt",
                Gender = "Male",
                Phone = "01151813561",
                Role = "Admin",
                Email = DefaultUsers.AdminEmail,
                UserName = DefaultUsers.AdminEmail,
                NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
                NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
                Password = DefaultUsers.AdminPassword,
                SecurityStamp = DefaultUsers.AdminSecurityStamp,
                ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
                EmailConfirmed = true,
                PasswordHash = passwordHasher.HashPassword(null!, DefaultUsers.AdminPassword)
            });
        });



        modelBuilder.Entity<UserRole>(entity =>
            {

                entity.ToTable("AspNetRoles");

                entity.Property(e => e.Id).HasMaxLength(450);
                entity.Property(e => e.Name).HasMaxLength(256);
                entity.Property(e => e.NormalizedName).HasMaxLength(256);
                entity.Property(e => e.ConcurrencyStamp).HasMaxLength(256);
                entity.HasKey(e => e.Id);

                entity.HasData([
                       new UserRole
                       {
                            Id = DefaultRoles.AdminRoleId,
                            Name = DefaultRoles.Admin,
                            NormalizedName = DefaultRoles.Admin.ToUpper(),
                            ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp ,
                            IsDefault = false,
                            IsDeleted = false

                       },
                       new UserRole
                       {
                            Id = DefaultRoles.MemberRoleId,
                            Name = DefaultRoles.Member,
                            NormalizedName = DefaultRoles.Member.ToUpper(),
                            ConcurrencyStamp = DefaultRoles.MemberRoleConcurrencyStamp,
                            IsDefault = true,
                            IsDeleted = false
                       },
                       new UserRole
                       {
                            Id = DefaultRoles.TourguidId,
                            Name = DefaultRoles.Tourguid,
                            NormalizedName = DefaultRoles.Tourguid.ToUpper(),
                            ConcurrencyStamp = DefaultRoles.TourguidRoleConcurrencyStamp,
                            IsDefault = false,
                            IsDeleted = false
                       }
                ]);


            });

            // Configure IdentityUserLogin<TKey> (AspNetUserLogins)
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
                entity.ToTable("IdentityUserLogin");

            });

            // Configure IdentityUserRole<TKey> (AspNetUserRoles)
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
                entity.ToTable("IdentityUserRole");

                entity.HasData(new IdentityUserRole<string>
                {
                    UserId = DefaultUsers.AdminId,
                    RoleId = DefaultRoles.AdminRoleId
                });

            });

            // Configure IdentityUserToken<TKey> (AspNetUserTokens)
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
                entity.ToTable("IdentityUserToken");

            });

            // Configure IdentityRoleClaim<TKey> (AspNetRoleClaims)
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.HasKey(e => e.Id); // Primary key is already defined, but adding explicitly for consistency
                entity.ToTable("IdentityRoleClaim");

            });

            // Configure IdentityUserClaim<TKey> (AspNetUserClaims)
            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.HasKey(e => e.Id); // Primary key is already defined, but adding explicitly for consistency
                entity.ToTable("IdentityUserClaim");

            });


            OnModelCreatingPartial(modelBuilder);

    }
     
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
  
}
