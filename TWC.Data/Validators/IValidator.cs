using System;
using System.Collections.Generic;
using System.Text;
using TWC.Data.Dtos;
using TWC.Data.Models;

namespace TWC.Data.Validators
{
    public interface IValidator
    {
        IValidator Validate(KeyDto key);

        IValidator Validate(FileDto fileDto);

        IValidator Validate(Key key);
    }
}
