using Microsoft.EntityFrameworkCore;
using SFG.Models;

namespace SFG.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<UsersModel> Users { get; set; }
        public DbSet<LastPurchaseInfoModel> LastPurchaseInfo { get; set; }
        public DbSet<QoutationModel> Qoutations { get; set; }
        public DbSet<MRPBOMModel> MRPBOM { get; set; }
    }
}
