import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { MatDialog } from '@angular/material';
import { SubcomponentePropiedad } from './model/SubcomponentePropiedad';
import { DialogDelete, DialogOverviewDelete } from '../../../assets/modals/deleteconfirmation/confirmation-delete';

@Component({
  selector: 'app-subcomponentepropiedad',
  templateUrl: './subcomponentepropiedad.component.html',
  styleUrls: ['./subcomponentepropiedad.component.css']
})
export class SubcomponentepropiedadComponent implements OnInit {
  mostrarcargando : boolean;
  color = 'primary';
  mode = 'indeterminate';
  value = 50;
  diameter = 45;
  strokewidth = 3;

  isLoggedIn : boolean;
  isMasterPage : boolean;
  esColapsado : boolean;
  elementosPorPagina : number;
  numeroMaximoPaginas : number;
  totalSubComponentePropiedades : number;
  subcomponentepropiedad : SubcomponentePropiedad;
  busquedaGlobal: string;
  paginaActual : number;
  source: LocalDataSource;
  esNuevo : boolean;
  datoTipoSelected: number;
  tipodatos = [];
  modalDelete: DialogOverviewDelete;

  @ViewChild('search') divSearch: ElementRef;

  constructor(private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog) {
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.totalSubComponentePropiedades = 0;
    this.subcomponentepropiedad = new SubcomponentePropiedad();
    this.source = new LocalDataSource();
    this.modalDelete = new DialogOverviewDelete(dialog);
  }

  ngOnInit() {
    this.mostrarcargando = true;
    this.obtenerTotalSubComponentePropiedades();
    this.obtenerDatosTipo();
  }

  obtenerTotalSubComponentePropiedades(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      t:moment().unix()
    };

    this.http.post('http://localhost:60081/api/SubcomponentePropiedad/NumeroSubComponentePropiedades', data, { withCredentials : true }).subscribe(response =>{
      if(response['success'] == true){
        this.totalSubComponentePropiedades = response['totalsubcomponentepropiedades'];
        this.paginaActual = 1;
        if(this.totalSubComponentePropiedades > 0)
          this.cargarTabla(this.paginaActual);
        else{
          this.source = new LocalDataSource();
          this.mostrarcargando = false;
        }
      }
      else{
        this.source = new LocalDataSource();
      }
    })
  }

  cargarTabla(pagina? : any){
    this.mostrarcargando = true;
    var filtro = {
      pagina: pagina,
      numeroSubComponentePropiedad: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      t:moment().unix()
    };

    this.http.post('http://localhost:60081/api/SubcomponentePropiedad/SubComponentePropiedadPagina', filtro, { withCredentials : true }).subscribe(response =>{
      if(response['success'] == true){
        var data = response['subcomponentepropiedades'];
        this.source = new LocalDataSource(data);
        this.source.setSort([
          { field: 'id', direction: 'asc' }  // primary sort
        ]);
        this.busquedaGlobal = null;

        this.mostrarcargando = false;
      }
    })
  }

  obtenerDatosTipo(){
    this.http.get('http://localhost:60017/api/DatoTipo/Listar', { withCredentials : true }).subscribe(response =>{
      if(response['success'] == true){
        this.tipodatos = response['datoTipos']
      }
    })
  }

  cambioOpcionDatoTipo(opcion){
    this.subcomponentepropiedad.datoTipoid = opcion;
  }

  handlePage(event){
    this.cargarTabla(event.pageIndex+1);
  }

  nuevo(){
    this.esColapsado = true;
    this.esNuevo = true;
    this.subcomponentepropiedad = new SubcomponentePropiedad();
    this.datoTipoSelected = 0;
  }

  editar(){
    if(this.subcomponentepropiedad.id > 0){
      this.esColapsado = true;
      this.esNuevo = false;
      this.datoTipoSelected = this.subcomponentepropiedad.datoTipoid;
    }
    else{
      this.utils.mensaje('warning', 'Debe seleccionar la Propiedad de subcomponente que desea editar');
    }
  }

  borrar(){
    if(this.subcomponentepropiedad.id > 0){
      this.modalDelete.dialog.open(DialogDelete, {
        width: '600px',
        height: '200px',
        data: { 
          id: this.subcomponentepropiedad.id,
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar la propiedad de subcomponente?',
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result != null && result){
          this.http.delete('http://localhost:60081/api/SubcomponentePropiedad/SubComponentePropiedad/'+ this.subcomponentepropiedad.id, { withCredentials : true }).subscribe(response =>{
            if(response['success'] == true){
              this.obtenerTotalSubComponentePropiedades();
            }
          })  
        }
      })
    }
    else{
      this.utils.mensaje('warning', 'Seleccione una propiedad de componente');
    }
  }

  filtrar(campo){
    this.busquedaGlobal = campo;
    this.obtenerTotalSubComponentePropiedades();
  }

  onDblClickRow(event){
    this.subcomponentepropiedad = event.data;
    this.editar();
  }

  onSelectRow(event){
    this.subcomponentepropiedad = event.data;
  }

  guardar(){
    if(this.subcomponentepropiedad != null && Number(this.datoTipoSelected) != 0){
      var objetoHttp;

      if(this.subcomponentepropiedad.id > 0){
        objetoHttp = this.http.put("http://localhost:60081/api/SubcomponentePropiedad/SubComponentePropiedad/" + this.subcomponentepropiedad.id, this.subcomponentepropiedad, { withCredentials: true });
      }
      else{
        objetoHttp = this.http.post("http://localhost:60081/api/SubcomponentePropiedad/SubComponentePropiedad", this.subcomponentepropiedad, { withCredentials: true });
      }

      objetoHttp.subscribe(response =>{
        if(response['success'] == true){
          this.subcomponentepropiedad.usuarioCreo = response['usuarioCreo'];
          this.subcomponentepropiedad.fechaCreacion = response['fechaCreacion'];
          this.subcomponentepropiedad.fechaActualizacion = response['fechaActualizacion'];
          this.subcomponentepropiedad.usuarioActualizo = response['usuarioActualizo'];
          this.subcomponentepropiedad.id = response['id'];

          this.esNuevo = false;
          this.obtenerTotalSubComponentePropiedades();
          this.utils.mensaje('success', 'Propiedad guardada exitosamente');
        }
      })
    }
    else{
      this.utils.mensaje('warning', 'Debe seleccionar un Tipo de dato');
    }
  }

  IrATabla(){
    this.esColapsado = false;
    this.subcomponentepropiedad = new SubcomponentePropiedad();
  }

  settings = {
    columns: {
      id: {
        title: 'ID',
        width: '6%',
        filter: false,
        type: 'html',
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + cell + "</div>";
        }
      },
      nombre: {
        title: 'Nombre',
        filter: false,       
      },
      descripcion: {
        title: 'Descripción',
        filter: false,       
      },
      datoTipoNombre: {
        title: 'Tipo dato',
        filter: false,       
      },
      usuarioCreo: {
        title: 'Usuario Creación',
        filter: false
      },
      fechaCreacion:{
        title: 'Fecha Creación',
        type: 'html',
        filter: false,
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + moment(cell,'DD/MM/YYYY HH:mm:ss').format('DD/MM/YYYY HH:mm:ss') + "</div>";
        }
      }
    },
    actions: false,
    noDataMessage: 'No se obtuvo información...',
    attr: {
      class: 'table table-bordered grid'
    },
    hideSubHeader: true
  };
}
