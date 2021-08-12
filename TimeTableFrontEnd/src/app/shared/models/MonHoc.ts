class Buoi {
  id: number;
  soTiet: number;
  tietBatDau: number;
  tietKetThuc: number;
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
}
