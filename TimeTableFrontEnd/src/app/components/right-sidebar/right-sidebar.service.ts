import { Injectable } from '@angular/core';
import {Observable} from 'rxjs';
import {MonHoc} from '../../shared/models/MonHoc';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RightSidebarService {

  constructor( private httpClient: HttpClient) { }

  getAllMonHoc(): Observable<any> {
    return this.httpClient.get('https://localhost:5001/api/MonHoc');
  }
}
