using System;
using System.Linq;
using System.Threading.Tasks;
using LuizaLabs.WebApi.Data;
using LuizaLabs.WebApi.Models;
using LuizaLabs.WebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuizaLabs.WebApi.Repositories
{
    public class RecuperaSenhaRepository : IRecuperaSenhaRepository
    {
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
        private readonly LuizaLabsDbContext _context;
        public RecuperaSenhaRepository(LuizaLabsDbContext context)
        {
            _context = context;
        }

        public async Task<RecuperacaoSenhaUsuario> GetRecuperaSenhaId(Guid id)
        {
            return await _context.RecuperacaoSenhaUsuario.AsNoTracking().Where(r => r.Id == id && r.Ativa).FirstOrDefaultAsync();
        }
    }
}