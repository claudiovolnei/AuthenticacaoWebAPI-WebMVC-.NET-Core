using System.Threading.Tasks;

namespace LuizaLabs.WebApi.Repositories.Interfaces
{
    public interface IGenericoRepository
    {
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
    }
}