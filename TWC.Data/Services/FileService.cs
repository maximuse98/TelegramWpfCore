using System;
using System.Collections.Generic;
using System.Text;
using TWC.Data.Dtos;
using TWC.Data.Models;
using TWC.Data.Repositories;

namespace TWC.Data.Services
{
    public class FileService
    {
        private readonly FileRepository fileRepository;

        public FileService(FileRepository repository)
        {
            this.fileRepository = repository;
        }

        public void AddKey(KeyDto key)
        {
            ValidateKeyDto(key);
            fileRepository.AddKey(key);
        }

        public void DeactivateKey(string value) => fileRepository.DeactivateKey(value);

        public void AddOrUpdateFile(FileDto file)
        {
            ValidateFileDto(file);
            fileRepository.AddOrUpdateFile(file);
        }

        public void AddOrUpdateFiles(List<FileDto> files)
        {
            foreach(FileDto file in files)
            {
                AddOrUpdateFile(file);
            }
        }

        public File GetFileByKey(string keyValue)
        {
            ValidateKey(keyValue);
            return fileRepository.GetFileByKey(keyValue);
        }

        public void ReduceKeyCount(string keyValue)
        {
            fileRepository.GetKey(keyValue).Count--;
            SaveChanges();
        }

        public void SaveChanges()
        {
            fileRepository.SaveChanges();
        }

        public File[] GetFiles() => fileRepository.GetFiles();

        public Key[] GetKeys() => fileRepository.GetKeys();

        private void ValidateKey(string keyValue)
        {
            Key key = fileRepository.GetKey(keyValue);
            if (key == null)
            {
                throw new ArgumentException("Provided key value is invalid");
            }
            if (key.ExpiryDate < DateTime.Now)
            {
                throw new ArgumentException("Key is expired");
            }
            if (key.Count == 0)
            {
                throw new ArgumentException("Key is already activated");
            }
        }

        private void ValidateKeyDto(KeyDto key)
        {
            if(key.SourceId == null)
            {
                throw new ArgumentNullException("SourceId");
            }
            if(key.KeyValue == null)
            {
                throw new ArgumentNullException("KeyValue");
            }
            if(fileRepository.GetFile(key.SourceId) == null)
            {
                throw new ArgumentException("Provided SourceId is invalid");
            }

            key.ExpiryDate = key.ExpiryDate.ToString() == null ? DateTime.Now.AddDays(10) : key.ExpiryDate;
            key.Count = key.Count > 0 ? key.Count : 5;
        }
    
        private void ValidateFileDto(FileDto file)
        {
            if (file.Name == null)
            {
                throw new ArgumentNullException("Name");
            }
            if (file.SourceId == null)
            {
                throw new ArgumentNullException("SourceId");
            }
        }
    }
}
