using System;
using System.Linq;
using System.Threading.Tasks;
using LuizaLabs.WebApi.Data;
using LuizaLabs.WebApi.Models;
using LuizaLabs.WebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuizaLabs.WebApi.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
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
        public UsuarioRepository(LuizaLabsDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> GetUsuarioAsync(Usuario usuario)
        {
            IQueryable<Usuario> query = _context.Usuarios
                                             .AsNoTracking()
                                             .Where(u => u.Email == usuario.Email && u.Senha == usuario.Senha);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Usuario> GetUsuarioId(Guid id)
        {
            return await _context.Usuarios.AsNoTracking().Where(user => user.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Usuario> GetUsuarioEmail(string email)
        {
            return await _context.Usuarios.AsNoTracking().Where(user => user.Email == email).FirstOrDefaultAsync();
        }
    }
}