using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TWC.Data.Models;

namespace TWC.Data
{
    public class FileContext : DbContext
    {
        public FileContext(DbContextOptions<FileContext> options) : base(options)
        {
            //Database.CanConnect();
            //Database.EnsureDeleted();
            //Database.EnsureCreated();

            //Apply migration
            //Database.Migrate();
        }

        public DbSet<File> Files { get; set; }
        public DbSet<Key> Keys { get; set; }
    }
}

