using Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{

    public class LocalDbContext : DbContext
    {
        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
     
        }

        // Single Entities

        public DbSet<User> Forms { get; set; }


    }






}