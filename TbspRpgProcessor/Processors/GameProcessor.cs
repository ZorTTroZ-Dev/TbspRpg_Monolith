using System;
using System.Threading.Tasks;
using TbspRpgApi.Entities;

namespace TbspRpgProcessor.Processors
{
    public interface IGameProcessor
    {
        Task<Game> StartGame(Guid userId, Guid adventureId);
    }
    
    public class GameProcessor : IGameProcessor
    {
        public Task<Game> StartGame(Guid userId, Guid adventureId)
        {
            throw new NotImplementedException();
        }
    }
}