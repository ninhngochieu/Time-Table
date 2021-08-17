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
        public static int index = 0;
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
        public ActionResult<List<List<NhomMonHoc>>> PostDanhSachMonHoc(List<MonHoc> dsMonHoc)
        {
            index = 0;
            List<NhomMonHoc> nhomMonHocs = new List<NhomMonHoc>();
            List<List<NhomMonHoc>> listNhomMonHocs = new List<List<NhomMonHoc>>();
            foreach (var monhoc in dsMonHoc)
            {
                List<NhomMonHoc> nhomMonHocEx = _context.MonHocs.Where(i => i.Id == monhoc.Id).Include(n => n.NhomMonHoc).ThenInclude(s => s.Buois).FirstOrDefault().NhomMonHoc.ToList();
                nhomMonHocs.AddRange(nhomMonHocEx);
            }
            Dictionary<List<NhomMonHoc>, List<List<NhomMonHoc>>> dict = new Dictionary<List<NhomMonHoc>, List<List<NhomMonHoc>>>();
            List<List<NhomMonHoc>> list = SapXepMonHoc(nhomMonHocs, dict, dsMonHoc.Count);
            list.RemoveAll(p =>
            {
                return false;
                if (chongCheoTKB(p))
                {
                    return true;
                }
                return false;
                //if (chongCheoTkbAlgorithm(p))
                //{
                //    return true;
                //}
                return false;
            });
            return list;
        }

        private bool chongCheoTkbAlgorithm(List<NhomMonHoc> nhomMonHocs)
        {
            List<Buoi> buois = new List<Buoi>();
            nhomMonHocs.ForEach(tkb =>
            {
                tkb.Buois.ForEach(buoi =>
                {
                    Buoi buoiMoi = _context.Buois.Where(i => i.Id == buoi.Id).Include(n => n.NhomMonHoc).FirstOrDefault();
                    buois.Add(buoiMoi);
                });
            });
            buois.Sort((obj1, obj2) =>
            {
                if (obj1.BatDauLuc < obj2.BatDauLuc) return -1;
                else if (obj2.BatDauLuc > obj2.BatDauLuc) return 1;
                return 0;
            });

            for (int i = 1; i < buois.Count; i++)
            {
                if (buois[i - 1].NhomMonHocId == buois[i].NhomMonHocId)
                {
                    //Dam bao 2 buoi trong cung 1 nhom khong gap nhau
                }
                else
                {
                    //Kiem tra thu may
                    if (buois[i - 1].BatDauLuc == buois[i].BatDauLuc)
                    {
                        //Co trung tiet bat dau hay khong
                        if (buois[i - 1].TietBatDau == buois[i].TietBatDau)
                        {
                            return true;
                        }
                        else if (buois[i - 1].TietBatDau < buois[i].TietBatDau)
                        {
                            if (buois[i - 1].TietBatDau + buois[i - 1].SoTiet - 1 >= buois[i].TietBatDau) return true;
                        }
                        else
                        {
                            if (buois[i].TietBatDau + buois[i].SoTiet - 1 >= buois[i - 1].TietBatDau) return true;
                        }

                    }
                }
            }
            return false;
        }

        private bool chongCheoTKB(List<NhomMonHoc> nhomMonHocs)
        {
            List<Buoi> buois = new List<Buoi>();
            nhomMonHocs.ForEach(tkb=>
            {
                tkb.Buois.ForEach(buoi =>
                {
                    Buoi buoiMoi = _context.Buois.Where(i => i.Id == buoi.Id).Include(n => n.NhomMonHoc).FirstOrDefault();
                    buois.Add(buoiMoi);
                });
            });
            for(int i = 0; i < buois.Count; i++)
            {
                for(int j = i + 1;j<buois.Count - 1; j++)
                {
                    if(buois[i].NhomMonHocId == buois[j].NhomMonHocId)
                    {
                        //Dam bao 2 buoi trong cung 1 nhom khong gap nhau
                    }
                    else
                    {
                        //Kiem tra thu may
                        if (buois[i].BatDauLuc == buois[j].BatDauLuc)
                        {
                            //Co trung tiet bat dau hay khong
                            if(buois[i].TietBatDau == buois[j].TietBatDau)
                            {
                                return true;
                            }
                            else if(buois[i].TietBatDau < buois[j].TietBatDau)
                            {
                                if (buois[i].TietBatDau + buois[i].SoTiet - 1 >= buois[j].TietBatDau) return true;
                            }
                            else
                            {
                                if (buois[j].TietBatDau + buois[j].SoTiet - 1 >= buois[i].TietBatDau) return true;
                            }

                            //Kiem tra xem co bi de tiet hay khong
                            //else if 
                        }
                    }
                }
            }
            return false;
        }

        private List<List<NhomMonHoc>> SapXepMonHoc(List<NhomMonHoc> nhomMonHocs, Dictionary<List<NhomMonHoc>, List<List<NhomMonHoc>>> dict, int count)
        {
            //if (dict.ContainsKey(nhomMonHocs)) return dict[nhomMonHocs] ;

            List<List<NhomMonHoc>> result = new List<List<NhomMonHoc>>();
            if (nhomMonHocs.Count == 0) return new List<List<NhomMonHoc>>()
            {
                new List<NhomMonHoc>()
            };

            List<NhomMonHoc> nhomMonHocFilter = nhomMonHocs.Where(m => m.MonHocId == nhomMonHocs[0].MonHocId).ToList();
            List<NhomMonHoc> newNhomMonHoc = nhomMonHocs.Except(nhomMonHocFilter).ToList();

            foreach(var nhm in nhomMonHocFilter)
            {
                List<List<NhomMonHoc>> list = SapXepMonHoc(newNhomMonHoc, dict, count);
                //Quy hoach dong

                //De quy
                foreach (var l in list)
                {
                    //l.Add(nhm); //Current node result
                    l.Insert(0, nhm);
                }
                result.AddRange(list);

                //result.RemoveAll(tkb =>
                //{
                //    if (chongCheoTKB(tkb)) return true;
                //    return false;
                //});

            }
            //dict[nhomMonHocs] = result;
            return result;
        }
    }
}
