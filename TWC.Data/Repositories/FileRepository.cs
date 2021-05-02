using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TWC.Data.Dtos;
using TWC.Data.Models;

namespace TWC.Data.Repositories
{
    public class FileRepository
    {
        private readonly FileContext dbContext;

        public FileRepository(FileContext context)
        {
            this.dbContext = context;
        }

        public void AddKey(KeyDto key)
        {
            dbContext.Keys.Add(new Key
            {
                SourceId = key.SourceId,
                KeyValue = key.KeyValue,
                ExpiryDate = key.ExpiryDate,
                Count = key.Count,
                IsDeactivated = false
            });

            dbContext.SaveChanges();
        }

        public void DeactivateKey(string value)
        {
            Key key = dbContext.Keys.Where(x => x.KeyValue == value).FirstOrDefault();
            key.IsDeactivated = true;

            dbContext.SaveChanges();
        }
        
        public void AddOrUpdateFile(FileDto file)
        {
            File f = dbContext.Files.Where(x => x.SourceId == file.SourceId).FirstOrDefault();
            
            if(f == null)
            {
                dbContext.Files.Add(new File
                {
                    SourceId = file.SourceId,
                    Name = file.Name
                });
            }
            else
            {
                f.Name = file.Name;
            }
            dbContext.SaveChanges();
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        public File[] GetFiles()
        {
            return dbContext.Files.ToArray();
        }

        public File GetFile(string sourceId)
        {
            return dbContext.Files.Where(x => x.SourceId == sourceId).FirstOrDefault();
        }

        public Key[] GetKeys()
        {
            return dbContext.Keys.ToArray();
        }

        public Key GetKey(string value)
        {
            return dbContext.Keys.Where(x => x.KeyValue == value).FirstOrDefault();
        }
    }
}
