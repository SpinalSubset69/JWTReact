import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LogincardComponent } from './logincard/logincard.component';




@NgModule({
  declarations: [
    LogincardComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    LogincardComponent
  ]
})
export class ComponentsModule { }
