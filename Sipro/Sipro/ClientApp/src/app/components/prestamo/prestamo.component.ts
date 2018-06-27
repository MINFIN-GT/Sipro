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
import { ButtonDeleteComponent } from '../../../assets/ts/ButtonDeleteComponent';
import { ButtonDownloadComponent } from '../../../assets/ts/ButtonDownloadComponent';
import { DialogDownloadDocument, DialogOverviewDownloadDocument } from '../../../assets/ts/documentosadjuntos/documento-adjunto'
import { Prestamo } from './model/model.prestamo'

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
  modalAdjuntarDocumento: DialogOverviewDownloadDocument;
  cooperanteid : number;
  matriz_valid : number;
  diferenciaCambios : number;
  m_organismosEjecutores = [];
  m_componentes = [];
  m_existenDatos: boolean;
  metasCargadas = false;
  riesgosCargados = false;
  activeTab : number;
  componentes = [];
  rowCollectionComponentes = [];
  toggle = {};
  tabActive : number;
  unidadesEjecutoras = [];
  totalIngresado : number;

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
    this.modalAdjuntarDocumento = new DialogOverviewDownloadDocument(dialog);
    this.toggle = {};
    this.matriz_valid = 1;
    this.diferenciaCambios = 0;
    this.tabActive = 0;
    this.totalIngresado  = 0;
  }

  ngOnInit() { 
    this.obtenerTotalPrestamos();
  }

  nuevo(){
    this.esColapsado = true;
    this.esnuevo = true;
    this.esNuevoDocumento = true;
    this.m_organismosEjecutores = [];
    this.m_componentes = [];
    this.componentes = [];
    this.tabActive = 0;
  }

  editar(){
    if(this.prestamo.id != null){
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
      t:moment().unix()
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
          this.source.setSort([
            { field: 'id', direction: 'asc' }  // primary sort
          ]);
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
      height: '520px',
      data: { titulo: 'Código Presupuestario' }
    }).afterClosed().subscribe(result => {
      if(result != null){
        this.prestamo.codigoPresupuestario = result.codigoPresupuestario;
        this.prestamo.numeroPrestamo = result.numeroprestamo;
        this.cargaSigade();
        this.cargarMatriz();
        var codigoPresupuestario = this.prestamo.codigoPresupuestario;
        this.http.get('http://localhost:60054/api/Prestamo/ComponentesSigade/'+ codigoPresupuestario, { withCredentials: true }).subscribe(response => {
          if(response['success'] == true){
            this.componentes = response['componentes'];
          }
        })

        this.http.post('http://localhost:60054/api/Prestamo/UnidadesEjecutoras',{
          codigoPresupuestario : this.prestamo.codigoPresupuestario,
          proyectoId : null
        }, { withCredentials: true }).subscribe(response => {
            if(response['success'] == true){
              this.unidadesEjecutoras = response['unidadesEjecutoras'];
              if(this.unidadesEjecutoras.length>0){
                this.unidadesEjecutoras[0].esCoordinador=true;
            }
          }
        })
      }
    });
  }

  cargaSigade(){
    var codigoPresupuestario = this.prestamo.codigoPresupuestario;

    this.http.get('http://localhost:60016/api/DataSigade/Datos/' + codigoPresupuestario, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {   
        this.prestamo= new Prestamo();  
        this.prestamo = response['prestamo'];
        this.prestamo.fechaDecreto = moment(this.prestamo.fechaDecreto,'DD/MM/YYYY').toDate();
        this.prestamo.fechaSuscripcion = moment(this.prestamo.fechaSuscripcion,'DD/MM/YYYY').toDate();
        this.prestamo.fechaVigencia = moment(this.prestamo.fechaVigencia,'DD/MM/YYYY').toDate();
        this.prestamo.nombre = this.prestamo.nombre == null || this.prestamo.nombre == undefined || this.prestamo.nombre == '' ?
          this.prestamo.proyectoPrograma : this.prestamo.nombre;
        this.cooperanteid = this.prestamo.cooperanteid;
        this.getPorcentajes();
      }else{
        alert('Warning, No se encontraron datos con los parámetros ingresados');
      }   
  });
  }

  cargarMatriz(){
    this.matriz_valid = null;
	  this.diferenciaCambios = 0;
			var parametros = {
					prestamoId: this.prestamo.id,
					codigoPresupuestario: this.prestamo.codigoPresupuestario,
				    t:moment().unix()
			};

    this.http.post('http://localhost:60054/api/Prestamo/ObtenerMatriz', parametros, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.m_organismosEjecutores = response["unidadesEjecutoras"];
        
        this.m_componentes = response["componentes"];

        for(var x in this.m_componentes){
          this.m_componentes[x].totalIngresado = 0;
        }

        this.m_existenDatos = response["existenDatos"];
        this.metasCargadas = false;
        this.riesgosCargados = false;
        this.activeTab = 0;
        this.diferenciaCambios = response["diferencia"];
        this.actualizarTotalesUE();
      }else{
        alert('Warning, No se encontraron datos con los parámetros ingresados')
      }
    })
  }

  actualizarTotalesUE(){
    var totalPrestamo = 0;
		var totalDonacion = 0;
		var totalNacional = 0;
    
    for(var x in this.m_organismosEjecutores){
      this.m_organismosEjecutores[x].totalAsignadoPrestamo = 0;
      this.m_organismosEjecutores[x].totalAsignadoDonacion = 0;
      this.m_organismosEjecutores[x].totalAsignadoNacional = 0;
    }

    for(var c = 0; c < this.m_componentes.length; c++){
      for(var ue = 0; ue < this.m_componentes[c].unidadesEjecutoras.length; ue++){
        this.m_organismosEjecutores[ue].totalAsignadoPrestamo += this.m_componentes[c].unidadesEjecutoras[ue].prestamo;
        this.m_organismosEjecutores[ue].totalAsignadoDonacion += this.m_componentes[c].unidadesEjecutoras[ue].donacion;
        this.m_organismosEjecutores[ue].totalAsignadoNacional += this.m_componentes[c].unidadesEjecutoras[ue].nacional;
      }
    }
  }

  actualizarComponentes(){
    this.matriz_valid = 1;
		this.totalIngresado  = 0;
		     for (var x in this.m_componentes){
		    	 this.m_componentes[x].totalAsignadoPrestamo = 0;
		    	 var  totalUnidades = 0;
		    	 var totalAsignado = 0;
		    	 for (var j in this.m_componentes[x].unidadesEjecutoras){
		    		 totalUnidades = totalUnidades +  this.m_componentes[x].unidadesEjecutoras[j].prestamo;
		    	 }
		    	 totalAsignado = totalUnidades;
		    	 this.totalIngresado = this.totalIngresado + totalUnidades;
		    	 this.matriz_valid = this.matriz_valid==1 &&  totalUnidades == this.m_componentes[x].techo ? 1 : null;
		    	 
		    	 this.m_componentes[x].totalIngresado = totalAsignado;
		     }
  }

  cambiarCoordinador(pos){
    for (var x in this.unidadesEjecutoras){
      this.unidadesEjecutoras[x].esCoordinador = false;
    }
    
    this.unidadesEjecutoras[pos].esCoordinador = true;
  }

  componenteSeleccionado(row){
    var rowAnterior
    if(rowAnterior){
      if(row != rowAnterior){
        rowAnterior.isSelected=false;
      }else{
        return;
      }
    }
    row.isSelected = true;
    rowAnterior = row;
  }

  buscarTipoMoneda(){
    this.modalMoneda.dialog.open(DialogMoneda, {
      width: '600px',
      height: '520px',
      data: { titulo: 'Tipo Moneda' }
    }).afterClosed().subscribe(result => {
      if(result != null){
        this.prestamo.tipoMonedaNombre = result.tipoMonedaNombre;
        this.prestamo.tipoMonedaId = result.tipoMonedaId;
      }
    });
  }

  setPorcentaje(tipo: number){
    var n = 0;
		if (tipo==1)
		{
			if(this.prestamo.desembolsoAFechaUsd !== undefined && this.prestamo.montoContratado !== undefined){
				n = (this.prestamo.desembolsoAFechaUsd / this.prestamo.montoContratado) * 100;
				this.prestamo.desembolsoAFechaUsdP = Number(n.toFixed(2));
				this.prestamo.montoPorDesembolsarUsd= ((1 - (this.prestamo.desembolsoAFechaUsdP/100) ) *  this.prestamo.montoContratado);
				this.prestamo.montoPorDesembolsarUsd= Number(this.prestamo.montoPorDesembolsarUsd.toFixed(2));
				this.prestamo.montoPorDesembolsarUsdP= 100 - this.prestamo.desembolsoAFechaUsdP;
				if(isNaN(this.prestamo.montoPorDesembolsarUsdP))
					this.prestamo.montoPorDesembolsarUsdP = null;
			}
		}else if (tipo==2){
			if(this.prestamo.montoContratadoUsd !== undefined && this.prestamo.montoPorDesembolsarUsd !== undefined){
				n = (this.prestamo.montoPorDesembolsarUsd / this.prestamo.montoContratadoUsd) * 100;
				this.prestamo.montoPorDesembolsarUsdP = Number(n.toFixed(2));
				if(isNaN(this.prestamo.montoPorDesembolsarUsdP))
					this.prestamo.montoPorDesembolsarUsdP = null;
			}
		}else if (tipo==3){
			if(this.prestamo.desembolsoAFechaUeUsd !== undefined && this.prestamo.montoAsignadoUe !== undefined){
				n = (this.prestamo.desembolsoAFechaUeUsd / this.prestamo.montoAsignadoUeUsd) * 100;
				this.prestamo.desembolsoAFechaUeUsdP = Number(n.toFixed(2));
				this.prestamo.montoPorDesembolsarUeUsd = ((1.00 - (this.prestamo.desembolsoAFechaUeUsdP/100.00) ) *  (this.prestamo.montoAsignadoUeUsd*1.00));
				this.prestamo.montoPorDesembolsarUeUsdP= 100.00 - this.prestamo.desembolsoAFechaUeUsdP;
				if(isNaN(this.prestamo.desembolsoAFechaUeUsdP))
					this.prestamo.desembolsoAFechaUeUsdP = null;
				if(isNaN(this.prestamo.montoPorDesembolsarUeUsd))
					this.prestamo.montoPorDesembolsarUeUsd = null;
				if(isNaN(this.prestamo.montoPorDesembolsarUeUsdP))
					this.prestamo.montoPorDesembolsarUeUsdP = null;
			}
		}else if(tipo==4){
			if(this.prestamo.montoAsignadoUeUsd !== undefined && this.prestamo.montoPorDesembolsarUeUsd !== undefined){
				n = (this.prestamo.montoPorDesembolsarUeUsd / this.prestamo.montoAsignadoUeUsd) * 100;
				this.prestamo.montoPorDesembolsarUeUsdP = Number(n.toFixed(2));
				if(isNaN(this.prestamo.montoPorDesembolsarUeUsdP))
					this.prestamo.montoPorDesembolsarUeUsdP = null;
			}
		}else if(tipo==5){
			if(this.prestamo.fechaCierreActualUe !== undefined && this.prestamo.fechaElegibilidadUe !== undefined){
				var fechaInicio = moment(this.prestamo.fechaElegibilidadUe).format('DD/MM/YYYY').split('/');
				var fechaFinal = moment(this.prestamo.fechaCierreActualUe).format('DD/MM/YYYY').split('/');
				var ffechaInicio = Date.UTC(Number(fechaInicio[2]),Number(fechaInicio[1])-1,Number(fechaInicio[0]));
				var ffechaFinal = Date.UTC(Number(fechaFinal[2]),Number(fechaFinal[1])-1,Number(fechaFinal[0]));
				
				var hoy = new Date();
				var fechaActual = moment(hoy).format('DD/MM/YYYY').split('/');
				var ffechaActual = Date.UTC(Number(fechaActual[2]),Number(fechaActual[1])-1,Number(fechaActual[0]));
				
				var dif1 = ffechaFinal - ffechaInicio;
				var dif2 = ffechaActual - ffechaInicio;
				n = (dif1>0) ? (dif2 / dif1) * 100.00 : 0.00;
				if (isNaN(n) || n < 0)
					n = 0.00;
        this.prestamo.plazoEjecucionUe = Number(n.toFixed(2));
        var diferencia = moment(this.prestamo.fechaCierreActualUe).diff(this.prestamo.fechaCierreOrigianlUe,'months',true);
				this.prestamo.mesesProrrogaUe = diferencia < 0 ? Number(0) : Number(diferencia.toFixed(2));
				if(isNaN(this.prestamo.plazoEjecucionUe))
					this.prestamo.plazoEjecucionUe = null;
				if(isNaN(this.prestamo.mesesProrrogaUe))
					this.prestamo.mesesProrrogaUe = null;
			}
			
		}
  }

  getPorcentajes(){
    this.setPorcentaje(1);
    this.setPorcentaje(2);
    this.setPorcentaje(3);
    this.setPorcentaje(4);
    this.setPorcentaje(5);
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
      eliminar:{
        title: 'Eliminar',
        sort: false,
        type: 'custom',
        renderComponent: ButtonDeleteComponent,
        onComponentInitFunction: (instance) =>{
          instance.actionEmitter.subscribe(row => {
            this.sourceTipoPrestamo.remove(row);
          });
        }
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
        title: 'Descargar',
        sort: false,
        type: 'custom',
        renderComponent: ButtonDownloadComponent,
        onComponentInitFunction: (instance) =>{
          instance.actionEmitter.subscribe(row => {
            
          });
        }
      },
      eliminar:{
        title: 'Eliminar',
        sort: false,
        type: 'custom',
        renderComponent: ButtonDeleteComponent,
        onComponentInitFunction: (instance) =>{
          instance.actionEmitter.subscribe(row => {
            this.sourceArchivosAdjuntos.remove(row);
          });
        }
      }
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
      height: '520px',
      data: { titulo: 'Tipo Préstamo' }
    }).afterClosed().subscribe(result => {      
      if(result != null){
        let tablaTipos = [];
        this.sourceTipoPrestamo.getAll().then(value =>{
          value.forEach(element =>{
            tablaTipos.push(element);
          })

          let existe = false;
          if(tablaTipos.length==0)
            tablaTipos.push({ id: result.tipoPrestamoId, nombre: result.tipoPrestamoNombre });
          else{
            tablaTipos.forEach(element => {
              if(element.id==result.tipoPrestamoId)
                existe = true;              
            });

            if(!existe)
              tablaTipos.push({ id: result.tipoPrestamoId, nombre: result.tipoPrestamoNombre });
          }  
        })
              
        this.sourceTipoPrestamo = new LocalDataSource(tablaTipos);
        this.sourceTipoPrestamo.setSort([
          { field: 'id', direction: 'asc' }  // primary sort
        ]);
      }
    });
  }

  adjuntarDocumentos(){
    this.modalAdjuntarDocumento.dialog.open(DialogDownloadDocument, {
      width: '600px',
      height: '200px',
      data: { titulo: 'Documentos Adjuntos' }
    }).afterClosed().subscribe(result => {
      if(result != null){
        
      }
    });
  }

  expandRowComponentes(row, index){
    this.componentes[index].mostrar = !this.componentes[index].mostrar;
  }
}

export class Etiqueta{
  proyecto : string;
  colorPrincipal: string;
}