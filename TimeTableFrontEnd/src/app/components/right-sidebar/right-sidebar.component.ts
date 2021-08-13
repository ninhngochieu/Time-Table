import { Component, OnInit } from '@angular/core';
import {MonHoc} from '../../shared/models/MonHoc';
import {RightSidebarService} from './right-sidebar.service';

@Component({
  selector: 'app-right-sidebar',
  templateUrl: './right-sidebar.component.html',
  styleUrls: ['./right-sidebar.component.scss']
})
export class RightSidebarComponent implements OnInit {
  monhocs: MonHoc[] = [];
  keyword = '';
  tempMonHoc: MonHoc[] = [];

  constructor(private rightSidebarService: RightSidebarService) { }

  ngOnInit(): void {
    this.rightSidebarService.getAllMonHoc().subscribe((monHocs: MonHoc[]) => {
      this.monhocs = monHocs;
    });
  }

  onChecked($event: any, monhoc: MonHoc) {
    monhoc.isActive = !monhoc.isActive;
    if (monhoc.isActive){
      if (!this.tempMonHoc.includes(monhoc)){
        this.tempMonHoc.push(monhoc);
      }
    }else {
      this.tempMonHoc = this.tempMonHoc.filter(x=>x!==monhoc);
    }
  }

  removeMonHoc($event: Event, monhoc: MonHoc) {
    monhoc.isActive = !monhoc.isActive;
    this.tempMonHoc = this.tempMonHoc.filter(x=>x!==monhoc);
  }
}
