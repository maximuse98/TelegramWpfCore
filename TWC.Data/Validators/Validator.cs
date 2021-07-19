using System;
using System.Collections.Generic;
using System.Text;
using TWC.Data.Dtos;
using TWC.Data.Models;

namespace TWC.Data.Validators
{
    public class Validator : IValidator
    {
        public IValidator Validate(KeyDto key)
        {
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
            return this;
        }

        public IValidator Validate(FileDto file)
        {
            if (file.Name == null)
            {
                throw new ArgumentNullException("Name");
            }
            if (file.SourceId == null)
            {
                throw new ArgumentNullException("SourceId");
            }
            return this;
        }

        public IValidator Validate(Key key)
        {
            if (key.SourceId == null)
            {
                throw new ArgumentNullException("SourceId");
            }
            if (key.KeyValue == null)
            {
                throw new ArgumentNullException("KeyValue");
            }

            return this;
        }
    }
}
