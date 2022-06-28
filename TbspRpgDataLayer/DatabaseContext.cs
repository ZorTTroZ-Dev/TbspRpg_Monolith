using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Entities;

namespace TbspRpgDataLayer
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options){}

        #region User

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        #endregion

        #region Adventure

        public DbSet<Adventure> Adventures { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<En> SourcesEn { get; set; }
        public DbSet<Esp> SourcesEsp { get; set; }
        public DbSet<Script> Scripts { get; set; }

        #endregion

        #region Game

        public DbSet<Game> Games { get; set; }
        public DbSet<Content> Contents { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            #region User

            modelBuilder.Entity<User>().ToTable("user");

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()");
            
            modelBuilder.Entity<Group>().ToTable("groups");

            modelBuilder.Entity<Group>().HasKey(g => g.Id);
            modelBuilder.Entity<Group>().Property(g => g.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()");
            
            modelBuilder.Entity<Permission>().ToTable("permissions");

            modelBuilder.Entity<Permission>().HasKey(p => p.Id);
            modelBuilder.Entity<Permission>().Property(p => p.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()");

            #endregion

            #region Adventure

            modelBuilder.Entity<Adventure>().ToTable("adventures");
            modelBuilder.Entity<Location>().ToTable("locations");
            modelBuilder.Entity<Route>().ToTable("routes");
            modelBuilder.Entity<En>().ToTable("sources_en");
            modelBuilder.Entity<Esp>().ToTable("sources_esp");
            modelBuilder.Entity<Script>().ToTable("scripts");

            modelBuilder.Entity<Adventure>().HasKey(a => a.Id);
            modelBuilder.Entity<Adventure>().Property(a => a.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            modelBuilder.Entity<Location>().HasKey(a => a.Id);
            modelBuilder.Entity<Location>().Property(a => a.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();
            
            modelBuilder.Entity<Route>().HasKey(a => a.Id);
            modelBuilder.Entity<Route>().Property(a => a.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();
            
            modelBuilder.Entity<Script>().HasKey(s => s.Id);
            modelBuilder.Entity<Script>().Property(s => s.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            modelBuilder.Entity<Adventure>()
                .HasMany(a => a.Locations)
                .WithOne(l => l.Adventure)
                .HasForeignKey(l => l.AdventureId);

            modelBuilder.Entity<Adventure>()
                .HasMany(a => a.Scripts)
                .WithOne(s => s.Adventure)
                .HasForeignKey(s => s.AdventureId);

            modelBuilder.Entity<Script>()
                .HasMany(s => s.AdventureInitializations)
                .WithOne(a => a.InitializationScript)
                .HasForeignKey(a => a.InitializationScriptId);

            modelBuilder.Entity<Script>()
                .HasMany(s => s.AdventureTerminations)
                .WithOne(a => a.TerminationScript)
                .HasForeignKey(a => a.TerminationScriptId);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.Routes)
                .WithOne(r => r.Location)
                .HasForeignKey(r => r.LocationId);

            modelBuilder.Entity<Route>()
                .HasOne(r => r.DestinationLocation)
                .WithMany()
                .HasForeignKey(r => r.DestinationLocationId);

                //language sources
            modelBuilder.Entity<En>().HasKey(e => e.Id);
            modelBuilder.Entity<En>().Property(e => e.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();
            
            modelBuilder.Entity<Esp>().HasKey(e => e.Id);
            modelBuilder.Entity<Esp>().Property(e => e.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            modelBuilder.Entity<En>().Ignore(en => en.Language);
            modelBuilder.Entity<Esp>().Ignore(esp => esp.Language);

            #endregion

            #region Game

            modelBuilder.Entity<Game>().ToTable("games");
            modelBuilder.Entity<Content>().ToTable("contents");

            modelBuilder.Entity<Game>().HasKey(a => a.Id);
            modelBuilder.Entity<Game>().Property(a => a.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();
            
            modelBuilder.Entity<Content>().HasKey(a => a.Id);
            modelBuilder.Entity<Content>().Property(a => a.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();
            
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Contents)
                .WithOne(c => c.Game)
                .HasForeignKey(c => c.GameId);

            #endregion

            #region Relations
            
            modelBuilder.Entity<User>()
                .HasMany(u => u.Games)
                .WithOne(g => g.User)
                .HasForeignKey(g => g.UserId);

            modelBuilder.Entity<Group>()
                .HasMany(g => g.Users)
                .WithMany(u => u.Groups);

            modelBuilder.Entity<Permission>()
                .HasMany(p => p.Groups)
                .WithMany(g => g.Permissions);

            modelBuilder.Entity<Script>()
                .HasMany(s => s.Includes)
                .WithMany(s => s.IncludedIn);
            
            modelBuilder.Entity<Adventure>()
                .HasMany(a => a.Games)
                .WithOne(g => g.Adventure)
                .HasForeignKey(g => g.AdventureId);
            
            modelBuilder.Entity<Location>()
                .HasMany(l => l.Games)
                .WithOne(g => g.Location)
                .HasForeignKey(g => g.LocationId);
            
            modelBuilder.Entity<User>()
                .HasMany(u => u.Adventures)
                .WithOne(a => a.CreatedByUser)
                .HasForeignKey(a => a.CreatedByUserId);

            #endregion
        }
    }
}