using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer;
using TbspRpgDataLayer.Entities;

namespace TbspRpgDatabaseSetup
{
    class Program
    {
        private static string HashPassword(string password)
        {
            var salt = Environment.GetEnvironmentVariable("DATABASE_SALT");
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                Convert.FromBase64String(salt ?? throw new InvalidOperationException()),
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));
        }

        private static void SeedUsers()
        {
            var contextFactory = new DatabaseContextFactory();
            using var context = contextFactory.CreateDbContext([]);
            
            var adminGroup = context.Groups.FirstOrDefault(group => group.Name == "admin");
            var adventureEditPermission = context.Permissions.FirstOrDefault(permission => permission.Name == "adventure-edit");
            if (adminGroup == null && adventureEditPermission == null)
            {
                // create an admin group
                adminGroup = new Group()
                {
                    Id = Guid.NewGuid(),
                    Name = "admin"
                };
                // create an adventure-edit permission
                adventureEditPermission = new Permission()
                {
                    Id = Guid.NewGuid(),
                    Name = "adventure-edit"
                };
                // add the adventure-edit permission to the admin group, if needed
                adminGroup.Permissions = new List<Permission>();
                adminGroup.Permissions.Add(adventureEditPermission);
                adventureEditPermission.Groups = new List<Group>();
                adventureEditPermission.Groups.Add(adminGroup);
                context.Groups.Add(adminGroup);
                context.Permissions.Add(adventureEditPermission);
            }

            // create admin user admin@zorttroz.com
            var adminUser = context.Users.FirstOrDefault(user => user.Email == "admin@zorttroz.com");
            if (adminUser == null)
            {
                var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
                adminUser = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin@zorttroz.com",
                    Password = HashPassword(adminPassword ?? throw new InvalidOperationException()),
                    DateCreated = DateTime.UtcNow,
                    RegistrationComplete = true
                };
                context.Users.Add(adminUser);
                // add the admin user to the admin group
                adminUser.Groups = new List<Group>();
                adminUser.Groups.Add(adminGroup);
                if (adminGroup != null)
                {
                    adminGroup.Users = new List<User>();
                    adminGroup.Users.Add(adminUser);
                }
            }

            var testUser = context.Users.FirstOrDefault(user => user.Email == "test@zorttroz.com");
            if (testUser == null)
            {
                // create a test user test@zorttroz.com
                var testPassword = Environment.GetEnvironmentVariable("TEST_PASSWORD");
                testUser = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = "test@zorttroz.com",
                    Password = HashPassword(testPassword ?? throw new InvalidOperationException()),
                    DateCreated = DateTime.UtcNow,
                    RegistrationComplete = true
                };
                context.Users.Add(testUser);
            }

            context.SaveChanges();
        }

        private static void SeedSource()
        {
            var contextFactory = new DatabaseContextFactory();
            using var context = contextFactory.CreateDbContext([]);
            // check if we have empty source for spanish and english
            var dbEnglishEmptySource = context.SourcesEn.FirstOrDefault(source => source.Key == Guid.Empty);
            // if we don't create them
            if (dbEnglishEmptySource == null)
            {
                var englishEmptySource = new En()
                {
                    Id = Guid.NewGuid(),
                    Key = Guid.Empty,
                    Text = "Empty Source",
                    AdventureId = Guid.Empty
                };
                context.SourcesEn.Add(englishEmptySource);
            }

            var dbSpanishEmptySource = context.SourcesEsp.FirstOrDefault(source => source.Key == Guid.Empty);
            if (dbSpanishEmptySource == null)
            {
                var spanishEmptySource = new Esp()
                {
                    Id = Guid.NewGuid(),
                    Key = Guid.Empty,
                    Text = "Fuente Vacia",
                    AdventureId = Guid.Empty
                };
                context.SourcesEsp.Add(spanishEmptySource);
            }

            context.SaveChanges();
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("Database Setup Start.");
            Console.WriteLine("Creating Admin and Test User");
            SeedUsers();
            Console.WriteLine("Creating Empty Source");
            SeedSource();
            Console.WriteLine("Database Setup Complete");
        }
    }
}