using AnaCSharp.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace AnaCSharp.DAL
{
    public class AnaContext : DbContext
    {
        public DbSet<DeterminingState> DeterminingStates { get; set; }
        public DbSet<DeterminedWord> DeterminedWords { get; set; }
        public DbSet<LogWord> LogWords { get; set; }
        public DbSet<Word> Words { get; set; }
    }
}
