import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { MatDialog } from '@angular/material';
import { DialogOverviewCodigoPresupuestario, DialogCodigoPresupuestario } from '../../../assets/modals/codigopresupuestario/modal-codigo-presupuestario';
import { DialogOverviewMoneda, DialogMoneda } from '../../../assets/modals/tipomoneda/modal-moneda';
import { DialogOverviewTipoPrestamo, DialogTipoPrestamo } from '../../../assets/modals/tipoprestamo/modal-tipo-prestamo';
import { ButtonDeleteComponent } from '../../../assets/customs/ButtonDeleteComponent';
import { ButtonDownloadComponent } from '../../../assets/customs/ButtonDownloadComponent';
import { DialogDownloadDocument, DialogOverviewDownloadDocument } from '../../../assets/modals/documentosadjuntos/documento-adjunto';
import { DialogDelete, DialogOverviewDelete } from '../../../assets/modals/deleteconfirmation/confirmation-delete';
import { Prestamo } from './model/Prestamo'
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { Router } from '@angular/router';
import { Etiqueta } from '../../../assets/models/Etiqueta';

export interface Cooperante {
  codigo: number;
  descripcion: string;
  estado: number;
  fechaActualizacion : Date;
  fechaCreacion: Date;
  nombre: string;
  siglas: string;
  usuarioActualizo: string;
  usuarioCreo: string;
}

@Component({
  selector: 'app-prestamo',
  templateUrl: './prestamo.component.html',
  styleUrls: ['./prestamo.component.css']
})

export class PrestamoComponent implements OnInit {
  isLoggedIn : boolean;
  isMasterPage : boolean;
  esColapsado : boolean;
  
  mostrarcargando : boolean;
  color = 'primary';
  mode = 'indeterminate';
  value = 50;
  diameter = 45;
  strokewidth = 3;

  totalPrestamos : number;
  paginaActual : number;
  data : string;
  elementosPorPagina : number;
  numeroMaximoPaginas : number;
  prestamo : Prestamo;
  source: LocalDataSource;
  sourceTipoPrestamo: LocalDataSource;
  sourceArchivosAdjuntos: LocalDataSource;
  esNuevo : boolean;
  etiqueta : Etiqueta;
  esNuevoDocumento: boolean;
  busquedaGlobal: string;
  modalCodigoPresupuestario: DialogOverviewCodigoPresupuestario;
  modalMoneda: DialogOverviewMoneda;
  modalTipoPrestamo: DialogOverviewTipoPrestamo;
  modalAdjuntarDocumento: DialogOverviewDownloadDocument;
  modalDelete: DialogOverviewDelete;
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
  toggle = {};
  tabActive : number;
  unidadesEjecutoras = [];
  totalIngresado: number;
  prestamotipos = [];
  esTreeview: boolean;
  botones: boolean;
  cooperantes = [];

  location: Location;

  @ViewChild('search') divSearch: ElementRef;
  myControl = new FormControl();
  filteredCooperantes: Observable<Cooperante[]>;

  constructor(private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog, private router: Router) {
    this.etiqueta = JSON.parse(localStorage.getItem("_etiqueta"));
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;    
    this.totalPrestamos = 0;
    this.prestamo = new Prestamo();
    this.esNuevoDocumento = true;
    this.busquedaGlobal = null;
    this.modalCodigoPresupuestario = new DialogOverviewCodigoPresupuestario(dialog);
    this.modalMoneda = new DialogOverviewMoneda(dialog);
    this.modalTipoPrestamo = new DialogOverviewTipoPrestamo(dialog);
    this.sourceTipoPrestamo = new LocalDataSource();
    this.modalAdjuntarDocumento = new DialogOverviewDownloadDocument(dialog);
    this.modalDelete = new DialogOverviewDelete(dialog);
    this.toggle = {};
    this.matriz_valid = 1;
    this.diferenciaCambios = 0;
    this.tabActive = 0;
    this.totalIngresado  = 0;
    this.esNuevo = true;

    this.filteredCooperantes = this.myControl.valueChanges
      .pipe(
        startWith(''),
        map(value => value ? this._filterCooperante(value) : this.cooperantes.slice())
      );
  }

