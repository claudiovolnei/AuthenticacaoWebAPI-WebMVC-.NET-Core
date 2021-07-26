using LuizaLabs.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LuizaLabs.WebApi.Data
{
    public class LuizaLabsDbContext : DbContext
    {
        public LuizaLabsDbContext(DbContextOptions<LuizaLabsDbContext> options)
           : base(options)
        { }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}