using api.parcial1.Models;
using Microsoft.EntityFrameworkCore;

namespace api.parcial1.Data
{
    public class DbPrimerParcial :DbContext
    {
        public DbPrimerParcial(DbContextOptions<DbPrimerParcial> options) : base(options)
        {

        }

        public DbSet<Clientes> Clientes => Set<Clientes>();

        public DbSet<Ciudades> Ciudades => Set<Ciudades>();


    }
}
