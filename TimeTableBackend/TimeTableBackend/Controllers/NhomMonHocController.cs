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
    public class NhomMonHocController : ControllerBase
    {
        private readonly Context _context;

        public NhomMonHocController(Context context)
        {
            _context = context;
        }

        // GET: api/NhomMonHoc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhomMonHoc>>> GetNhomMonHocs()
        {
            return await _context.NhomMonHocs.ToListAsync();
        }

        // GET: api/NhomMonHoc/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NhomMonHoc>> GetNhomMonHoc(int id)
        {
            var nhomMonHoc = await _context.NhomMonHocs.FindAsync(id);

            if (nhomMonHoc == null)
            {
                return NotFound();
            }

            return nhomMonHoc;
        }

        // PUT: api/NhomMonHoc/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhomMonHoc(int id, NhomMonHoc nhomMonHoc)
        {
            if (id != nhomMonHoc.Id)
            {
                return BadRequest();
            }

            _context.Entry(nhomMonHoc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhomMonHocExists(id))
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

        // POST: api/NhomMonHoc
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NhomMonHoc>> PostNhomMonHoc(NhomMonHoc nhomMonHoc)
        {
            _context.NhomMonHocs.Add(nhomMonHoc);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNhomMonHoc", new { id = nhomMonHoc.Id }, nhomMonHoc);
        }

        // DELETE: api/NhomMonHoc/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhomMonHoc(int id)
        {
            var nhomMonHoc = await _context.NhomMonHocs.FindAsync(id);
            if (nhomMonHoc == null)
            {
                return NotFound();
            }

            _context.NhomMonHocs.Remove(nhomMonHoc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NhomMonHocExists(int id)
        {
            return _context.NhomMonHocs.Any(e => e.Id == id);
        }
    }
}
