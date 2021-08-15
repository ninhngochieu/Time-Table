using System;
using System.Collections.Generic;

namespace TimeTableBackend.Models
{
    public class NienKhoa
    {
        public NienKhoa()
        {
        }
        public int Id { get; set; }
        public string HocKy { get; set; }
        public string NamHoc{ get; set; }
        public virtual List<MonHoc> ? MonHocs{ get; set; }
    }
}
