import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {HeaderComponent} from './header/header.component';
import {FooterComponent} from './footer/footer.component';
import {LeftSidebarComponent} from './left-sidebar/left-sidebar.component';
import {RightSidebarComponent} from './right-sidebar/right-sidebar.component';
import {ContentComponent} from './content/content.component';
import { ComponentsComponent } from './components.component';
import { RightSidebarPipe } from './right-sidebar/right-sidebar.pipe';
import {FormsModule} from "@angular/forms";



@NgModule({
  declarations: [
    HeaderComponent,
    FooterComponent,
    LeftSidebarComponent,
    RightSidebarComponent,
    ContentComponent,
    FooterComponent,
    ComponentsComponent,
    RightSidebarPipe
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
        FormsModule,
    ]
})
export class ComponentsModule { }
