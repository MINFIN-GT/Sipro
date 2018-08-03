import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import * as moment from 'moment';
import { LocalDataSource } from 'ng2-smart-table';
import { Componente } from './model/Componente';
import { ActivatedRoute } from "@angular/router";
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';
import { Etiqueta } from '../../../assets/models/Etiqueta';
import { DialogMapa, DialogOverviewMapa } from '../../../assets/modals/cargamapa/modal-carga-mapa';

@Component({
  selector: 'app-componente',
  templateUrl: './componente.component.html',
  styleUrls: ['./componente.component.css']
})
export class ComponenteComponent implements OnInit {
  mostrarcargando : boolean;
  color = 'primary';
  mode = 'indeterminate';
  value = 50;
  diameter = 45;
  strokewidth = 3;

  isLoggedIn : boolean;
  isMasterPage : boolean;
  objetoTipoNombre: string;
  proyectoNombre: string;
  esTreeview: boolean;
  esColapsado: boolean;
  congelado: number;  
  source : LocalDataSource;
  totalComponentes: number;
  elementosPorPagina: number;
  numeroMaximoPaginas: number;
  componente: Componente;
  pepid: number;
  busquedaGlobal : string;
  paginaActual : number;
  tabActive: number;
  etiqueta : Etiqueta;
  etiquetaProyecto : string;
  prestamoid: number;
  unidadEjecutoraNombre: string;
  unidadEjecutora: number;
  entidad: number;
  entidadNombre: string;
  ejercicio: number;
  esNuevo: boolean;
  modalMapa: DialogOverviewMapa;
  coordenadas: string;

  @ViewChild('search') divSearch: ElementRef;
  
  constructor(private route: ActivatedRoute, private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog, private router: Router) { 
    this.etiqueta = JSON.parse(localStorage.getItem("_etiqueta"));
    this.etiquetaProyecto = this.etiqueta.proyecto;
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.totalComponentes = 0;

    this.route.params.subscribe(param => {
      this.pepid = Number(param.id);
    })

    this.busquedaGlobal = null;
    this.tabActive = 0;
    this.congelado = 0;
    this.obtenerPep();
    this.componente = new Componente();
    this.modalMapa = new DialogOverviewMapa(dialog);
  }

  ngOnInit() {
    this.mostrarcargando=true;
    this.obtenerTotalComponentes();
  }

  obtenerPep(){
    this.http.get('http://localhost:60064/api/Proyecto/ObtenerProyectoPorId/' + this.pepid, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.proyectoNombre = response['nombre'];
        this.objetoTipoNombre = this.etiquetaProyecto;
        this.congelado = response['congelado'];  
        this.prestamoid = response['prestamoId'];
        this.unidadEjecutoraNombre = response['unidadEjecutoraNombre'];
        this.unidadEjecutora = response['unidadEjecutora'];
        this.entidad = response['entidad'];
        this.entidadNombre = response['entidadNombre'];
        this.ejercicio = response['ejercicio'];
      }
    })
  }

  obtenerTotalComponentes(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      proyectoId: this.pepid,
      t: new Date().getTime()      
    };
    this.http.post('http://localhost:60012/api/Componente/NumeroComponentesPorProyecto', data, { withCredentials : true}).subscribe(response =>{
      if(response['success'] == true){
        this.totalComponentes = response['totalcomponentes'];
        this.paginaActual = 1;
        if(this.totalComponentes > 0){
          this.cargarTabla(this.paginaActual);
        }
        else{
          this.source = new LocalDataSource();
          this.mostrarcargando=false;
        }
      }
    })
  }

  cargarTabla(pagina? : number){
    this.mostrarcargando = true;
    var filtro = {
      proyectoId: this.pepid,
      pagina: pagina,
      numerocomponente: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      t:moment().unix()
    };

    this.http.post('http://localhost:60012/api/Componente/ComponentesPaginaPorProyecto', filtro, { withCredentials : true}).subscribe(response =>{
      if(response['success'] == true){
        var data = response['componentes'];

        this.source = new LocalDataSource(data);
        this.source.setSort([
            { field: 'id', direction: 'asc' }  // primary sort
        ]);
        this.busquedaGlobal = null;
      }

      this.mostrarcargando = false;
    })

  }

  nuevo(){
    this.esColapsado = true;
    this.esNuevo = true;
    this.tabActive = 0;
  }

  editar(){
    if(this.componente.id != null){
      this.esColapsado = true;
      this.esNuevo = false;
      this.tabActive = 0;
    }
    else
      this.utils.mensaje("warning", "Debe de seleccionar el componente que desea editar");
  }

  borrar(){

  }

  filtrar(campo){
    this.busquedaGlobal = campo;
    this.obtenerTotalComponentes();
  }

  onSelectRow(event) {
    this.componente = event.data;
  }

  onDblClickRow(event) {
    this.componente = event.data;
    this.editar();
  }

  handlePage(event){
    this.cargarTabla(event.pageIndex+1);
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
        width: '35%',
        filter: false,       
      },
      descripcion: {
        title: 'Descripción',
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
    noDataMessage: 'No se obtuvo información...',
    attr: {
      class: 'table table-bordered grid estilo-letra'
    },
    hideSubHeader: true
  };

  guardar(){

  }

  IrATabla(){
    this.esColapsado = false;
    this.componente = new Componente();
  }

  buscarUnidadEjecutora(){
    
  }

  abrirMapa(){
    this.modalMapa.dialog.open(DialogMapa, {
      width: '1000px',
      height: '500px',
      data: { titulo: 'Mapa' }
    }).afterClosed().subscribe(result=>{
      if(result != null && result.success){
        this.componente.latitud = result.latitud;
        this.componente.longitud = result.longitud;
        this.coordenadas = result.latitud + ", " + result.longitud;
      }else{
        this.coordenadas = '';
      }
    })
  }
}
