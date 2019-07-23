using Caching.DB;
using Caching.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caching.Services
{
    public class IMemDrugsService
    {
        private readonly ApiContext _context;
        private readonly IMemoryCache _cache;

        public IMemDrugsService(ApiContext context, IMemoryCache memCache)
        {
            _context = context;
            _cache = memCache;
        }

        public List<Drug> GetDrugs()
        {
            return _context.Drugs.ToList();
        }
        
    }
}
