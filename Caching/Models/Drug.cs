using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caching.Models
{
    public class Drug
    {
        public int Id { get; set; }
        public string drugNdc { get; set; }
        public string drugName { get; set; }
        public decimal drugPrice { get; set; }
        public int packSize { get; set; }
    }
}
