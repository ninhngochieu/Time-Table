using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTableBackend.Models;

namespace TimeTableBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuoiController : ControllerBase
    {
        private readonly Context _context;

        public BuoiController(Context context)
        {
            _context = context;
        }

        // GET: api/Buoi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Buoi>>> GetBuois()
        {
            return await _context.Buois.ToListAsync();
        }

        // GET: api/Buoi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Buoi>> GetBuoi(int id)
        {
            var buoi = await _context.Buois.FindAsync(id);

            if (buoi == null)
            {
                return NotFound();
            }

            return buoi;
        }

        // PUT: api/Buoi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuoi(int id, Buoi buoi)
        {
            if (id != buoi.Id)
            {
                return BadRequest();
            }

            _context.Entry(buoi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuoiExists(id))
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

        // POST: api/Buoi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Buoi>> PostBuoi(Buoi buoi)
        {
            _context.Buois.Add(buoi);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBuoi", new { id = buoi.Id }, buoi);
        }

        // DELETE: api/Buoi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuoi(int id)
        {
            var buoi = await _context.Buois.FindAsync(id);
            if (buoi == null)
            {
                return NotFound();
            }

            _context.Buois.Remove(buoi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BuoiExists(int id)
        {
            return _context.Buois.Any(e => e.Id == id);
        }
    }
}
