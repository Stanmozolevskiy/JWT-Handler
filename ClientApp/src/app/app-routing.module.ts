import { EmailComponent } from './email/email.component';
import { TokenComponent } from './token/token.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PdfComponent } from './pdf/pdf.component';
import { SmsComponent } from './sms/sms.component';

const routes: Routes = [
  { path: '', component: TokenComponent },
  { path: 'token', component: TokenComponent },
  { path: 'email', component: EmailComponent },
  { path: 'pdf', component: PdfComponent },
  { path: 'sms', component: SmsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'enabled' } )],
  exports: [RouterModule]
})
export class AppRoutingModule { }
