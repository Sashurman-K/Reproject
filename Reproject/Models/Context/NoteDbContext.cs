using Microsoft.EntityFrameworkCore;
using Reproject.Configures;

namespace Reproject.Models.Context
{
    public class NoteDbContext : DbContext
    {
        private readonly string _connectionString;

        public NoteDbContext(ServiceConfigure configure)
        {
            _connectionString = configure.ConnectionStrings.GetValueOrDefault("DefaultConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        public DbSet<Note> NoteDbSet { get; set; }
        public DbSet<User> UserDbSet { get; set; }

    }
}
