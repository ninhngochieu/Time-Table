namespace TimeTableBackend.Models
{
    public class Buoi
    {
        public int Id { get; set; }
        public int SoTiet { get; set; }
        public int BatDauLuc { get; set; }
        public int TietBatDau { get; set; }
        public string GiangVien { get; set; }
        public string Phong { get; set; }

        public int NhomMonHocId { get; set; }
        public virtual NhomMonHoc NhomMonHoc { get; set; }
    }
}