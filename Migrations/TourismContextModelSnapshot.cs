﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tourism_Api.model.Context;

#nullable disable

namespace Tourism_Api.Migrations
{
    [DbContext(typeof(TourismContext))]
    partial class TourismContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IdentityRoleClaim", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IdentityUserClaim", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.ToTable("IdentityUserLogin", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("IdentityUserRole", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "6dc6528a-b280-4770-9eae-82671ee81ef7",
                            RoleId = "92b75286-d8f8-4061-9995-e6e23ccdee94"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("IdentityUserToken", (string)null);
                });

            modelBuilder.Entity("ProgramPlace", b =>
                {
                    b.Property<string>("ProgramName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("Program_Name");

                    b.Property<string>("PlaceName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("Place_Name");

                    b.HasKey("ProgramName", "PlaceName")
                        .HasName("PK__Program___3088689D29880B4F");

                    b.HasIndex("PlaceName");

                    b.ToTable("Program_Places", (string)null);
                });

            modelBuilder.Entity("Tourism_Api.model.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<string>("PlaceName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("Place_Name");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__Comments__3214EC07BE7AB12A");

                    b.HasIndex("PlaceName");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Tourism_Api.model.FavoritePlace", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PlaceName")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "PlaceName");

                    b.HasIndex("PlaceName");

                    b.ToTable("FavoritePlaces");
                });

            modelBuilder.Entity("Tourism_Api.model.Governorate", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Photo")
                        .HasMaxLength(355)
                        .IsUnicode(false)
                        .HasColumnType("varchar(355)");

                    b.HasKey("Name")
                        .HasName("PK__Governor__737584F7DFCB6029");

                    b.ToTable("Governorates");
                });

            modelBuilder.Entity("Tourism_Api.model.Place", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("GovernmentName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("Government_name");

                    b.Property<string>("Location")
                        .HasMaxLength(355)
                        .IsUnicode(false)
                        .HasColumnType("varchar(355)");

                    b.Property<string>("Photo")
                        .HasMaxLength(355)
                        .IsUnicode(false)
                        .HasColumnType("varchar(355)");

                    b.Property<decimal?>("Rate")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<string>("VisitingHours")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Name")
                        .HasName("PK__Places__737584F77499CA45");

                    b.HasIndex("GovernmentName");

                    b.ToTable("Places");
                });

            modelBuilder.Entity("Tourism_Api.model.PlaceRate", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PlaceName")
                        .HasColumnType("varchar(255)");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("UserId", "PlaceName");

                    b.HasIndex("PlaceName");

                    b.ToTable("placeRates");
                });

            modelBuilder.Entity("Tourism_Api.model.Program", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Activities")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("Name")
                        .HasName("PK__Programs__737584F73BE6336D");

                    b.ToTable("Programs");
                });

            modelBuilder.Entity("Tourism_Api.model.ProgramsPhoto", b =>
                {
                    b.Property<string>("ProgramName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("Program_Name");

                    b.Property<string>("Photo")
                        .HasMaxLength(355)
                        .IsUnicode(false)
                        .HasColumnType("varchar(355)");

                    b.HasKey("ProgramName", "Photo")
                        .HasName("PK__Programs__983F9C28780D659C");

                    b.ToTable("Programs_Photo", (string)null);
                });

            modelBuilder.Entity("Tourism_Api.model.TourguidAndPlaces", b =>
                {
                    b.Property<string>("TouguidId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PlaceName")
                        .HasColumnType("varchar(255)");

                    b.HasKey("TouguidId", "PlaceName");

                    b.HasIndex("PlaceName");

                    b.ToTable("TourguidAndPlaces");
                });

            modelBuilder.Entity("Tourism_Api.model.TypeOfTourism", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Photo")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Name")
                        .HasName("PK__Type_of___737584F7C5CDEEEF");

                    b.ToTable("Type_of_Tourism", (string)null);
                });

            modelBuilder.Entity("Tourism_Api.model.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("BirthDate")
                        .HasColumnType("date");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Gender")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Photo")
                        .HasMaxLength(355)
                        .IsUnicode(false)
                        .HasColumnType("varchar(355)");

                    b.Property<string>("Role")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TourguidId")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("Tourguidid");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("PK__Users__3214EC07EBFEDA1A");

                    b.HasIndex("TourguidId");

                    b.HasIndex(new[] { "Email" }, "UQ__Users__A9D10534F4661017")
                        .IsUnique();

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = "6dc6528a-b280-4770-9eae-82671ee81ef7",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "99d2bbc6-bc54-4248-a172-a77de3ae4430",
                            Country = "Egypt",
                            Email = "admin@gmail.com",
                            EmailConfirmed = true,
                            Gender = "Male",
                            LockoutEnabled = false,
                            Name = "Admin",
                            NormalizedEmail = "ADMIN@GMAIL.COM",
                            NormalizedUserName = "ADMIN@GMAIL.COM",
                            Password = "P@ssword123",
                            PasswordHash = "AQAAAAIAAYagAAAAEKf5oZglWRCePUcvOzYL+inrvbtAleDtE41Wz0sXKBNHwWJQgARvX5WnZDdcu5GFew==",
                            Phone = "01151813561",
                            PhoneNumberConfirmed = false,
                            Role = "Admin",
                            SecurityStamp = "55BF92C9EF0249CDA210D85D1A851BC9",
                            TwoFactorEnabled = false,
                            UserName = "admin@gmail.com"
                        });
                });

            modelBuilder.Entity("Tourism_Api.model.UserAswers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ProgramName")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("Question1")
                        .HasColumnType("bit");

                    b.Property<bool>("Question10")
                        .HasColumnType("bit");

                    b.Property<bool>("Question2")
                        .HasColumnType("bit");

                    b.Property<bool>("Question3")
                        .HasColumnType("bit");

                    b.Property<bool>("Question4")
                        .HasColumnType("bit");

                    b.Property<bool>("Question5")
                        .HasColumnType("bit");

                    b.Property<bool>("Question6")
                        .HasColumnType("bit");

                    b.Property<bool>("Question7")
                        .HasColumnType("bit");

                    b.Property<bool>("Question8")
                        .HasColumnType("bit");

                    b.Property<bool>("Question9")
                        .HasColumnType("bit");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("ProgramName");

                    b.HasIndex("UserId");

                    b.ToTable("UserAswers");
                });

            modelBuilder.Entity("Tourism_Api.model.UserRole", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "92b75286-d8f8-4061-9995-e6e23ccdee94",
                            ConcurrencyStamp = "f51e5a91-bced-49c2-8b86-c2e170c0846c",
                            IsDefault = false,
                            IsDeleted = false,
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4",
                            ConcurrencyStamp = "5ee6bc12-5cb0-4304-91e7-6a00744e042a",
                            IsDefault = true,
                            IsDeleted = false,
                            Name = "Member",
                            NormalizedName = "MEMBER"
                        },
                        new
                        {
                            Id = "f78b99da-c771-4b4a-8c85-e83079dc228c",
                            ConcurrencyStamp = "4f98002a-a963-4043-a4e5-100592050714",
                            IsDefault = false,
                            IsDeleted = false,
                            Name = "Tourguid",
                            NormalizedName = "TOURGUID"
                        });
                });

            modelBuilder.Entity("TypeOfTourismPlace", b =>
                {
                    b.Property<string>("TourismName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("Tourism_Name");

                    b.Property<string>("PlaceName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("Place_Name");

                    b.HasKey("TourismName", "PlaceName")
                        .HasName("PK__Type_of___C27BE45AEE2DFDFA");

                    b.HasIndex("PlaceName");

                    b.ToTable("Type_of_Tourism_Places", (string)null);
                });

            modelBuilder.Entity("ProgramPlace", b =>
                {
                    b.HasOne("Tourism_Api.model.Place", null)
                        .WithMany()
                        .HasForeignKey("PlaceName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Program_P__Place__403A8C7D");

                    b.HasOne("Tourism_Api.model.Program", null)
                        .WithMany()
                        .HasForeignKey("ProgramName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Program_P__Progr__3F466844");
                });

            modelBuilder.Entity("Tourism_Api.model.Comment", b =>
                {
                    b.HasOne("Tourism_Api.model.Place", "PlaceNameNavigation")
                        .WithMany("Comments")
                        .HasForeignKey("PlaceName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__Comments__Place___2B3F6F97");

                    b.HasOne("Tourism_Api.model.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Comments__UserI__2A4B4B5E");

                    b.Navigation("PlaceNameNavigation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Tourism_Api.model.FavoritePlace", b =>
                {
                    b.HasOne("Tourism_Api.model.Place", "Place")
                        .WithMany("FavoritePlaces")
                        .HasForeignKey("PlaceName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tourism_Api.model.User", "User")
                        .WithMany("FavoritePlaces")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Place");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Tourism_Api.model.Place", b =>
                {
                    b.HasOne("Tourism_Api.model.Governorate", "GovernmentNameNavigation")
                        .WithMany("Places")
                        .HasForeignKey("GovernmentName")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK__Places__Governme__21B6055D");

                    b.Navigation("GovernmentNameNavigation");
                });

            modelBuilder.Entity("Tourism_Api.model.PlaceRate", b =>
                {
                    b.HasOne("Tourism_Api.model.Place", "Place")
                        .WithMany("PlaceRates")
                        .HasForeignKey("PlaceName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tourism_Api.model.User", "User")
                        .WithMany("PlaceRates")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Place");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Tourism_Api.model.ProgramsPhoto", b =>
                {
                    b.HasOne("Tourism_Api.model.Program", "ProgramNameNavigation")
                        .WithMany("ProgramsPhotos")
                        .HasForeignKey("ProgramName")
                        .IsRequired()
                        .HasConstraintName("FK__Programs___Progr__3C69FB99");

                    b.Navigation("ProgramNameNavigation");
                });

            modelBuilder.Entity("Tourism_Api.model.TourguidAndPlaces", b =>
                {
                    b.HasOne("Tourism_Api.model.Place", "Place")
                        .WithMany("TourguidAndPlaces")
                        .HasForeignKey("PlaceName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tourism_Api.model.User", "Touguid")
                        .WithMany("TourguidAndPlaces")
                        .HasForeignKey("TouguidId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Place");

                    b.Navigation("Touguid");
                });

            modelBuilder.Entity("Tourism_Api.model.User", b =>
                {
                    b.HasOne("Tourism_Api.model.User", "Tourguid")
                        .WithMany("InverseTourguid")
                        .HasForeignKey("TourguidId")
                        .HasConstraintName("FK__Users__Tourguid___1CF15040");

                    b.OwnsMany("Tourism_Api.model.RefreshToken", "RefreshTokens", b1 =>
                        {
                            b1.Property<string>("UserId")
                                .HasColumnType("varchar(255)");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<DateTime>("CreatedOn")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime>("ExpiresOn")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime?>("RevokedOn")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Token")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("UserId", "Id");

                            b1.ToTable("RefreshTokens", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("RefreshTokens");

                    b.Navigation("Tourguid");
                });

            modelBuilder.Entity("Tourism_Api.model.UserAswers", b =>
                {
                    b.HasOne("Tourism_Api.model.Program", "Program")
                        .WithMany()
                        .HasForeignKey("ProgramName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tourism_Api.model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Program");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TypeOfTourismPlace", b =>
                {
                    b.HasOne("Tourism_Api.model.Place", null)
                        .WithMany()
                        .HasForeignKey("PlaceName")
                        .IsRequired()
                        .HasConstraintName("FK__Type_of_T__Place__33D4B598");

                    b.HasOne("Tourism_Api.model.TypeOfTourism", null)
                        .WithMany()
                        .HasForeignKey("TourismName")
                        .IsRequired()
                        .HasConstraintName("FK__Type_of_T__Touri__32E0915F");
                });

            modelBuilder.Entity("Tourism_Api.model.Governorate", b =>
                {
                    b.Navigation("Places");
                });

            modelBuilder.Entity("Tourism_Api.model.Place", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("FavoritePlaces");

                    b.Navigation("PlaceRates");

                    b.Navigation("TourguidAndPlaces");
                });

            modelBuilder.Entity("Tourism_Api.model.Program", b =>
                {
                    b.Navigation("ProgramsPhotos");
                });

            modelBuilder.Entity("Tourism_Api.model.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("FavoritePlaces");

                    b.Navigation("InverseTourguid");

                    b.Navigation("PlaceRates");

                    b.Navigation("TourguidAndPlaces");
                });
#pragma warning restore 612, 618
        }
    }
}
