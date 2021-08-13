using System.Collections.Generic;

namespace TimeTableBackend.Models
{
    public class NhomMonHoc
    {
        public int Id { get; set; }
        public string NMH { get; set; }
        public List<Buoi> ? Buois{ get; set; }
    }
}