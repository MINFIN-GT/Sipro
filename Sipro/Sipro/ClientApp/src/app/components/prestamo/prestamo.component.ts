import { Component, OnInit } from '@angular/core';
import { utils } from 'protractor';
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { Moment } from 'moment';

@Component({
  selector: 'app-prestamo',
  templateUrl: './prestamo.component.html',
  styleUrls: ['./prestamo.component.css']
})

export class PrestamoComponent implements OnInit {
  isLoggedIn : boolean;
  isMasterPage : boolean;
  esColapsado : boolean;
  totalPrestamos : number;
  paginaActual : number;
  data : string;
  elementosPorPagina : number;
  numeroMaximoPaginas : number;
  prestamo : Prestamo;
  source: LocalDataSource;
  filtros: Filtros;
  esnuevo : boolean;
  etiqueta : Etiqueta;

  constructor(private auth: AuthService, private utils: UtilsService, private http: HttpClient) {
    this.totalPrestamos = 0;
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.filtros = new Filtros();
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.prestamo = new Prestamo();
    this.etiqueta = new Etiqueta();
  }

  ngOnInit() { 
    this.obtenerTotalPrestamos();
  }

  nuevo(){
    this.esColapsado = true;
    this.esnuevo = true;
  }

  editar(){
    if(this.prestamo != null){
      this.esColapsado = true;
      this.esnuevo = false;
    }
    else
      alert('seleccione un item');
  }

  borrar(){

  }

  refresh(){
    this.filtros.filtro_nombre = null;
    this.filtros.filtro_codigo_presupuestario = null;
    this.filtros.filtro_fecha_creacion = null;
    this.filtros.filtro_numero_prestamo = null;
    this.filtros.filtro_usuario_creo = null;
    this.cargarTabla(1);
  }

  IrATabla(){
    this.esColapsado = false;
    this.prestamo = new Prestamo();
  }

  obtenerTotalPrestamos(){
    var data = {  
      filtro_nombre: null, 
      filtro_codigo_presupuestario: null, 
      filtro_numero_prestamo: null, 
      filtro_usuario_creo: null, 
      filtro_fecha_creacion: null
    };

    this.http.post('http://localhost:60054/api/Prestamo/NumeroPrestamos', data, { withCredentials: true }).subscribe(response => {
        if (response['success'] == true) {
          this.totalPrestamos = response["totalprestamos"];
          this.paginaActual = 1;
          this.cargarTabla(1);
        } else {
          console.log('Error');
        }
      });
  }

  cargarTabla(pagina? : number) : boolean{
    var exito = false;
    var filtro = {
      pagina: pagina,
      elementosPorPagina: this.elementosPorPagina,
      filtro_nombre: this.filtros.filtro_nombre, 
      filtro_codigo_presupuestario: null, 
      filtro_numero_prestamo: null, 
      filtro_usuario_creo: null, 
      filtro_fecha_creacion: null,
      columna_ordenada: null,
    };

    this.http.post('http://localhost:60054/api/Prestamo/PrestamosPagina', filtro, { withCredentials: true }).subscribe(response => {
        if (response['success'] == true) {   
          var data = response['prestamos'];
          for(var i = 0; i<data.length; i++){
            data[i].fechaElegibilidadUe = data[i].fechaElegibilidadUe != null ? moment(data[i].fechaElegibilidadUe,'DD/MM/YYYY').toDate() : null;
            data[i].fechaActualizacion = data[i].fechaActualizacion != null ? moment(data[i].fechaActualizacion,'DD/MM/YYYY').toDate() : null;
            data[i].fechaAutorizacion = data[i].fechaAutorizacion != null ? moment(data[i].fechaAutorizacion,'DD/MM/YYYY').toDate() : null;
            data[i].fechaCierreActualUe = data[i].fechaCierreActualUe != null ? moment(data[i].fechaCierreActualUe,'DD/MM/YYYY').toDate() : null;
            data[i].fechaCierreOrigianlUe = data[i].fechaCierreOrigianlUe != null ? moment(data[i].fechaCierreOrigianlUe,'DD/MM/YYYY').toDate() : null;
            data[i].fechaCorte = data[i].fechaCorte != null ? moment(data[i].fechaCorte,'DD/MM/YYYY').toDate() : null;
            data[i].fechaCreacion = data[i].fechaCreacion != null ? moment(data[i].fechaCreacion,'DD/MM/YYYY').toDate() : null;
            data[i].fechaDecreto = data[i].fechaDecreto != null ? moment(data[i].fechaDecreto,'DD/MM/YYYY').toDate() : null;
            data[i].fechaFinEjecucion = data[i].fechaFinEjecucion != null ? moment(data[i].fechaFinEjecucion,'DD/MM/YYYY').toDate() : null;
            data[i].fechaFirma = data[i].fechaFirma != null ? moment(data[i].fechaFirma,'DD/MM/YYYY').toDate() : null;
            data[i].fechaSuscripcion = data[i].fechaSuscripcion != null ? moment(data[i].fechaSuscripcion,'DD/MM/YYYY').toDate() : null;
            data[i].fechaVigencia = data[i].fechaVigencia != null ? moment(data[i].fechaVigencia,'DD/MM/YYYY').toDate() : null;
          }
          this.source = new LocalDataSource(data);
          exito = true;
        } else {
          console.log('Error');
          exito = false;
        }
      });

      return exito;
  }

