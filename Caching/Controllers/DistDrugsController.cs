using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caching.DB;
using Caching.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Caching.Controllers
{
    [Controller]
    public class DistDrugsController : Controller
    {
        private readonly ApiContext _context;
        private readonly IDistributedCache _cache;
        private readonly string _drugsKey;
        private CancellationToken cts;

        public DistDrugsController(ApiContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
            _drugsKey = "drugs";
        }

        // GET: DistDrugs
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Drug> drugs;
            var key = "drugs";
            var cachedDrugs = await _cache.GetStringAsync(key);
            
            if (cachedDrugs == null)
            {
                Thread.Sleep(3000);

                drugs = await _context.Drugs.ToListAsync();
                cachedDrugs = JsonConvert.SerializeObject(drugs);
                var options = new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(200)
                };
                await _cache.SetStringAsync(key, cachedDrugs, options);
            }
            drugs = JsonConvert.DeserializeObject<List<Drug>>(cachedDrugs);
            await _cache.RefreshAsync(key);

            return View(drugs);
        }

        // GET: DistDrugs/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Drug drug;
            var key = "drug/"+id;
            var cachedDrug = await _cache.GetStringAsync(key);

            if (cachedDrug == null)
            {
                Thread.Sleep(3000);
                drug = await _context.Drugs.FindAsync(id);
                cachedDrug = JsonConvert.SerializeObject(drug);
                var options = new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(200)
                };
                await _cache.SetStringAsync(key, cachedDrug, options);
            }
            drug = JsonConvert.DeserializeObject<Drug>(cachedDrug);
            await _cache.RefreshAsync(key);

            return View(drug);
        }

        // GET: DistDrugs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var key = "drug/"+id;
            Drug drug;
            var cachedDrug = await _cache.GetStringAsync(key);


            if (cachedDrug == null)
            {
                Thread.Sleep(3000);

                drug = await _context.Drugs.FindAsync(id);
                cachedDrug = JsonConvert.SerializeObject(drug);
                var options = new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(200)
                };
                await _cache.SetStringAsync(key, cachedDrug, options);
            }
            drug = JsonConvert.DeserializeObject<Drug>(cachedDrug);
            await _cache.RefreshAsync(key);

            return View(drug);
        }

        // POST: DistDrugs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,drugName,drugPrice,drugNdc,packSize")] Drug drug)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var key = "drug/" + id;

                    _context.Update(drug);
                    await _context.SaveChangesAsync();
                    var options = new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromSeconds(200)
                    };
                    await _cache.SetStringAsync(key, JsonConvert.SerializeObject(drug), options);
                    await _cache.RemoveAsync("drugs");
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(drug);
        }

        // POST: DistDrugs/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var drug = await _context.Drugs.FindAsync(id);
            if (drug == null)
            {
                return NotFound();
            }
            var key = "drugs/"+id;

            _context.Drugs.Remove(drug);

            await _context.SaveChangesAsync();

            await _cache.RemoveAsync(key);
            await _cache.RemoveAsync("drugs");

            return RedirectToAction(nameof(Index));
        }
    }
}