using System.Collections.Generic;

namespace TimeTableBackend.Models
{
    public class NhomMonHoc
    {
        public int Id { get; set; }
        public string NMH { get; set; }
        public string Phong { get; set; }
        public IList<Buoi> ? Buois{ get; set; }
    }
}