using System;
using System.Collections.Generic;
using System.Text;
using TWC.Data.Dtos;
using TWC.Data.Models;
using TWC.Data.Repositories;
using TWC.Data.Validators;

namespace TWC.Data.Services
{
    public class FileService
    {
        private readonly FileRepository fileRepository;
        private IValidator validator;

        public FileService(FileRepository repository, IValidator validator)
        {
            this.fileRepository = repository;
            this.validator = validator;
        }

        public void AddKey(KeyDto key)
        {
            this.validator.Validate(key);
            fileRepository.AddKey(key);
        }

        public void DeactivateKey(string value) => fileRepository.DeactivateKey(value);

        public void AddOrUpdateFile(FileDto file)
        {
            this.validator.Validate(file);
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
            this.validator.Validate(fileRepository.GetKey(keyValue));
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
    }
}
