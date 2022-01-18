import { TokenComponent } from './token/token.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [{ path: '', component: TokenComponent }];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'enabled' } )],
  exports: [RouterModule]
})
export class AppRoutingModule { }
