using System;
using System.Threading.Tasks;
using LuizaLabs.WebApi.Models;

namespace LuizaLabs.WebApi.Repositories.Interfaces
{
    public interface IRecuperaSenhaRepository : IGenericoRepository
    {
        Task<RecuperacaoSenhaUsuario> GetRecuperaSenhaId(Guid id);
    }
}