  onSelectRow(event) {
    this.prestamo = event.data;
  }

  onDblClickRow(event) {
    this.prestamo = event.data;
    this.editar();
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
      proyectoPrograma: {
        title: 'Nombre',
        filterFunction:(cell?: any, search?: string) :boolean => {
          this.filtrar("filtro_nombre", search)
          return true;
        }        
      },
      codigoPresupuestario: {
        title: 'Código presupuestario',
        type: 'html',
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + cell + "</div>";
        }
      },
      numeroPrestamo: {
        title: 'Número de Préstamo'
      },
      usuarioCreo: {
        title: 'Usuario Creación'
      },
      fechaCreacion:{
        title: 'Fecha Creación',
        type: 'html',
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + moment(cell).format('DD/MM/YYYY HH:mm:ss') + "</div>";
        }
      }
    },
    actions: false,
    noDataMessage: 'No se encontró información.'
  };

  handlePage(event){
    this.cargarTabla(event.pageIndex+1);
  }

  filtrar(campo, valor){
    this.filtros[campo] = valor;    
    return this.cargarTabla(1);
  }

  buscarCodigoPresupuestario(){
    alert('hola');
  }
}

export class Filtros {
  filtro_nombre : string;
  filtro_codigo_presupuestario: number;
  filtro_numero_prestamo: string;
  filtro_usuario_creo: string;
  filtro_fecha_creacion: string;
}

export class Prestamo{
  amortizado: number;
  aniosGracia: number;
  aniosPlazo: number;
  codigoPresupuestario: number;
  comisionCompromisoAcumulado: number;
  comisionCompromisoAnio: number;
  cooperanteid: number;
  cooperantenombre: string;
  desembolsadoAFecha: number;
  desembolsoAFechaUe: number;
  desembolsoAFechaUeUsd: number;
  desembolsoAFechaUsd: number;
  desembolsoReal: number;
  destino: string;
  ejecucionEstadoId: number;
  ejecucionEstadoNombre: string;
  ejecucionFisicaRealPEP: number;
  fechaActualizacion: Date;
  fechaAutorizacion: Date;
  fechaCierreActualUe: Date;
  fechaCierreOrigianlUe: Date;
  fechaCorte: Date;
  fechaCreacion: Date;
  fechaDecreto: Date;
  fechaElegibilidadUe: Date;
  fechaFinEjecucion: Date;
  fechaFirma: Date;
  fechaSuscripcion: Date;
  fechaVigencia: Date;
  id : number;
  interesesAcumulados: number;
  interesesAnio: number;
  mesesProrrogaUe: number;
  montoAsignadoUe: number;
  montoAsignadoUeQtz: number;
  montoAsignadoUeUsd: number;
  montoContratado: number;
  montoContratadoEntidadUsd: number;
  montoContratadoQtz: number;
  montoContratadoUsd: number;
  montoPorDesembolsarUe: number;
  montoPorDesembolsarUeUsd: number;
  montoPorDesembolsarUsd: number;
  nombreEntidadEjecutora: string;
  numeroAutorizacion: number;
  numeroPrestamo: string;
  objetivo: string;
  otrosCargosAcumulados: number;
  otrosGastos: number;
  periodoEjecucion: number;
  plazoEjecucionPEP: number;
  plazoEjecucionUe: number;
  porAmortizar: number;
  porcentajeAvance: number;
  porcentajeComisionCompra: number;
  porcentajeInteres: number;
  presupuestoAsignadoFuncionamiento: number;
  presupuestoAsignadoInversion: number;
  presupuestoDevengadoFun: number;
  presupuestoDevengadoInv: number;
  presupuestoModificadoFun:number;
  presupuestoModificadoInv: number;
  presupuestoPagadoFun: number;
  presupuestoPagadoInv: number;
  presupuestoVigenteFun: number;
  presupuestoVigenteInv: number;
  principalAcumulado: number;
  principalAnio: number;
  proyectoPrograma: string;
  saldoCuentas: number;
  sectorEconomico: number;
  tipoAutorizacionId: number;
  tipoAutorizacionNombre: number;
  tipoInteresId: number;
  tipoInteresNombre: string;
  tipoMonedaId: number;
  tipoMonedaNombre: string;
  unidadEjecutora: number;
  unidadEjecutoraNombre: string;
  usuarioActualizo: string;
  usuarioCreo: string;
}

export class Etiqueta{
  proyecto : string;
  colorPrincipal: string;
}