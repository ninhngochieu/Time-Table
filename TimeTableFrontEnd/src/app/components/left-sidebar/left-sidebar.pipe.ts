import { Pipe, PipeTransform } from '@angular/core';
import {NhomMonHoc} from '../../shared/models/MonHoc';

@Pipe({
  name: 'leftSidebar'
})
export class LeftSidebarPipe implements PipeTransform {

  transform(value: NhomMonHoc[] ): unknown {
    let name = '';
    value.forEach((x: NhomMonHoc)=>{
        name+= '-'+ x.nmh;
    });
    return name;
  }

}
