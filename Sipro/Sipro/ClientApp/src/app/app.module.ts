import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FlashMessagesModule } from 'angular2-flash-messages';
import { UtilsService } from './utils.service';
import { AuthService } from './auth.service';
import { RouteguardService } from './routeguard.service';
import { MaterialModule } from './material/material.module';
import { utils } from 'protractor'; 
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { MatPaginatorModule, MatTabsModule, MatDatepickerModule, MatNativeDateModule } from '@angular/material';
import { MomentModule } from 'angular2-moment/moment.module';

import { AppComponent } from './app.component';
import { MainComponent } from './components/main/main.component';
import { LoginComponent } from './components/login/login.component';
import { AccesodenegadoComponent } from './components/accesodenegado/accesodenegado.component';
import { PagenotfoundComponent } from './components/pagenotfound/pagenotfound.component';
import { FooterComponent } from './components/footer/footer.component';
import { MainmenuComponent } from './components/mainmenu/mainmenu.component';
import { PrestamoComponent } from './components/prestamo/prestamo.component';

const routes: Routes = [{
          path: '',    // Va a Main
          component: MainComponent,
          canActivate: [RouteguardService] 
        },{
          path: 'main',
          component: MainComponent,
          canActivate: [RouteguardService]
        },{
          path: 'main/prestamo',
          component: PrestamoComponent
        },{
          path: 'accesodenegado',
          component: AccesodenegadoComponent
        },{
          path: 'login',
          component: LoginComponent
        },{
          path: '**',
          component: PagenotfoundComponent
        }];

@NgModule({
  declarations: [
      AppComponent,
      LoginComponent, 
      AccesodenegadoComponent, MainComponent, PagenotfoundComponent, FooterComponent, MainmenuComponent, PrestamoComponent      
  ],
  imports: [
      BrowserModule,
      FormsModule,
      RouterModule.forRoot(routes),
      HttpClientModule,
      FlashMessagesModule.forRoot(),
      MaterialModule,
      Ng2SmartTableModule, 
      MatPaginatorModule, 
      MatTabsModule, MatDatepickerModule, MatNativeDateModule, MomentModule
  ],
  providers: [UtilsService, AuthService, RouteguardService],
  bootstrap: [AppComponent]
})
export class AppModule { 
  
}
