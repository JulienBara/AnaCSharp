using AnaCSharp.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace AnaCSharp.DAL
{
    public class AnaContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
    }
}
