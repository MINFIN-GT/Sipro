import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { MatDialog } from '@angular/material';
import { DialogOverviewCodigoPresupuestario, DialogCodigoPresupuestario } from './modals/modal-codigo-presupuestario'
import { DialogOverviewMoneda, DialogMoneda } from './modals/modal-moneda'
import { DialogOverviewTipoPrestamo, DialogTipoPrestamo } from './modals/modal-tipo-prestamo'


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
  sourceTipoPrestamo: LocalDataSource;
  sourceArchivosAdjuntos: LocalDataSource;
  esnuevo : boolean;
  etiqueta : Etiqueta;
  esNuevoDocumento: boolean;
  busquedaGlobal: string;
  modalCodigoPresupuestario: DialogOverviewCodigoPresupuestario;
  modalMoneda: DialogOverviewMoneda;
  modalTipoPrestamo: DialogOverviewTipoPrestamo;
  tablaTipos = [];

  @ViewChild('search') divSearch: ElementRef;

  constructor(private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog) {
    this.totalPrestamos = 0;
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.prestamo = new Prestamo();
    this.etiqueta = new Etiqueta();
    this.esNuevoDocumento = true;
    this.busquedaGlobal = null;
    this.modalCodigoPresupuestario = new DialogOverviewCodigoPresupuestario(dialog);
    this.modalMoneda = new DialogOverviewMoneda(dialog);
    this.modalTipoPrestamo = new DialogOverviewTipoPrestamo(dialog);
    this.sourceTipoPrestamo = new LocalDataSource();
  }

  ngOnInit() { 
    this.obtenerTotalPrestamos();
  }

  onBusqueda(){
    alert('hola busqueda global');
  }

  nuevo(){
    this.esColapsado = true;
    this.esnuevo = true;
    this.esNuevoDocumento = true;
  }

  editar(){
    if(this.prestamo != null){
      this.esColapsado = true;
      this.esnuevo = false;
      this.esNuevoDocumento = false;
    }
    else
      alert('seleccione un item');
  }

  borrar(){

  }

  refresh(){
    this.obtenerTotalPrestamos();
    this.divSearch.nativeElement.value = null;
  }

  guardar(){

  }

  IrATabla(){
    this.esColapsado = false;
    this.prestamo = new Prestamo();
  }

  obtenerTotalPrestamos(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal
    };

    this.http.post('http://localhost:60054/api/Prestamo/NumeroPrestamos', data, { withCredentials: true }).subscribe(response => {
        if (response['success'] == true) {
          this.totalPrestamos = response["totalprestamos"];
          this.paginaActual = 1;
          this.cargarTabla(this.paginaActual);
        } else {
          console.log('Error');
        }
      });
  }

  cargarTabla(pagina? : number){
    var filtro = {
      pagina: pagina,
      elementosPorPagina: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
    };

    this.http.post('http://localhost:60054/api/Prestamo/PrestamosPagina', filtro, { withCredentials: true }).subscribe(response => {
        if (response['success'] == true) {   
          var data = response['prestamos'];
          for(var i = 0; i<data.length; i++){
            data[i].fechaElegibilidadUe = data[i].fechaElegibilidadUe != null ? moment(data[i].fechaElegibilidadUe,'DD/MM/YYYY').toDate() : null;
            data[i].fechaAutorizacion = data[i].fechaAutorizacion != null ? moment(data[i].fechaAutorizacion,'DD/MM/YYYY').toDate() : null;
            data[i].fechaCierreActualUe = data[i].fechaCierreActualUe != null ? moment(data[i].fechaCierreActualUe,'DD/MM/YYYY').toDate() : null;
            data[i].fechaCierreOrigianlUe = data[i].fechaCierreOrigianlUe != null ? moment(data[i].fechaCierreOrigianlUe,'DD/MM/YYYY').toDate() : null;
            data[i].fechaCorte = data[i].fechaCorte != null ? moment(data[i].fechaCorte,'DD/MM/YYYY').toDate() : null;            
            data[i].fechaDecreto = data[i].fechaDecreto != null ? moment(data[i].fechaDecreto,'DD/MM/YYYY').toDate() : null;
            data[i].fechaFinEjecucion = data[i].fechaFinEjecucion != null ? moment(data[i].fechaFinEjecucion,'DD/MM/YYYY').toDate() : null;
            data[i].fechaFirma = data[i].fechaFirma != null ? moment(data[i].fechaFirma,'DD/MM/YYYY').toDate() : null;
            data[i].fechaSuscripcion = data[i].fechaSuscripcion != null ? moment(data[i].fechaSuscripcion,'DD/MM/YYYY').toDate() : null;
            data[i].fechaVigencia = data[i].fechaVigencia != null ? moment(data[i].fechaVigencia,'DD/MM/YYYY').toDate() : null;
          }
          this.source = new LocalDataSource(data);
          this.busquedaGlobal = null;
        } else {
          console.log('Error');
        }
      });
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
        filter: false,       
      },
      codigoPresupuestario: {
        title: 'Código presupuestario',
        type: 'html',
        filter: false,
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + cell + "</div>";
        }
      },
      numeroPrestamo: {
        title: 'Número de Préstamo',
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
    noDataMessage: 'No se encontró información.',
    attr: {
      class: 'table table-bordered'
    },
    hideSubHeader: true
  };

  handlePage(event){
    this.cargarTabla(event.pageIndex+1);
  }

  filtrar(campo){  
    this.busquedaGlobal = campo;
    this.obtenerTotalPrestamos();
  }

  buscarCodigoPresupuestario(){
    this.modalCodigoPresupuestario.dialog.open(DialogCodigoPresupuestario, {
      width: '600px',
      height: '550px',
      data: { titulo: 'Código Presupuestario' }
    }).afterClosed().subscribe(result => {
      if(result != null){
        this.prestamo.codigoPresupuestario = result.codigoPresupuestario;
        this.prestamo.numeroPrestamo = result.numeroprestamo;
      }
    });
  }

  buscarTipoMoneda(){
    this.modalMoneda.dialog.open(DialogMoneda, {
      width: '600px',
      height: '550px',
      data: { titulo: 'Tipo Moneda' }
    }).afterClosed().subscribe(result => {
      if(result != null){
        this.prestamo.tipoMonedaNombre = result.tipoMonedaNombre;
        this.prestamo.tipoMonedaId = result.tipoMonedaId;
      }
    });
  }

  setPorcentaje(val: number){

  }

  settingsTipoPrestamo = {
    columns: {
      id: {
        title: 'ID',
        width: '5%',
        filter: false,
        type: 'html',
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + cell + "</div>";
        }
      },
      nombre: {
        title: 'Nombre',
        align: 'left',
        width: '90%',
        filter: false,
        class: 'align-left'
      },
      eliminar: {
        title: 'Eliminar',
        width: '5%',
        filter: false,
      }
    },
    actions: false,
    attr: {
      class: 'table table-bordered'
    },
    hideSubHeader: true,
    noDataMessage: ''
  };

  settingsArchivosAdjuntos = {
    columns: {
      id: {
        title: 'ID',
        width: '5%',
        filter: false,
        type: 'html',
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + cell + "</div>";
        }
      },
      nombre: {
        title: 'Nombre',
        width: '42.5%',
        filter: false
      },
      extension: {
        title: 'Extensión',
        width: '42.5%',
        filter: false
      },
      descargar: {
        title: 'Descarga',
        type: 'custom',
        width: '5%',
        filter: false,
        renderComponent: PrestamoComponent,
        onComponentInitFunction(instance) {
          instance.save.subscribe(row => {
            alert(`${row.name} saved!`)
          });
        }
      },
      eliminar: {
        title: 'Eliminar',
        type: 'custom',
        width: '5%',
        filter: false,
        renderComponent: PrestamoComponent,
        onComponentInitFunction(instance) {
          instance.save.subscribe(row => {
            alert(`${row.name} saved!`)
          });
        }
      },
    },
    actions: false,
    attr: {
      class: 'table table-bordered'
    },
    hideSubHeader: true,
    noDataMessage: ''
  };

  buscarTiposPrestamo(){
    this.modalTipoPrestamo.dialog.open(DialogTipoPrestamo, {
      width: '600px',
      height: '550px',
      data: { titulo: 'Tipo Préstamo' }
    }).afterClosed().subscribe(result => {
      if(result != null){
        this.source.getElements().then(result=>{
          this.tablaTipos.push({ id: result.tipoPrestamoId, nombre: result.tipoPrestamoNombre });
        });
        this.tablaTipos.push({ id: result.tipoPrestamoId, nombre: result.tipoPrestamoNombre })
        this.sourceTipoPrestamo = new LocalDataSource(this.tablaTipos);
      }
    });
  }

  borrarTipoPrestamo(){

  }

  adjuntarDocumentos(){

  }
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