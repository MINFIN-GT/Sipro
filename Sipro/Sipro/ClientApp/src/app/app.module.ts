import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FlashMessagesModule } from 'angular2-flash-messages';
import { UtilsService } from './utils.service';
import { AuthService } from './auth.service';
import { RouteguardService } from './routeguard.service';
import { MaterialModule } from './material/material.module';
import { FlexLayoutModule } from "@angular/flex-layout";
import { utils } from 'protractor'; 
import { Ng2SmartTableModule } from 'ng2-smart-table';
import { MatInputModule, MatPaginatorModule, MatTabsModule, MatDatepickerModule, MatNativeDateModule, MAT_DATE_LOCALE, MatPaginatorIntl, MatTooltipModule, MatAutocompleteModule } from '@angular/material';
import { MomentModule } from 'angular2-moment/moment.module';
import { MatDialogModule } from '@angular/material/dialog';
import { DialogOverviewCodigoPresupuestario, DialogCodigoPresupuestario } from './components/prestamo/modals/modal-codigo-presupuestario'
import { DialogOverviewMoneda, DialogMoneda } from './components/prestamo/modals/modal-moneda'
import { DialogOverviewTipoPrestamo, DialogTipoPrestamo } from './components/prestamo/modals/modal-tipo-prestamo'
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginatorIntlSpanish } from '../assets/ts/custom-paginator-spanish';
import { ButtonDeleteComponent } from '../assets/ts/ButtonDeleteComponent';
import { ButtonDownloadComponent } from '../assets/ts/ButtonDownloadComponent';
import { DialogOverviewDownloadDocument, DialogDownloadDocument } from '../assets/ts/documentosadjuntos/documento-adjunto';
import { FormatoMillones, FormatoMillonesDolares, FormatoMillonesSinTipo } from '../assets/ts/FormatoMillones';

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
      AccesodenegadoComponent, MainComponent, PagenotfoundComponent, FooterComponent, MainmenuComponent, PrestamoComponent,
      DialogOverviewCodigoPresupuestario, DialogCodigoPresupuestario, DialogOverviewMoneda, DialogMoneda, DialogOverviewTipoPrestamo, 
      DialogTipoPrestamo, ButtonDeleteComponent, ButtonDownloadComponent, DialogOverviewDownloadDocument, DialogDownloadDocument, 
      FormatoMillones, FormatoMillonesDolares, FormatoMillonesSinTipo
  ],
  imports: [
      BrowserModule,
      FormsModule, ReactiveFormsModule, 
      RouterModule.forRoot(routes),
      HttpClientModule,
      FlashMessagesModule.forRoot(),
      MaterialModule,
      FlexLayoutModule,      
      Ng2SmartTableModule, 
      MatPaginatorModule, MatTabsModule, MatDatepickerModule, MatNativeDateModule, MomentModule, 
      MatInputModule, MatDialogModule, MatProgressSpinnerModule, MatTooltipModule, MatAutocompleteModule
  ],
  providers: [UtilsService, AuthService, RouteguardService, 
    { provide: MAT_DATE_LOCALE, useValue: 'es-ES' },
    { provide: LOCALE_ID, useValue: "es-ES" },
    { provide: MatPaginatorIntl, useClass: MatPaginatorIntlSpanish }    
  ],
  bootstrap: [AppComponent],
  entryComponents: [DialogOverviewCodigoPresupuestario, DialogCodigoPresupuestario, DialogOverviewMoneda, DialogMoneda,
    DialogOverviewTipoPrestamo, DialogTipoPrestamo, ButtonDeleteComponent, ButtonDownloadComponent,DialogOverviewDownloadDocument, 
    DialogDownloadDocument]
})
export class AppModule { 
  
}
