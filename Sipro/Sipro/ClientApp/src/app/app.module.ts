import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
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
import { MatInputModule, MatPaginatorModule, MatTabsModule, MatDatepickerModule, MatNativeDateModule, MAT_DATE_LOCALE, MatPaginatorIntl, MatTooltipModule, MatAutocompleteModule, MAT_DATE_FORMATS, MAT_NATIVE_DATE_FORMATS, DateAdapter } from '@angular/material';
import { MomentModule } from 'angular2-moment/moment.module';
import { MatDialogModule } from '@angular/material/dialog';
import { DialogOverviewCodigoPresupuestario, DialogCodigoPresupuestario } from './components/prestamo/modals/modal-codigo-presupuestario'
import { DialogOverviewMoneda, DialogMoneda } from './components/prestamo/modals/modal-moneda'
import { DialogOverviewTipoPrestamo, DialogTipoPrestamo } from './components/prestamo/modals/modal-tipo-prestamo'
import { DialogOverviewProyectoTipo, DialogProyectoTipo } from './components/proyecto/modals/proyecto-tipo'
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginatorIntlSpanish } from '../assets/customs/custom-paginator-spanish';
import { ButtonDeleteComponent } from '../assets/customs/ButtonDeleteComponent';
import { ButtonDownloadComponent } from '../assets/customs/ButtonDownloadComponent';
import { DialogOverviewDownloadDocument, DialogDownloadDocument } from '../assets/modals/documentosadjuntos/documento-adjunto';
import { DialogDelete, DialogOverviewDelete } from './components/prestamo/modals/confirmation-delete';
import { FormatoMillones, FormatoMillonesDolares, FormatoMillonesSinTipo } from '../assets/pipes/FormatoMillones.pipe';
import { MatMomentDateModule, MomentDateAdapter } from '@angular/material-moment-adapter';
import { CUSTOM_DATE_FORMAT } from '../assets/customs/formatdate/CUSTOM_DATE_FORMAT';

import { AppComponent } from './app.component';
import { MainComponent } from './components/main/main.component';
import { LoginComponent } from './components/login/login.component';
import { AccesodenegadoComponent } from './components/accesodenegado/accesodenegado.component';
import { PagenotfoundComponent } from './components/pagenotfound/pagenotfound.component';
import { FooterComponent } from './components/footer/footer.component';
import { MainmenuComponent } from './components/mainmenu/mainmenu.component';
import { PrestamoComponent } from './components/prestamo/prestamo.component';
import { ProyectoComponent } from './components/proyecto/proyecto.component';
import { PrestamotipoComponent } from './components/prestamotipo/prestamotipo.component';

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
          path: 'main/prestamotipo',
          component: PrestamotipoComponent
        },{
          path: 'main/pep/:id',
          component: ProyectoComponent
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
      FormatoMillones, FormatoMillonesDolares, FormatoMillonesSinTipo, ProyectoComponent, DialogDelete, DialogOverviewDelete,
      DialogOverviewProyectoTipo, DialogProyectoTipo, PrestamotipoComponent
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
      MatInputModule, MatDialogModule, MatProgressSpinnerModule, MatTooltipModule, MatAutocompleteModule, MatMomentDateModule
  ],
  providers: [UtilsService, AuthService, RouteguardService, 
    { provide: MatPaginatorIntl, useClass: MatPaginatorIntlSpanish },
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: CUSTOM_DATE_FORMAT },
  ],
  bootstrap: [AppComponent],
  entryComponents: [DialogOverviewCodigoPresupuestario, DialogCodigoPresupuestario, DialogOverviewMoneda, DialogMoneda,
    DialogOverviewTipoPrestamo, DialogTipoPrestamo, ButtonDeleteComponent, ButtonDownloadComponent,DialogOverviewDownloadDocument, 
    DialogDownloadDocument, DialogDelete, DialogOverviewDelete, DialogOverviewProyectoTipo, DialogProyectoTipo]
})
export class AppModule { 

}
