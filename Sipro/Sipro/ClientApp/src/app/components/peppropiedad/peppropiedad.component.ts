import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { MatDialog } from '@angular/material';
import { PepPropiedad } from './model/PepPropiedad';
import { Etiqueta } from '../../../assets/models/Etiqueta';
import { DialogDeleteProyectoPropiedad, DialogOverviewDelete } from './modals/confirmation-delete';

@Component({
  selector: 'app-peppropiedad',
  templateUrl: './peppropiedad.component.html',
  styleUrls: ['./peppropiedad.component.css']
})
export class PeppropiedadComponent implements OnInit {
  isLoggedIn : boolean;
  isMasterPage : boolean;
  esColapsado : boolean;
  elementosPorPagina : number;
  numeroMaximoPaginas : number;
  totalProyectoPropiedades : number;
  proyectopropiedad : PepPropiedad;
  etiqueta : Etiqueta;
  busquedaGlobal: string;
  paginaActual : number;
  source: LocalDataSource;
  esNuevo : boolean;
  datoTipoSelected: number;
  tipodatos = [];
  modalDelete: DialogOverviewDelete;

  @ViewChild('search') divSearch: ElementRef;

  constructor(private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog) { 
    this.etiqueta = JSON.parse(localStorage.getItem("_etiqueta"));
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.totalProyectoPropiedades = 0;
    this.proyectopropiedad = new PepPropiedad();
    this.source = new LocalDataSource();
    this.modalDelete = new DialogOverviewDelete(dialog);
  }

  ngOnInit() {
    this.obtenerTotalProyectoPropiedades();
    this.obtenerDatosTipo();
  }

  obtenerTotalProyectoPropiedades(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      t:moment().unix()
    };

    this.http.post('http://localhost:60067/api/ProyectoPropiedad/NumeroProyectoPropiedades', data, { withCredentials : true }).subscribe(response =>{
      if(response['success'] == true){
        this.totalProyectoPropiedades = response['totalproyectopropiedades'];
        this.paginaActual = 1;
        if(this.totalProyectoPropiedades > 0)
          this.cargarTabla(this.paginaActual);
        else
          this.source = new LocalDataSource();
      }
    })
  }

  cargarTabla(pagina? : any){
    var filtro = {
      pagina: pagina,
      numeroProyectoPropiedad: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      t:moment().unix()
    };

    this.http.post('http://localhost:60067/api/ProyectoPropiedad/ProyectoPropiedadPagina', filtro, { withCredentials : true }).subscribe(response =>{
      if(response['success'] == true){
        var data = response['proyectopropiedades'];
        this.source = new LocalDataSource(data);
        this.source.setSort([
          { field: 'id', direction: 'asc' }  // primary sort
        ]);
        this.busquedaGlobal = null;
      }
    })
  }

  nuevo(){
    this.esColapsado = true;
    this.esNuevo = true;
    this.proyectopropiedad = new PepPropiedad();
    this.datoTipoSelected = 0;
  }

  editar(){
    if(this.proyectopropiedad.id > 0){
      this.esColapsado = true;
      this.esNuevo = false;
      this.datoTipoSelected = this.proyectopropiedad.datoTipoid;
    }
    else{
      alert('warning, Debe seleccionar la Propiedad de ' + this.etiqueta.proyecto+' que desea editar');
    }
  }

  borrar(){
    if(this.proyectopropiedad.id > 0){
      this.modalDelete.dialog.open(DialogDeleteProyectoPropiedad, {
        width: '600px',
        height: '200px',
        data: { 
          id: this.proyectopropiedad.id,
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar la propiedad de ' + this.etiqueta.proyecto + this.proyectopropiedad.nombre + "?",
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result != null){
          this.obtenerTotalProyectoPropiedades();
        }
      })
    }
    else{
      alert('warning, Seleccione una propiedad de ' + this.etiqueta.proyecto);
    }
  }

  refresh(){
    this.busquedaGlobal = null;
    this.divSearch.nativeElement.value = null;
    this.obtenerTotalProyectoPropiedades();
  }

  filtrar(campo){
    this.busquedaGlobal = campo;
    this.obtenerTotalProyectoPropiedades();
  }

  onDblClickRow(event){
    this.proyectopropiedad = event.data;
    this.editar();
  }

  onSelectRow(event){
    this.proyectopropiedad = event.data;
  }

  guardar(){
    if(this.proyectopropiedad != null && Number(this.datoTipoSelected) != 0){
      var objetoHttp;

      if(this.proyectopropiedad.id > 0){
        objetoHttp = this.http.put("http://localhost:60067/api/ProyectoPropiedad/ProyectoPropiedad/" + this.proyectopropiedad.id, this.proyectopropiedad, { withCredentials: true });
      }
      else{
        objetoHttp = this.http.post("http://localhost:60067/api/ProyectoPropiedad/ProyectoPropiedad", this.proyectopropiedad, { withCredentials: true });
      }

      objetoHttp.subscribe(response =>{
        if(response['success'] == true){
          this.proyectopropiedad.usuarioCreo = response['usuarioCreo'];
          this.proyectopropiedad.fechaCreacion = response['fechaCreacion'];
          this.proyectopropiedad.fechaActualizacion = response['fechaActualizacion'];
          this.proyectopropiedad.usuarioActualizo = response['usuarioActualizo'];
          this.proyectopropiedad.id = response['id'];

          this.esNuevo = false;
          this.obtenerTotalProyectoPropiedades();
          alert("warning, Guardado exitosamente");
        }
      })
    }
    else{
      alert("warning, Debe seleccionar un Tipo de dato");
    }
  }

  IrATabla(){
    this.esColapsado = false;
    this.proyectopropiedad = new PepPropiedad();
  }

  obtenerDatosTipo(){
    this.http.get('http://localhost:60017/api/DatoTipo/Listar', { withCredentials : true }).subscribe(response =>{
      if(response['success'] == true){
        this.tipodatos = response['datoTipos']
      }
    })
  }

  cambioOpcionDatoTipo(opcion){
    this.proyectopropiedad.datoTipoid = opcion;
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
      datoTiponombre: {
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
    noDataMessage: 'Cargando, por favor espere...',
    attr: {
      class: 'table table-bordered'
    },
    hideSubHeader: true
  };
}
