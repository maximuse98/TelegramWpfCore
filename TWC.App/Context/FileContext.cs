using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TWC.App.Models;

namespace TWC.App.Context
{
    public class FileContext : DbContext
    {
        public FileContext(DbContextOptions<FileContext> options): base(options)
        {
            //Database.CanConnect();
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public void UpdateFiles<T>(List<T> files)where T : File
        {
            Files.RemoveRange(Files);
            Files.AddRange(files);

            //Keys.Add(new Key { Count = 5, ExpiryDate = DateTime.Now, SourceId = files[0].SourceId, KeyValue = "qwerty1231" });
            //Keys.Add(new Key { Count = 5, ExpiryDate = DateTime.Now, SourceId = files[0].SourceId, KeyValue = "qwerty1232" });
            //Keys.Add(new Key { Count = 5, ExpiryDate = DateTime.Now, SourceId = files[1].SourceId, KeyValue = "qwerty1233" });

            //SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GoogleFile>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<File> Files { get; set; }
        public DbSet<Key> Keys { get; set; }
    }
}