  private _filterCooperante(value: string): Cooperante[] {
    const filterValue = value.toLowerCase();
    return this.cooperantes.filter(c => c.nombre.toLowerCase().indexOf(filterValue) === 0);
  }

  cooperanteSeleccionado(value){
    this.prestamo.cooperantecodigo = value.codigo;
  }

  ngOnInit() { 
    this.mostrarcargando=true;
    this.obtenerTotalPrestamos();
    this.obtenerCooperantes();
  }  

  nuevo(){
    this.esColapsado = true;
    this.esNuevo = true;
    this.esNuevoDocumento = true;
    this.m_organismosEjecutores = [];
    this.unidadesEjecutoras = [];
    this.m_componentes = [];
    this.componentes = [];
    this.tabActive = 0;
    this.prestamo = new Prestamo();
    this.sourceTipoPrestamo = new LocalDataSource();
    this.sourceArchivosAdjuntos = new LocalDataSource();
    this.matriz_valid = 1;
  }

  editar(){
    if(this.prestamo.id != null){
      this.esColapsado = true;
      this.esNuevo = false;
      this.esNuevoDocumento = false;
      this.tabActive = 0;

      this.http.get('http://localhost:60054/api/Prestamo/Tipos/'+ this.prestamo.id, { withCredentials: true }).subscribe(response => {
        if (response['success'] == true) {
          this.prestamotipos = response['prestamoTipos'];
          this.sourceTipoPrestamo = new LocalDataSource(this.prestamotipos);
        }
      })

      var codigoPresupuestario = this.prestamo.codigoPresupuestario;
      this.http.get("http://localhost:60054/api/Prestamo/ComponentesSigade/" + codigoPresupuestario, { withCredentials: true }).subscribe(response => {
        this.componentes = response['componentes'];
      })

      this.http.post('http://localhost:60054/api/Prestamo/UnidadesEjecutoras',{
        codigoPresupuestario : this.prestamo.codigoPresupuestario,
        prestamoId : this.prestamo.id
      }, { withCredentials: true }).subscribe(response => {
        if(response['success'] == true){
          this.unidadesEjecutoras = response['unidadesEjecutoras'];
          for(var ue=0; ue < this.unidadesEjecutoras.length; ue++){
            this.unidadesEjecutoras[ue].fechaElegibilidad = this.unidadesEjecutoras[ue].fechaElegibilidad != null ? moment(this.unidadesEjecutoras[ue].fechaElegibilidad,'DD/MM/YYYY').toDate() : null;
            this.unidadesEjecutoras[ue].fechaCierre = this.unidadesEjecutoras[ue].fechaCierre != null ? moment(this.unidadesEjecutoras[ue].fechaCierre,'DD/MM/YYYY').toDate() : null;
            this.unidadesEjecutoras[0].esCoordinador=true;
          }      
        }
      })

      this.cargarMatriz();			
			this.getPorcentajes();			
			this.getDocumentosAdjuntos(this.prestamo.id, -1);
    }
    else
    this.utils.mensaje("warning", "Debe de seleccionar el préstamo que desea editar");
  }

  borrar(){
    if(this.prestamo.id > 0){
      this.modalDelete.dialog.open(DialogDelete, {
        width: '600px',
        height: '200px',
        data: { 
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar el préstamo ' + this.prestamo.proyectoPrograma + "?",
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result == true){
          //falta poner la lógica del borrar de préstamo

          this.prestamo.tipoMonedaNombre = result.tipoMonedaNombre;
          this.prestamo.tipoMonedaid = result.tipoMonedaId;
        }
      });
    }
    else{
      this.utils.mensaje("warning", "Debe de seleccionar el préstamo que desea borrar");
    }
  }

