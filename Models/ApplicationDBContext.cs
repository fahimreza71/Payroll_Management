using Microsoft.EntityFrameworkCore;

namespace PayrollWebApp.Models
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DesignationEntity> Designations { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<PayrollEntity> Payrolls { get; set; }
    }
}
