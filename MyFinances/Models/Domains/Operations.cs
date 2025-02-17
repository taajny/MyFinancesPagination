﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.Models.Domains
{
    public class Operations
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; } = null;

        public string Description { get; set; }

        public decimal Value { get; set; }

        public DateTime Date { get; set; }

        public int CategoryId { get; set; }
    }
}
