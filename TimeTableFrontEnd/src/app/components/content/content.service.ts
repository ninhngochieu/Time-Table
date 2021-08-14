import { Injectable } from '@angular/core';
import {NhomMonHoc} from "../../shared/models/MonHoc";
import {Subject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ContentService {

  tkbSubject = new Subject<NhomMonHoc[]>();
  constructor() { }

  postTKB(tkb: NhomMonHoc[]) {
    this.tkbSubject.next(tkb);
  }
}
