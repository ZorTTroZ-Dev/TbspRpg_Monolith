﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TbspRpgDataLayer;

#nullable disable

namespace TbspRpgDataLayer.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20220422233757_AlterRouteFieldNames")]
    partial class AlterRouteFieldNames
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GroupPermission", b =>
                {
                    b.Property<Guid>("GroupsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PermissionsId")
                        .HasColumnType("uuid");

                    b.HasKey("GroupsId", "PermissionsId");

                    b.HasIndex("PermissionsId");

                    b.ToTable("GroupPermission");
                });

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.Property<Guid>("GroupsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uuid");

                    b.HasKey("GroupsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("GroupUser");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.LanguageSources.En", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("AdventureId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Key")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("sources_en", (string)null);
                });

            modelBuilder.Entity("TbspRpgApi.Entities.LanguageSources.Esp", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("AdventureId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Key")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("sources_esp", (string)null);
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Adventure", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DescriptionSourceKey")
                        .HasColumnType("uuid");

                    b.Property<Guid>("InitialSourceKey")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.ToTable("adventures", (string)null);
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Content", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Position")
                        .HasColumnType("numeric(20,0)");

                    b.Property<Guid>("SourceKey")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("contents", (string)null);
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("AdventureId")
                        .HasColumnType("uuid");

                    b.Property<string>("Language")
                        .HasColumnType("text");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uuid");

                    b.Property<long>("LocationUpdateTimeStamp")
                        .HasColumnType("bigint");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AdventureId");

                    b.HasIndex("LocationId");

                    b.HasIndex("UserId");

                    b.ToTable("games", (string)null);
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("groups", (string)null);
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("AdventureId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Final")
                        .HasColumnType("boolean");

                    b.Property<bool>("Initial")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("SourceKey")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AdventureId");

                    b.ToTable("locations", (string)null);
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("permissions", (string)null);
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Route", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("DestinationLocationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("RouteTakenSourceKey")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SourceKey")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DestinationLocationId");

                    b.HasIndex("LocationId");

                    b.ToTable("routes", (string)null);
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<bool>("RegistrationComplete")
                        .HasColumnType("boolean");

                    b.Property<string>("RegistrationKey")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("GroupPermission", b =>
                {
                    b.HasOne("TbspRpgDataLayer.Entities.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TbspRpgDataLayer.Entities.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.HasOne("TbspRpgDataLayer.Entities.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TbspRpgDataLayer.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Adventure", b =>
                {
                    b.HasOne("TbspRpgDataLayer.Entities.User", "CreatedByUser")
                        .WithMany("Adventures")
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Content", b =>
                {
                    b.HasOne("TbspRpgDataLayer.Entities.Game", "Game")
                        .WithMany("Contents")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Game", b =>
                {
                    b.HasOne("TbspRpgDataLayer.Entities.Adventure", "Adventure")
                        .WithMany("Games")
                        .HasForeignKey("AdventureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TbspRpgDataLayer.Entities.Location", "Location")
                        .WithMany("Games")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TbspRpgDataLayer.Entities.User", "User")
                        .WithMany("Games")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Adventure");

                    b.Navigation("Location");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Location", b =>
                {
                    b.HasOne("TbspRpgDataLayer.Entities.Adventure", "Adventure")
                        .WithMany("Locations")
                        .HasForeignKey("AdventureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Adventure");
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Route", b =>
                {
                    b.HasOne("TbspRpgDataLayer.Entities.Location", "DestinationLocation")
                        .WithMany()
                        .HasForeignKey("DestinationLocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TbspRpgDataLayer.Entities.Location", "Location")
                        .WithMany("Routes")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DestinationLocation");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Adventure", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("Locations");
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Game", b =>
                {
                    b.Navigation("Contents");
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Location", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("Routes");
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.User", b =>
                {
                    b.Navigation("Adventures");

                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
