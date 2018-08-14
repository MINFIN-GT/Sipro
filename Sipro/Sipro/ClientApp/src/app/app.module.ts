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
import { MatInputModule, MatPaginatorModule, MatTabsModule, MatDatepickerModule, MatNativeDateModule, MAT_DATE_LOCALE, MatPaginatorIntl, MatTooltipModule, MatAutocompleteModule, MAT_DATE_FORMATS, MAT_NATIVE_DATE_FORMATS, DateAdapter, MatSelectModule, MatCheckboxModule } from '@angular/material';
import { MomentModule } from 'angular2-moment/moment.module';
import { MatDialogModule } from '@angular/material/dialog';
import { DialogCodigoPresupuestario } from '../assets/modals/codigopresupuestario/modal-codigo-presupuestario';
import { DialogMoneda } from '../assets/modals/tipomoneda/modal-moneda';
import { DialogTipoPrestamo } from '../assets/modals/tipoprestamo/modal-tipo-prestamo';
import { DialogProyectoTipo } from '../assets/modals/peptipo/proyecto-tipo';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginatorIntlSpanish } from '../assets/customs/custom-paginator-spanish';
import { ButtonDeleteComponent } from '../assets/customs/ButtonDeleteComponent';
import { ButtonDownloadComponent } from '../assets/customs/ButtonDownloadComponent';
import { DialogDownloadDocument } from '../assets/modals/documentosadjuntos/documento-adjunto';
import { FormatoMillones, FormatoMillonesDolares, FormatoMillonesSinTipo } from '../assets/pipes/FormatoMillones.pipe';
import { MatMomentDateModule, MomentDateAdapter } from '@angular/material-moment-adapter';
import { CUSTOM_DATE_FORMAT } from '../assets/customs/formatdate/CUSTOM_DATE_FORMAT';
import { DialogProyectoPropiedad } from '../assets/modals/proyectopropiedad/modal-proyecto-propiedad';
import { DialogDelete } from '../assets/modals/deleteconfirmation/confirmation-delete';
import { CurrencyMaskModule } from "ng2-currency-mask";
import { DialogUnidadEjecutora } from '../assets/modals/unidadejecutora/unidad-ejecutora';
import { DialogColaborador } from '../assets/modals/colaborador/modal-colaborador';
import { DialogImpacto } from '../assets/modals/impacto/modal-impacto';
import { DialogEntidad } from '../assets/modals/entidad/modal-entidad';
import { DialogCargarProject } from '../assets/modals/cargarproject/modal-cargar-project';
import { NguiMapModule } from '@ngui/map';
import { DialogMapa } from '../assets/modals/cargamapa/modal-carga-mapa';
import { DialogComponentePropiedad } from '../assets/modals/componentepropiedad/modal-componente-propiedad';
import { DialogComponenteTipo } from '../assets/modals/componentetipo/componente-tipo';
import { DialogSubComponentePropiedad } from '../assets/modals/subcomponentepropiedad/modal-subcomponente-propiedad';
import { DialogSubComponenteTipo } from '../assets/modals/subcomponentetipo/modal-subcomponente-tipo';

