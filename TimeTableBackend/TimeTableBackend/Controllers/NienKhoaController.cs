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
    public class NienKhoaController : ControllerBase
    {
        private readonly Context _context;

        public NienKhoaController(Context context)
        {
            _context = context;
        }

        // GET: api/NienKhoa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NienKhoa>>> GetNienKhoas()
        {
            return await _context.NienKhoas.ToListAsync();
        }

        // GET: api/NienKhoa/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NienKhoa>> GetNienKhoa(int id)
        {
            var nienKhoa = await _context.NienKhoas.FindAsync(id);

            if (nienKhoa == null)
            {
                return NotFound();
            }

            return nienKhoa;
        }

        // PUT: api/NienKhoa/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNienKhoa(int id, NienKhoa nienKhoa)
        {
            if (id != nienKhoa.Id)
            {
                return BadRequest();
            }

            _context.Entry(nienKhoa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NienKhoaExists(id))
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

        // POST: api/NienKhoa
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NienKhoa>> PostNienKhoa(NienKhoa nienKhoa)
        {
            _context.NienKhoas.Add(nienKhoa);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNienKhoa", new { id = nienKhoa.Id }, nienKhoa);
        }

        // DELETE: api/NienKhoa/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNienKhoa(int id)
        {
            var nienKhoa = await _context.NienKhoas.FindAsync(id);
            if (nienKhoa == null)
            {
                return NotFound();
            }

            _context.NienKhoas.Remove(nienKhoa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NienKhoaExists(int id)
        {
            return _context.NienKhoas.Any(e => e.Id == id);
        }
    }
}
