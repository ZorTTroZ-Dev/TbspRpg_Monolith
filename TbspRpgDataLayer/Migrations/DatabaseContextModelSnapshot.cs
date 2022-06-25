﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TbspRpgDataLayer;

#nullable disable

namespace TbspRpgDataLayer.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
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

            modelBuilder.Entity("ScriptScript", b =>
                {
                    b.Property<Guid>("IncludedInId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("IncludesId")
                        .HasColumnType("uuid");

                    b.HasKey("IncludedInId", "IncludesId");

                    b.HasIndex("IncludesId");

                    b.ToTable("ScriptScript");
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

                    b.Property<Guid?>("ScriptId")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ScriptId");

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

                    b.Property<Guid?>("ScriptId")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ScriptId");

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

                    b.Property<Guid?>("InitializationScriptId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("TerminationScriptId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("InitializationScriptId");

                    b.HasIndex("TerminationScriptId");

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

                    b.Property<string>("GameState")
                        .HasColumnType("jsonb");

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

                    b.Property<Guid?>("EnterScriptId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ExitScriptId")
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

                    b.HasIndex("EnterScriptId");

                    b.HasIndex("ExitScriptId");

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

                    b.Property<Guid?>("RouteTakenScriptId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RouteTakenSourceKey")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SourceKey")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DestinationLocationId");

                    b.HasIndex("LocationId");

                    b.HasIndex("RouteTakenScriptId");

                    b.ToTable("routes", (string)null);
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Script", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("AdventureId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AdventureId");

                    b.ToTable("scripts", (string)null);
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

            modelBuilder.Entity("ScriptScript", b =>
                {
                    b.HasOne("TbspRpgDataLayer.Entities.Script", null)
                        .WithMany()
                        .HasForeignKey("IncludedInId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TbspRpgDataLayer.Entities.Script", null)
                        .WithMany()
                        .HasForeignKey("IncludesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TbspRpgApi.Entities.LanguageSources.En", b =>
                {
                    b.HasOne("TbspRpgDataLayer.Entities.Script", "Script")
                        .WithMany()
                        .HasForeignKey("ScriptId");

                    b.Navigation("Script");
                });

            modelBuilder.Entity("TbspRpgApi.Entities.LanguageSources.Esp", b =>
                {
                    b.HasOne("TbspRpgDataLayer.Entities.Script", "Script")
                        .WithMany()
                        .HasForeignKey("ScriptId");

                    b.Navigation("Script");
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Adventure", b =>
                {
                    b.HasOne("TbspRpgDataLayer.Entities.User", "CreatedByUser")
                        .WithMany("Adventures")
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TbspRpgDataLayer.Entities.Script", "InitializationScript")
                        .WithMany("AdventureInitializations")
                        .HasForeignKey("InitializationScriptId");

                    b.HasOne("TbspRpgDataLayer.Entities.Script", "TerminationScript")
                        .WithMany("AdventureTerminations")
                        .HasForeignKey("TerminationScriptId");

                    b.Navigation("CreatedByUser");

                    b.Navigation("InitializationScript");

                    b.Navigation("TerminationScript");
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

                    b.HasOne("TbspRpgDataLayer.Entities.Script", "EnterScript")
                        .WithMany()
                        .HasForeignKey("EnterScriptId");

                    b.HasOne("TbspRpgDataLayer.Entities.Script", "ExitScript")
                        .WithMany()
                        .HasForeignKey("ExitScriptId");

                    b.Navigation("Adventure");

                    b.Navigation("EnterScript");

                    b.Navigation("ExitScript");
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

                    b.HasOne("TbspRpgDataLayer.Entities.Script", "RouteTakenScript")
                        .WithMany()
                        .HasForeignKey("RouteTakenScriptId");

                    b.Navigation("DestinationLocation");

                    b.Navigation("Location");

                    b.Navigation("RouteTakenScript");
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Script", b =>
                {
                    b.HasOne("TbspRpgDataLayer.Entities.Adventure", "Adventure")
                        .WithMany("Scripts")
                        .HasForeignKey("AdventureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Adventure");
                });

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Adventure", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("Locations");

                    b.Navigation("Scripts");
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

            modelBuilder.Entity("TbspRpgDataLayer.Entities.Script", b =>
                {
                    b.Navigation("AdventureInitializations");

                    b.Navigation("AdventureTerminations");
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
