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
        public IList<MonHoc> ? MonHocs{ get; set; }
    }
}
