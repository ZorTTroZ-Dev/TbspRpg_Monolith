using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgSettings.Settings;

namespace TbspRpgDataLayer.Repositories
{
    public interface ISourcesRepository
    {
        Task<string> GetSourceTextForKey(Guid key, string language = null);
        Task<Source> GetSourceForKey(Guid key, Guid adventureId, string language);
        Task AddSource(Source source, string language);
    }
    
    public class SourcesRepository : ISourcesRepository
    {
        private readonly DatabaseContext _databaseContext;

        public SourcesRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        private IQueryable<Source> GetQueryRoot(string language)
        {
            IQueryable<Source> query = null;
            if (language == null || language == Languages.ENGLISH)
            {
                query = _databaseContext.SourcesEn.AsQueryable();
            }
            if (language == Languages.SPANISH)
            {
                query = _databaseContext.SourcesEsp.AsQueryable();
            }
            return query;
        }

        public Task<string> GetSourceTextForKey(Guid key, string language = null)
        {
            var query = GetQueryRoot(language);
            if (query != null)
                return query.Where(s => s.Key == key)
                    .Select(s => s.Text)
                    .FirstOrDefaultAsync();
            
            throw new ArgumentException($"invalid language {language}");
        }

        public Task<Source> GetSourceForKey(Guid key, Guid adventureId, string language)
        {
            var query = GetQueryRoot(language);
            if(query != null && key != Guid.Empty)
                return query.FirstOrDefaultAsync(source => 
                    source.Key == key && source.AdventureId == adventureId);
            if(query != null && key == Guid.Empty) 
                return query.FirstOrDefaultAsync(source => 
                    source.Key == key);
            
            throw new ArgumentException($"invalid language {language}");
        }

        private En SourceToEn(Source source)
        {
            return new En()
            {
                Id = source.Id,
                Key = source.Key,
                AdventureId = source.AdventureId,
                Name = source.Name,
                Text = source.Text
            };
        }

        private Esp SourcetoEsp(Source source)
        {
            return new Esp()
            {
                Id = source.Id,
                Key = source.Key,
                AdventureId = source.AdventureId,
                Name = source.Name,
                Text = source.Text
            };
        }

        public async Task AddSource(Source source, string language)
        {
            if (language == Languages.ENGLISH || language == null)
            {
                await _databaseContext.SourcesEn.AddAsync(SourceToEn(source));
                return;
            }

            if (language == Languages.SPANISH)
            {
                await _databaseContext.SourcesEsp.AddAsync(SourcetoEsp(source));
                return;
            }

            throw new ArgumentException($"invalid language {language}");
        }
    }
}