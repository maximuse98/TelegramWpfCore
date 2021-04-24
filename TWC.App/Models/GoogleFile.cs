using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace TWC.App.Models
{
    public class GoogleFile : File
    {
        private Google.Apis.Drive.v3.Data.File _file;

        public GoogleFile(Google.Apis.Drive.v3.Data.File file)
        {
            _file = file;

            SourceId = file.Id;
            Name = file.Name;
        }

        public GoogleFile()
        {
        }
    }
}