  guardar(){
    if (this.prestamo!=null && this.prestamo.codigoPresupuestario !=null){
      this.prestamo.idPrestamoTipos = "";
      this.prestamotipos = [];
      this.sourceTipoPrestamo.getAll().then(value => { 
          value.forEach(element => {
            this.prestamotipos.push(element); 
          });

          for(var x=0; x < this.prestamotipos.length; x++){
            this.prestamo.idPrestamoTipos = this.prestamo.idPrestamoTipos + (x > 0 ? "," : "") + this.prestamotipos[x].id;
          }

          var objetoHttp;

          if(this.prestamo.id > 0){
            objetoHttp = this.http.put("http://localhost:60054/api/Prestamo/Prestamo/" + this.prestamo.id, this.prestamo, { withCredentials: true });
          }
          else{
            this.prestamo.id=0;
            objetoHttp = this.http.post("http://localhost:60054/api/Prestamo/Prestamo", this.prestamo, { withCredentials: true });
          }
    
          objetoHttp.subscribe(response => {
            if(response['success'] == true){
              this.diferenciaCambios = 0;
              this.prestamo.usuarioCreo = response['usuarioCreo'];
              this.prestamo.fechaCreacion = response['fechaCreacion'];
              this.prestamo.fechaActualizacion = response['fechaActualizacion'];
              this.prestamo.usuarioActualizo = response['usuarioActualizo'];
              this.prestamo.id = response['id'];
    
              if(this.esTreeview){
    
              }else{
                this.obtenerTotalPrestamos();
              }
    
              if (this.matriz_valid == 1 && this.totalIngresado  > 0 && this.m_componentes.length > 0 ){
                for(var c=0; c<this.m_componentes.length; c++){
                  this.m_componentes[c].descripcion = this.componentes[c]!=null ? this.componentes[c].descripcion : null;              
                }
    
                var parametros = {
                  estructura: JSON.stringify(this.m_componentes),
                  componentes: JSON.stringify(this.componentes),
                  unidadesEjecutoras: JSON.stringify(this.unidadesEjecutoras),
                  prestamoId: this.prestamo.id,
                  existenDatos: this.m_existenDatos ? true : false,
                  t:moment().unix()
                }
    
                this.http.post('http://localhost:60054/api/Prestamo/GuardarMatriz', parametros, { withCredentials: true }).subscribe(response => {
                  if(response['success'] == true){
                    this.m_existenDatos = true;
    
                    /*
                    if(this.child_metas!=null || this.child_riesgos!=null){
                      if(this.child_metas)
                        this.child_metas.guardar(null, (this.child_riesgos!=null) ?  this.child_riesgos.guardar : null,'Préstamo '+(this.esNuevo ? 'creado' : 'guardado')+' con éxito',
                            'Error al '+(this.esNuevo ? 'creado' : 'guardado')+' el Préstamo');
                      else if(this.child_riesgos)
                        this.child_riesgos.guardar('Préstamo '+(this.esNuevo ? 'creado' : 'guardado')+' con Éxito',
                            'Error al '+(this.esNuevo ? 'creado' : 'guardado')+' el Préstamo');
                    }else{
                      $utilidades.mensaje('success','Préstamo '+(this.esNuevo ? 'creado' : 'guardado')+' con éxito');
                      
                      this.esNuevoDocumento = false;
                    }
                    */
                   this.utils.mensaje("success", "Préstamo guardado con éxito");
                   this.botones=true;
                   this.esNuevo=false;
                   this.obtenerTotalPrestamos();
                   this.tabActive = 0;
                  }else{
                    this.utils.mensaje("danger", "Error al " +(this.esNuevo ? "crear" : "guardar") + " la matriz del préstamo");
                    this.botones=true;
                  }
                })
              }else{
                /*
                if(mi.child_metas!=null)
                  mi.child_metas.guardar(null, null,'Préstamo '+(mi.esNuevo ? 'creado' : 'guardado')+' con éxito',
                      'Error al '+(mi.esNuevo ? 'crear' : 'guardar')+' el préstamo');
                else{
                  $utilidades.mensaje('success','Préstamo '+(mi.esNuevo ? 'creado' : 'guardado')+' con éxito');
                }
                */
               this.botones=true;
               this.utils.mensaje("success", "Préstamo guardado con éxito");
              }
              this.esNuevo = false;
            }else{
              this.utils.mensaje("danger", "Error al " +(this.esNuevo ? "crear" : "guardar") + " el préstamo");
              this.botones=true;
            }
          });
      });
    }
    else
      this.utils.mensaje("warning", "Debe de llenar todos los campos obligatorios");
  }

