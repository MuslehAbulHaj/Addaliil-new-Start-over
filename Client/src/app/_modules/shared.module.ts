import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from 'ngx-spinner';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(), // this is userd in member details TABs
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-center',
    }), // ToastrModule added
    NgxGalleryModule, // this used for photo gallary
    NgxSpinnerModule.forRoot({
      type: 'ball-clip-rotate-pulse'
    })
  ],
  exports: [
    BsDropdownModule,
    ToastrModule,
    TabsModule, //when u imports u need to export also
    NgxGalleryModule,
    NgxSpinnerModule
  ]
})


export class SharedModule { }
