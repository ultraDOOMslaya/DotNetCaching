using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Caching.DB;
using Caching.Models;
using System.Threading;

namespace Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugsController : ControllerBase
    {
        private readonly ApiContext _context;

        public DrugsController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/Drugs
        [HttpGet]
        public IEnumerable<Drug> GetDrugs()
        {
            Thread.Sleep(5000);
            return _context.Drugs;
        }

        // GET: api/Drugs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDrug([FromRoute] int id)
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

            return Ok(drug);
        }

        // PUT: api/Drugs/5
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

            try
            {
                await _context.SaveChangesAsync();
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

        // POST: api/Drugs
        [HttpPost]
        public async Task<IActionResult> PostDrug([FromBody] Drug drug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Drugs.Add(drug);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDrug", new { id = drug.Id }, drug);
        }

        // DELETE: api/Drugs/5
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

            return Ok(drug);
        }

        private bool DrugExists(int id)
        {
            return _context.Drugs.Any(e => e.Id == id);
        }
    }
}