import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';
import { MatDialog } from '@angular/material';
import * as moment from 'moment';
import { Etiqueta } from '../../../assets/models/Etiqueta';
import { Proyecto } from './model/model.proyecto'
import { DialogOverviewProyectoTipo, DialogProyectoTipo } from './modals/proyecto-tipo'


@Component({
  selector: 'app-proyecto',
  templateUrl: './proyecto.component.html',
  styleUrls: ['./proyecto.component.css']
})
export class ProyectoComponent implements OnInit {
  isLoggedIn : boolean;
  isMasterPage : boolean;
  esColapsado : boolean;
  elementosPorPagina : number;
  numeroMaximoPaginas : number;  
  prestamo : number;
  prestamoNombre : string;
  objetoTipoNombre : string;
  prestamoid : number;
  codigoPresupuestario : number;
  fechaCierreActualUe : Date;
  etiqueta : Etiqueta;
  etiquetaProyecto : string;
  totalProyectos : number;
  busquedaGlobal : string;
  paginaActual : number;
  mostrarcargando : boolean;
  source : LocalDataSource;
  proyecto : Proyecto;
  esNuevo : boolean;
  esNuevoDocumento: boolean;
  tabActive : number;
  unidadejecutoranombre: string;
  congelado : number;
  modalProyectoTipo: DialogOverviewProyectoTipo;
  proyectotipoid : number;
  proyectotiponombre : string;
  camposdinamicos = [];

  @ViewChild('search') divSearch: ElementRef;

  constructor(private route: ActivatedRoute, private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog) { 
    this.etiqueta = JSON.parse(localStorage.getItem("_etiqueta"));
    this.etiquetaProyecto = this.etiqueta.proyecto;
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    
    this.route.params.subscribe(param => {
      this.prestamo = Number(param.id);
    })

    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.totalProyectos = 0;
    this.busquedaGlobal = null;
    this.proyecto = new Proyecto();
    this.tabActive = 0;
    this.unidadejecutoranombre = "";
    this.congelado = 0;
    this.modalProyectoTipo = new DialogOverviewProyectoTipo(dialog);
    this.proyectotipoid = 0;
    this.proyectotiponombre = "";

    this.obtenerPrestamo();
  }

  ngOnInit() {
    this.obtenerTotalProyectos();
  }

  obtenerPrestamo(){
    this.http.get('http://localhost:60054/api/Prestamo/PrestamoPorId/' + this.prestamo, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.prestamoNombre = response['nombre'];
        this.objetoTipoNombre = "Préstamo";
        this.prestamoid = Number(response['id']);
        this.codigoPresupuestario = Number(response['codigoPresupuestario']);
        this.fechaCierreActualUe = moment(response['fechaCierreActualUe'], 'DD/MM/YYYY').toDate();
      }
    })
  }

  obtenerTotalProyectos(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      prestamoId: this.prestamo,
      t: new Date().getTime()      
    };
    this.http.post('http://localhost:60064/api/Proyecto/NumeroProyectos', data, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.totalProyectos = response['totalproyectos'];
        this.paginaActual = 1;
        if(this.totalProyectos > 0){
          this.cargarTabla(this.paginaActual);
        }
        else{
          this.source = new LocalDataSource();
        }
      }
    })
  }

  cargarTabla(pagina? : number){
    this.mostrarcargando = true;
    var filtro = {
      prestamoId: this.prestamo,
      pagina: pagina,
      numeroproyecto: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      t:moment().unix()
    };

    this.http.post('http://localhost:60064/api/Proyecto/ProyectoPagina', filtro, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        var data = response['proyectos'];

        this.source = new LocalDataSource(data);
            this.source.setSort([
              { field: 'id', direction: 'asc' }  // primary sort
            ]);
            this.busquedaGlobal = null;
      }
    })
  }

  filtrar(campo){  
    this.busquedaGlobal = campo;
    this.obtenerTotalProyectos();
  }

  refresh(){
    this.busquedaGlobal = null;
    this.divSearch.nativeElement.value = null;
    this.obtenerTotalProyectos();
  }

  nuevo(){
    this.esColapsado = true;
    this.tabActive = 0;
    this.esNuevo = true;
  }

  editar(){
    if(this.proyecto.id != null){
      this.esColapsado = true;
      this.esNuevo = false;
      this.esNuevoDocumento = false;
      this.tabActive = 0;
    }
    else
      alert('seleccione un item');
  }

  borrar(){

  }

  guardar(){

  }

  IrATabla(){
    this.esColapsado = false;
    this.proyecto = new Proyecto();
  }

  onSelectRow(event) {
    this.proyecto = event.data;
  }

  onDblClickRow(event) {
    this.proyecto = event.data;
    this.editar();
  }

  buscarProyectoTipo(){
    this.modalProyectoTipo.dialog.open(DialogProyectoTipo, {
      width: '600px',
      height: '585px',
      data: { titulo: 'Proyecto Tipo' }
    }).afterClosed().subscribe(result => {
      if(result != null){
        this.proyectotipoid = result.id;
        this.proyectotiponombre = result.nombre;

        var parametros ={
          idProyecto : this.proyecto.id,
          idProyectoTipo : this.proyectotipoid,
          t: new Date().getTime() 
        }
        this.http.post('http://localhost:60067/api/ProyectoPropiedad/ProyectoPropiedadPorTipo', parametros, { withCredentials: true }).subscribe(response => {
          if (response['success'] == true) {
            this.camposdinamicos = response['proyectopropiedades'];
            for(var i=0; i<this.camposdinamicos.length; i++){
              switch(this.camposdinamicos[i].tipo){
                case "fecha":
                  this.camposdinamicos[i].valor = this.camposdinamicos[i].valor != null ? moment(this.camposdinamicos[i].valor, 'DD/MM/YYYY').toDate() : null;
                break;
                case "entero":
                  this.camposdinamicos[i].valor = this.camposdinamicos[i].valor != null ? Number(this.camposdinamicos[i].valor) : null;
                break;
                case "decimal":
                  this.camposdinamicos[i].valor = this.camposdinamicos[i].valor != null ? Number(this.camposdinamicos[i].valor) : null;
                break;
                case "booleano":
                  this.camposdinamicos[i].valor = this.camposdinamicos[i].valor == 'true' ? true : false;
                break;
              }
            }
          }
        })
      }
    })
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
      proyectotipo: {
        title: 'Caracterización ' + (this.etiquetaProyecto != null ? this.etiquetaProyecto : ''),
        filter: false
      },
      unidadejecutora: {
        title: 'Unidad ejecutora',
        filter: false
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