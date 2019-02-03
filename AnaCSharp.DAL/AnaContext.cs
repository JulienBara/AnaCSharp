using AnaCSharp.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace AnaCSharp.DAL
{
    public class AnaContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ana.db");
        }

        public DbSet<DeterminingState> DeterminingStates { get; set; }
        public DbSet<DeterminedWord> DeterminedWords { get; set; }
        public DbSet<LogWord> LogWords { get; set; }
        public DbSet<MaxMarkovDegree> MaxMarkovDegrees { get; set; }
        public DbSet<Word> Words { get; set; }
    }
}
