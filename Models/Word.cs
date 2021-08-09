using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class Word
    {
        public long BookId { get; set; }

        public string Name { get; set; }

        public long Count { get; set; }
    }
}