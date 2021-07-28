using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;

namespace TbspRpgDataLayer
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options){}

        #region User

        public DbSet<User> Users { get; set; }

        #endregion

        #region Adventure

        public DbSet<Adventure> Adventures { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<En> SourcesEn { get; set; }
        public DbSet<Esp> SourcesEsp { get; set; }

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

            #endregion

            #region Adventure

            modelBuilder.Entity<Adventure>().ToTable("adventures");
            modelBuilder.Entity<Location>().ToTable("locations");
            modelBuilder.Entity<Route>().ToTable("routes");
            modelBuilder.Entity<En>().ToTable("sources_en");
            modelBuilder.Entity<Esp>().ToTable("sources_esp");

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

            modelBuilder.Entity<Adventure>()
                .HasMany(a => a.Locations)
                .WithOne(l => l.Adventure)
                .HasForeignKey(l => l.AdventureId);

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
            
            modelBuilder.Entity<Adventure>()
                .HasMany(a => a.Games)
                .WithOne(g => g.Adventure)
                .HasForeignKey(g => g.AdventureId);
            
            modelBuilder.Entity<Location>()
                .HasMany(l => l.Games)
                .WithOne(g => g.Location)
                .HasForeignKey(g => g.LocationId);

            #endregion
        }
    }
}