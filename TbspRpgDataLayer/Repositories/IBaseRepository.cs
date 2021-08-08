using System.Threading.Tasks;

namespace TbspRpgDataLayer.Repositories
{
    public interface IBaseRepository
    {
        Task SaveChanges();
    }
}