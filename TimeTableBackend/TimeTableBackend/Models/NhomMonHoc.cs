using System.Collections.Generic;

namespace TimeTableBackend.Models
{
    public class NhomMonHoc
    {
        public int Id { get; set; }
        public string NMH { get; set; }

        public int MonHocId { get; set; }
        public virtual MonHoc MonHoc { get; set; }
        public virtual List<Buoi> ? Buois{ get; set; }
    }
}