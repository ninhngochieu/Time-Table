import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {HeaderComponent} from './header/header.component';
import {FooterComponent} from './footer/footer.component';
import {LeftSidebarComponent} from './left-sidebar/left-sidebar.component';
import {RightSidebarComponent} from './right-sidebar/right-sidebar.component';
import {ContentComponent} from './content/content.component';



@NgModule({
  declarations: [
    HeaderComponent,
    FooterComponent,
    LeftSidebarComponent,
    RightSidebarComponent,
    ContentComponent,
    FooterComponent
  ],
  exports: [
    HeaderComponent,
    LeftSidebarComponent,
    ContentComponent,
    RightSidebarComponent,
    FooterComponent
  ],
  imports: [
    CommonModule,
  ]
})
export class ComponentsModule { }
