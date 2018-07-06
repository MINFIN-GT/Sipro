import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';
import { MatDialog } from '@angular/material';
import * as moment from 'moment';

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

  constructor(private route: ActivatedRoute, private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog) { 
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    
    this.route.params.subscribe(param => {
      this.prestamo = Number(param.id);
    })

    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.etiqueta = new Etiqueta();
    this.etiquetaProyecto = this.etiqueta.proyecto; //Cambiarlo
    this.totalProyectos = 0;
    this.busquedaGlobal = null;
  }

  ngOnInit() {
    this.obtenerPrestamo();
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
      filtro_busqueda: this.busquedaGlobal
    };

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
        title: 'Caracterización ' + this.etiquetaProyecto,
        type: 'html',
        filter: false
      },
      unidadejecutora: {
        title: 'Unidad ejecutora',
        filter: false,
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + cell + "</div>";
        }
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

export class Etiqueta{
  proyecto : string;
  colorPrincipal: string;
}