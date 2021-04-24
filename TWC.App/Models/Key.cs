﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TWC.App.Models
{
    public class Key
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string SourceId { get; set; }
        public string KeyValue { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Count { get; set; }
        public bool IsDeactivated { get; set; }
    }
}