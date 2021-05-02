using System;
using System.Collections.Generic;
using System.Text;

namespace TWC.Data.Dtos
{
    public class KeyDto
    {
        public string SourceId { get; set; }
        public string KeyValue { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Count { get; set; }
    }
}
