using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;
using TbspRpgApi.Entities.LanguageSources;

#nullable disable

namespace TbspRpgDataLayer.Migrations
{
    /// <inheritdoc />
    public partial class SeedEmptySourcePart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var contextFactory = new DatabaseContextFactory();
            using (var context = contextFactory.CreateDbContext(new string[] { }))
            {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
