using Microsoft.EntityFrameworkCore;
using MvcNetSession.Models;

namespace MvcNetSession.Data
{
    public class HospitalContext: DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options)
        {}
        public DbSet<Empleado> Empleados { get; set; }

    }
        
}