  IrATabla(){
    this.esColapsado = false;
    this.prestamo = new Prestamo();
  }

  obtenerTotalPrestamos(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      t:moment().unix()
    };

    this.http.post('http://localhost:60054/api/Prestamo/NumeroPrestamos', data, { withCredentials: true }).subscribe(response => {
        if (response['success'] == true) {
          this.totalPrestamos = response["totalprestamos"];
          this.paginaActual = 1;
          if(this.totalPrestamos > 0){
            this.cargarTabla(this.paginaActual);
          }
          else{
            this.source = new LocalDataSource();
            this.mostrarcargando = false;
          }
        } else {
          this.mostrarcargando = false;
        }
      });
  }

  obtenerCooperantes(){
    this.http.get('http://localhost:60015/api/Cooperante/Cooperantes', { withCredentials: true}).subscribe(response => {
      if (response['success'] == true) {
        this.cooperantes = response["cooperantes"];        
      }
    })
  }

  cargarTabla(pagina? : number){
    this.mostrarcargando = true;
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
        }

        this.mostrarcargando = false;
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
        width: '40%',
        filter: false  
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
    noDataMessage: 'No se obtuvo información...',
    attr: {
      class: 'table table-bordered grid'
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
      height: '585px',
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
        this.cooperanteid = this.prestamo.cooperantecodigo;
        this.getPorcentajes();
      }else{
        this.utils.mensaje("warning", "No se encontraron datos con los parámetros ingresados");
      }   
  });
  }

  cargarMatriz(){
    this.matriz_valid = 0;
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
        this.actualizarComponentes();
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

  buscarTipoMoneda(){
    this.modalMoneda.dialog.open(DialogMoneda, {
      width: '600px',
      height: '585px',
      data: { titulo: 'Tipo Moneda' }
    }).afterClosed().subscribe(result => {
      if(result != null){
        this.prestamo.tipoMonedaNombre = result.tipoMonedaNombre;
        this.prestamo.tipoMonedaid = result.tipoMonedaId;
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
      class: 'table table-bordered grid estilo-letra'
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
            window.location.href='http://localhost:60021/api/DocumentoAdjunto/Descarga/' + row.id;
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
            this.http.delete('http://localhost:60021/api/DocumentoAdjunto/Documento/' + row.id, { withCredentials: true }).subscribe(response => {
              if (response['success'] == true){
                this.sourceArchivosAdjuntos.remove(row);
              }
              else{
                this.utils.mensaje("danger", "Error al borrar el documento");
              } 
            })
            
          });
        }
      }
    },
    actions: false,
    attr: {
      class: 'table table-bordered grid estilo-letra'
    },
    hideSubHeader: true,
    noDataMessage: ''
  };

  buscarTiposPrestamo(){
    this.modalTipoPrestamo.dialog.open(DialogTipoPrestamo, {
      width: '600px',
      height: '585px',
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
      data: { titulo: 'Documentos Adjuntos', idObjeto: this.prestamo.id, idTipoObjeto: -1 }
    }).afterClosed().subscribe(result => {
      if(result != null){
        this.sourceArchivosAdjuntos = new LocalDataSource(result);
      }
    });
  }

  getDocumentosAdjuntos = function(objetoId, tipoObjetoId){
    var formatData = {
      idObjeto: objetoId,
      idTipoObjeto: tipoObjetoId,
      t: new Date().getTime()
    }
    
    this.http.post('http://localhost:60021/api/DocumentoAdjunto/Documentos', formatData, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.sourceArchivosAdjuntos = new LocalDataSource(response['documentos']);
      }
    })
  }

  expandRowComponentes(row, index){
    this.componentes[index].mostrar = !this.componentes[index].mostrar;
  }

  verHistoria(){

  }

  congelarDescongelar(){

  }

  irAPeps(prestamoid){
    if(this.prestamo!=null){
      this.router.navigateByUrl('/main/pep/'+ prestamoid);
    }
  }

  customTrackBy(index: number, obj: any): any {
    return index;
  }
}