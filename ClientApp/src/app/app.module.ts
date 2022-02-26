import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';


import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { SmsModule } from './sms/sms.module';
import { AppRoutingModule } from './app-routing.module';
import { EmailModule } from './email/email.module';
import { AppComponent } from './app.component';
import { TokenModule } from './token/token.module';
import { PdfModule } from './pdf/pdf.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent
  ],
  imports: [
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    CommonModule,
    TokenModule,
    EmailModule,
    PdfModule,
    BrowserAnimationsModule,
    SmsModule,
  ],
  providers:[],
  bootstrap: [AppComponent]
})
export class AppModule { }
