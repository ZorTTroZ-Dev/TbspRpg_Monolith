﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TbspRpgDataLayer;

namespace TbspRpgDataLayer.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210815201624_RenameContentColumnSourceIdToSourceKey")]
    partial class RenameContentColumnSourceIdToSourceKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasPostgresExtension("uuid-ossp")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("TbspRpgApi.Entities.Adventure", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("SourceKey")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("adventures");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.Content", b =>
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

                    b.ToTable("contents");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("AdventureId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AdventureId");

                    b.HasIndex("LocationId");

                    b.HasIndex("UserId");

                    b.ToTable("games");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.LanguageSources.En", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("Key")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("sources_en");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.LanguageSources.Esp", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("Key")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("sources_esp");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("AdventureId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Initial")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("SourceKey")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AdventureId");

                    b.ToTable("locations");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.Route", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("DestinationLocationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FailureSourceKey")
                        .HasColumnType("uuid");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("SourceKey")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SuccessSourceKey")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DestinationLocationId");

                    b.HasIndex("LocationId");

                    b.ToTable("routes");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("user");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.Content", b =>
                {
                    b.HasOne("TbspRpgApi.Entities.Game", "Game")
                        .WithMany("Contents")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.Game", b =>
                {
                    b.HasOne("TbspRpgApi.Entities.Adventure", "Adventure")
                        .WithMany("Games")
                        .HasForeignKey("AdventureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TbspRpgApi.Entities.Location", "Location")
                        .WithMany("Games")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TbspRpgApi.Entities.User", "User")
                        .WithMany("Games")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Adventure");

                    b.Navigation("Location");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.Location", b =>
                {
                    b.HasOne("TbspRpgApi.Entities.Adventure", "Adventure")
                        .WithMany("Locations")
                        .HasForeignKey("AdventureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Adventure");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.Route", b =>
                {
                    b.HasOne("TbspRpgApi.Entities.Location", "DestinationLocation")
                        .WithMany()
                        .HasForeignKey("DestinationLocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TbspRpgApi.Entities.Location", "Location")
                        .WithMany("Routes")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DestinationLocation");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.Adventure", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("Locations");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.Game", b =>
                {
                    b.Navigation("Contents");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.Location", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("Routes");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.User", b =>
                {
                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
