using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caching.DB;
using Caching.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Caching.Controllers
{
    public class MemDrugsController : Controller
    {
        private readonly ApiContext _context;
        private readonly IMemoryCache _cache;

        public MemDrugsController(ApiContext context, IMemoryCache memCache)
        {
            _context = context;
            _cache = memCache;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Drug> drugs;

            if (!_cache.TryGetValue("drugs", out drugs))
            {
                Thread.Sleep(3000);

                drugs = await _context.Drugs.ToListAsync();

                var cts = new CancellationTokenSource();
                _cache.Set("cts", cts);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(200));

                cacheEntryOptions.AddExpirationToken(new CancellationChangeToken(cts.Token));

                _cache.Set("drugs", drugs, cacheEntryOptions);
            }

            return View(drugs);
        }

        // GET: MemDrugs/Details/5
        public async Task<IActionResult> Details(int id)
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

            return View(drug);
        }

        // GET: MemDrugs/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Drug drug;
            var key = "drugs/" + id;

            if (!_cache.TryGetValue(key, out drug))
            {
                Thread.Sleep(3000);
                drug = await _context.Drugs.FindAsync(id);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(200));

                _cache.Set(key, drug, cacheEntryOptions);
            }

            if (drug == null)
            {
                return NotFound();
            }

            return View(drug);
        }

        // POST: MemDrugs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,drugName,drugPrice,drugNdc,packSize")] Drug drug)
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
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        // POST: MemDrugs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
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

            return RedirectToAction(nameof(Index));
        }
    }
}