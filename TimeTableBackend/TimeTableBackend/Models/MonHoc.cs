using System;
using System.Collections;
using System.Collections.Generic;

namespace TimeTableBackend.Models
{
    public class MonHoc
    {
        public MonHoc()
        {
        }
        public int Id { get; set; }
        public string Ten { get; set; }
        public string MaMonHoc { get; set; }
        public int SoTinChi { get; set; }


        public virtual List<NhomMonHoc>? NhomMonHoc { get; set; }

        public int NienKhoaId { get; set; }
        public virtual NienKhoa NienKhoa { get; set; }
    }
}
