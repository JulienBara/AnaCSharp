using AnaCSharp.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace AnaCSharp.DAL
{
    public class AnaContext : DbContext
    {
        public AnaContext(string connectionString) : base(GetOptions(connectionString))
        {
        }

        public DbSet<DeterminingState> DeterminingStates { get; set; }
        public DbSet<DeterminedWord> DeterminedWords { get; set; }
        public DbSet<Word> Words { get; set; }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }
    }
}
