using System;
using System.Threading.Tasks;
using LuizaLabs.WebApi.Models;

namespace LuizaLabs.WebApi.Repositories.Interfaces
{
    public interface IUsuarioRepository : IGenericoRepository
    {
        Task<Usuario> GetUsuarioAsync(Usuario usuario);
        Task<Usuario> GetUsuarioId(Guid id);
        Task<Usuario> GetUsuarioEmail(string email);
    }
}