using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Caching.DB;
using Caching.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using Caching.Services;
using Microsoft.Extensions.Primitives;

namespace Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InmemoryDrugsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMemoryCache _cache;
        private readonly ApiContext _callbackcontext;

        public InmemoryDrugsController(ApiContext context, ApiContext callbackContext, IMemoryCache memCache)
        {
            _context = context;
            _cache = memCache;
            _callbackcontext = callbackContext;
        }

        // GET: api/InmemoryDrugs
        [HttpGet]
        public IEnumerable<Drug> GetDrugs()
        {
            List<Drug> drugs;

            if (!_cache.TryGetValue("drugs", out drugs))
            {
                Thread.Sleep(5000);

                drugs = _context.Drugs.ToList();

                var cts = new CancellationTokenSource();
                _cache.Set("cts", cts);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(200));
                    
                cacheEntryOptions.AddExpirationToken(new CancellationChangeToken(cts.Token));

                _cache.Set("drugs", drugs, cacheEntryOptions);
            }

            return drugs;
        }

        // GET: api/InmemoryDrugs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDrug([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Drug drug;
            var key = "drugs/" + id;

            if (!_cache.TryGetValue(key, out drug))
            {
                Thread.Sleep(5000);
                drug = await _context.Drugs.FindAsync(id);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(200));

                _cache.Set(key, drug, cacheEntryOptions);
            }

            if (drug == null)
            {
                return NotFound();
            }

            return Ok(drug);
        }

        // PUT: api/InmemoryDrugs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDrug([FromRoute] int id, [FromBody] Drug drug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != drug.Id)
            {
                return BadRequest();
            }

            _context.Entry(drug).State = EntityState.Modified;
            var key = "drugs/" + id;

            try
            {
                await _context.SaveChangesAsync();
                if (_cache.TryGetValue(key, out drug))
                {
                    _cache.Set(key, drug);
                    CancellationTokenSource cts = _cache.Get<CancellationTokenSource>("cts");
                    cts.Cancel();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DrugExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/InmemoryDrugs
        [HttpPost]
        public async Task<IActionResult> PostDrug([FromBody] Drug drug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Drugs.Add(drug);
            await _context.SaveChangesAsync();
            CancellationTokenSource cts = _cache.Get<CancellationTokenSource>("cts");
            cts.Cancel();

            _context.Entry(drug).State = EntityState.Modified;

            return CreatedAtAction("GetDrug", new { id = drug.Id }, drug);
        }

        // DELETE: api/InmemoryDrugs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrug([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var drug = await _context.Drugs.FindAsync(id);
            if (drug == null)
            {
                return NotFound();
            }

            _context.Drugs.Remove(drug);

            await _context.SaveChangesAsync();

            _cache.Remove("drugs/" + id);
            CancellationTokenSource cts = _cache.Get<CancellationTokenSource>("cts");
            cts.Cancel();

            return Ok(drug);
        }

        private bool DrugExists(int id)
        {
            return _context.Drugs.Any(e => e.Id == id);
        }
    }
}