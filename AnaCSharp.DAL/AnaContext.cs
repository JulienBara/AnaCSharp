using AnaCSharp.DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace AnaCSharp.DAL
{
    public class AnaContext : DbContext
    {
        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .UseSqlite("Data Source=ana.db")
                //.EnableSensitiveDataLogging()
                ;
        }

        public DbSet<DeterminingState> DeterminingStates { get; set; }
        public DbSet<DeterminedWord> DeterminedWords { get; set; }
        public DbSet<LogWord> LogWords { get; set; }
        public DbSet<MaxMarkovDegree> MaxMarkovDegrees { get; set; }
        public DbSet<Word> Words { get; set; }
    }
}