import { AppComponent } from './app.component';
import { MainComponent } from './components/main/main.component';
import { LoginComponent } from './components/login/login.component';
import { AccesodenegadoComponent } from './components/accesodenegado/accesodenegado.component';
import { PagenotfoundComponent } from './components/pagenotfound/pagenotfound.component';
import { FooterComponent } from './components/footer/footer.component';
import { MainmenuComponent } from './components/mainmenu/mainmenu.component';
import { PrestamoComponent } from './components/prestamo/prestamo.component';
import { PepComponent } from './components/pep/pep.component';
import { PrestamotipoComponent } from './components/prestamotipo/prestamotipo.component';
import { PeppropiedadComponent } from './components/peppropiedad/peppropiedad.component';
import { PeptipoComponent } from './components/peptipo/peptipo.component';
import { ComponenteComponent } from './components/componente/componente.component';
import { ComponentepropiedadComponent } from './components/componentepropiedad/componentepropiedad.component';
import { ComponentetipoComponent } from './components/componentetipo/componentetipo.component';
import { SubcomponenteComponent } from './components/subcomponente/subcomponente.component';
import { SubcomponentepropiedadComponent } from './components/subcomponentepropiedad/subcomponentepropiedad.component';
import { SubcomponentetipoComponent } from './components/subcomponentetipo/subcomponentetipo.component';

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
          component: PepComponent
        },{
          path: 'main/peppropiedad',
          component: PeppropiedadComponent
        },{
          path: 'main/peptipo',
          component: PeptipoComponent
        },{
          path: 'main/componente/:id',
          component: ComponenteComponent
        },{
          path: 'main/componentepropiedad',
          component: ComponentepropiedadComponent
        },{
          path: 'main/componentetipo',
          component: ComponentetipoComponent
        },{
          path: 'main/subcomponente/:id',
          component: SubcomponenteComponent
        },{
          path: 'main/subcomponentepropiedad',
          component: SubcomponentepropiedadComponent
        },{
          path: 'main/subcomponentetipo',
          component: SubcomponentetipoComponent
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
      DialogCodigoPresupuestario, DialogMoneda, DialogTipoPrestamo, ButtonDeleteComponent, ButtonDownloadComponent, DialogDownloadDocument, 
      FormatoMillones, FormatoMillonesDolares, FormatoMillonesSinTipo, PepComponent, DialogProyectoTipo, PrestamotipoComponent, 
      PeppropiedadComponent, PeptipoComponent, DialogProyectoPropiedad, DialogDelete, DialogUnidadEjecutora, DialogColaborador,
      DialogImpacto, DialogEntidad, ComponenteComponent, DialogCargarProject, DialogMapa, ComponentepropiedadComponent, ComponentetipoComponent,
      DialogComponentePropiedad, DialogComponenteTipo, SubcomponenteComponent, SubcomponentepropiedadComponent, SubcomponentetipoComponent,
      DialogSubComponentePropiedad, DialogSubComponenteTipo
  ],
  imports: [
      BrowserModule,
      FormsModule, ReactiveFormsModule, 
      NguiMapModule.forRoot({apiUrl: 'https://maps.google.com/maps/api/js?key=AIzaSyDgZeBouc6cujKM2NM3_F_jMJbKUc1XjKk'}),
      RouterModule.forRoot(routes),
      HttpClientModule,
      FlashMessagesModule.forRoot(),
      MaterialModule,
      FlexLayoutModule,      
      Ng2SmartTableModule, 
      MatPaginatorModule, MatTabsModule, MatDatepickerModule, MatNativeDateModule, MomentModule, 
      MatInputModule, MatDialogModule, MatProgressSpinnerModule, MatTooltipModule, MatAutocompleteModule, 
      MatMomentDateModule, MatSelectModule, MatCheckboxModule, CurrencyMaskModule
  ],
  providers: [UtilsService, AuthService, RouteguardService, 
    { provide: MatPaginatorIntl, useClass: MatPaginatorIntlSpanish },
    { provide: DateAdapter, useClass: MomentDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: CUSTOM_DATE_FORMAT }
  ],
  bootstrap: [AppComponent],
  entryComponents: [DialogCodigoPresupuestario, DialogMoneda, DialogTipoPrestamo, ButtonDeleteComponent, ButtonDownloadComponent, 
    DialogDownloadDocument, DialogProyectoTipo, DialogProyectoPropiedad, DialogDelete, DialogUnidadEjecutora, DialogColaborador,
    DialogImpacto, DialogEntidad, DialogCargarProject, DialogMapa, DialogComponentePropiedad, DialogComponenteTipo, DialogSubComponentePropiedad,
    DialogSubComponenteTipo]
})
export class AppModule { 

}
