import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TokenComponent } from './token.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ButtonsModule } from '@progress/kendo-angular-buttons';
import { IconsModule } from '@progress/kendo-angular-icons';
import { InputsModule } from '@progress/kendo-angular-inputs';
import { IndicatorsModule } from '@progress/kendo-angular-indicators';
import { DialogsModule } from '@progress/kendo-angular-dialog';
import { LayoutModule } from '@progress/kendo-angular-layout';
import { DropDownsModule } from '@progress/kendo-angular-dropdowns';



@NgModule({
  declarations: [
    TokenComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    ButtonsModule,
    IconsModule,
    InputsModule,
    IndicatorsModule,
    DialogsModule,
    LayoutModule,
    ReactiveFormsModule,
    FormsModule,
    BrowserAnimationsModule,
    DropDownsModule
    
  ]
})
export class TokenModule { }
