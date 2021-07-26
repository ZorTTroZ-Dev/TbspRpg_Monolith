using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;
using TbspRpgApi.Settings;

namespace TbspRpgApi.Repositories
{
    public interface ISourcesRepository
    {
        Task<string> GetSourceForKey(Guid key, string language = null);
    }
    
    public class SourcesRepository : ISourcesRepository
    {
        private readonly DatabaseContext _databaseContext;

        public SourcesRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<string> GetSourceForKey(Guid key, string language = null)
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

            if (query != null)
                return query.Where(s => s.Key == key)
                    .Select(s => s.Text)
                    .FirstOrDefaultAsync();
            
            throw new ArgumentException($"invalid language {language}");
        }
    }
}