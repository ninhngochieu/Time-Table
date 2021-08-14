class Buoi {
  id: number;
  soTiet: number;
  batDauLuc: number;
  tietBatDau: number;
  giangVien: string;
  phong: string;
}

class NhomMonHoc {
  id: string;
  phong: string;
  buoi: Buoi[];
}

export class MonHoc{
  id: number;
  ten: string;
  maMonHoc: string;
  soTinChi: number;
  nhomMonHoc: NhomMonHoc[];
  isActive = false;
}
