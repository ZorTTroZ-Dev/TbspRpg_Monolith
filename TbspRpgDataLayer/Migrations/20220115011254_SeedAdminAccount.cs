using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using TbspRpgDataLayer.Entities;

#nullable disable

namespace TbspRpgDataLayer.Migrations
{
    public partial class SeedAdminAccount : Migration
    {
        private string HashPassword(string password)
        {
            var salt = Environment.GetEnvironmentVariable("DATABASE_SALT").ToString();
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                Convert.FromBase64String(salt),
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));
        }
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var contextFactory = new DatabaseContextFactory();
            using (var context = contextFactory.CreateDbContext(new string[] {}))
            {
                // create an admin group
                var adminGroup = new Group()
                {
                    Id = Guid.NewGuid(),
                    Name = "admin"
                };
                // create an adventure-edit permission
                var adventureEditPermission = new Permission()
                {
                    Id = Guid.NewGuid(),
                    Name = "adventure-edit"
                };
                // add the adventure-edit permission to the admin group
                adminGroup.Permissions = new List<Permission>();
                adminGroup.Permissions.Add(adventureEditPermission);
                adventureEditPermission.Groups = new List<Group>();
                adventureEditPermission.Groups.Add(adminGroup);
                context.Groups.Add(adminGroup);
                context.Permissions.Add(adventureEditPermission);
                
                // create admin user admin@zorttroz.com
                var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD").ToString();
                var adminUser = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = "admin@zorttroz.com",
                    Password = HashPassword(adminPassword),
                    DateCreated = DateTime.UtcNow,
                    RegistrationComplete = true
                };
                
                // create a test user test@zorttroz.com
                var testPassword = Environment.GetEnvironmentVariable("TEST_PASSWORD").ToString();
                var testUser = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = "test@zorttroz.com",
                    Password = HashPassword(testPassword),
                    DateCreated = DateTime.UtcNow,
                    RegistrationComplete = true
                };
                context.Users.Add(adminUser);
                context.Users.Add(testUser);
                
                // add the admin user to the admin group
                adminUser.Groups = new List<Group>();
                adminUser.Groups.Add(adminGroup);
                adminGroup.Users = new List<User>();
                adminGroup.Users.Add(adminUser);
                
                context.SaveChanges();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var contextFactory = new DatabaseContextFactory();
            using (var context = contextFactory.CreateDbContext(new string[] { }))
            {
                // remove the admin use from the admin group
                var dbAdminUser = context.Users.Where(user => user.Email == "admin@zorttroz.com").Include("Groups").First();
                dbAdminUser.Groups.Clear();
                context.SaveChanges();
                
                // remove the admin user
                context.Users.Remove(dbAdminUser);
                context.SaveChanges();
                
                // remove the test user
                var dbTestUser = context.Users.First(user => user.Email == "test@zorttroz.com");
                context.Users.Remove(dbTestUser);
                context.SaveChanges();
                
                // remove the adventure-edit permission from the admin group
                var dbAdminGroup = context.Groups.Where(group => group.Name == "admin").Include("Permissions").First();
                var dbAdventureEditPermission = context.Permissions.Where(perm => perm.Name == "adventure-edit").Include("Groups").First();
                dbAdminGroup.Permissions.Clear();
                dbAdventureEditPermission.Groups.Clear();
                context.SaveChanges();

                // remove the adventure-edit permission
                context.Permissions.Remove(dbAdventureEditPermission);
                context.SaveChanges();

                // remove the admin group
                context.Groups.Remove(dbAdminGroup);
                context.SaveChanges();
            }
        }
    }
}
