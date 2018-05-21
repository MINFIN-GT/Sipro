import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { DynamicComponentModule } from 'ng-dynamic';

import { Utilities } from './utilities';

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { AccesodenegadoComponent } from './components/accesodenegado/accesodenegado.component';
import { utils } from 'protractor';


const routes: Routes = [{
        path: 'accesodenegado',
        component: AccesodenegadoComponent
    },{
        path: 'login',
        component: LoginComponent
    }];

@NgModule({
  declarations: [
      AppComponent,
      LoginComponent, 
      AccesodenegadoComponent
  ],
  imports: [
      BrowserModule,
      FormsModule,
      RouterModule.forRoot(routes),
      HttpClientModule,
        DynamicComponentModule.forRoot({})
  ],
  providers: [Utilities],
  bootstrap: [AppComponent]
})
export class AppModule { }
