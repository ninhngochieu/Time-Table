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
    public class MonHocController : ControllerBase
    {
        private readonly Context _context;

        public MonHocController(Context context)
        {
            _context = context;
        }

        // GET: api/MonHoc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MonHoc>>> GetMonHocs()
        {
            return await _context.MonHocs.Include(n=>n.NhomMonHoc).ThenInclude(n=>n.Buois)
                .ToListAsync();
        }

        // GET: api/MonHoc/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MonHoc>> GetMonHoc(int id)
        {
            var monHoc = await _context.MonHocs.FindAsync(id);

            if (monHoc == null)
            {
                return NotFound();
            }

            return monHoc;
        }

        // PUT: api/MonHoc/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMonHoc(int id, MonHoc monHoc)
        {
            if (id != monHoc.Id)
            {
                return BadRequest();
            }

            _context.Entry(monHoc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonHocExists(id))
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

        // POST: api/MonHoc
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MonHoc>> PostMonHoc(MonHoc monHoc)
        {
            _context.MonHocs.Add(monHoc);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMonHoc", new { id = monHoc.Id }, monHoc);
        }

        // DELETE: api/MonHoc/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMonHoc(int id)
        {
            var monHoc = await _context.MonHocs.FindAsync(id);
            if (monHoc == null)
            {
                return NotFound();
            }

            _context.MonHocs.Remove(monHoc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MonHocExists(int id)
        {
            return _context.MonHocs.Any(e => e.Id == id);
        }
        [HttpPost("SapXepDanhSachMonHoc")]
        public async Task<ActionResult<List<List<NhomMonHoc>>>> PostDanhSachMonHoc(List<MonHoc> dsMonHoc)
        {

            List<NhomMonHoc> nhomMonHocs = new List<NhomMonHoc>();
            List<List<NhomMonHoc>> listNhomMonHocs = new List<List<NhomMonHoc>>();
            foreach(var monhoc in dsMonHoc)
            {
                List<NhomMonHoc> nhomMonHocEx = _context.MonHocs.Where(i => i.Id == monhoc.Id).Include(n => n.NhomMonHoc).ThenInclude(s => s.Buois).FirstOrDefault().NhomMonHoc.ToList();
                nhomMonHocs.AddRange(nhomMonHocEx);
            }
            List<List<NhomMonHoc>> list = SapXepMonHoc(nhomMonHocs);
            return list;
        }

        private List<List<NhomMonHoc>> SapXepMonHoc(List<NhomMonHoc> nhomMonHocs)
        {
            List<List<NhomMonHoc>> result = new List<List<NhomMonHoc>>();
            if (nhomMonHocs.Count == 0) return new List<List<NhomMonHoc>>()
            {
                new List<NhomMonHoc>()
            };

            List<NhomMonHoc> nhomMonHocFilter = nhomMonHocs.Where(m => m.MonHocId == nhomMonHocs[0].MonHocId).ToList();
            List<NhomMonHoc> newNhomMonHoc = nhomMonHocs.Except(nhomMonHocFilter).ToList();

            foreach(var nhm in nhomMonHocFilter)
            {
                List<List<NhomMonHoc>> list = SapXepMonHoc(newNhomMonHoc);
                foreach(var l in list)
                {
                    l.Add(nhm); //Current node result
                }
                result.AddRange(list);
            }
            return result;
        }
    }
}
