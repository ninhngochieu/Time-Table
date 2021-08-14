using System.Collections.Generic;

namespace TimeTableBackend.Models
{
    public class NhomMonHoc
    {
        public int Id { get; set; }
        public string NMH { get; set; }

        public int MonHocId { get; set; }
        public MonHoc MonHoc { get; set; }
        public List<Buoi> ? Buois{ get; set; }
    }
}