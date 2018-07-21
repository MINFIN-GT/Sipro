import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';
import { MatDialog } from '@angular/material';
import * as moment from 'moment';
import { Etiqueta } from '../../../assets/models/Etiqueta';
import { Proyecto } from './model/Proyecto';
import { DialogOverviewProyectoTipo, DialogProyectoTipo } from '../../../assets/modals/peptipo/proyecto-tipo';
import { DialogOverviewUnidadEjecutora, DialogUnidadEjecutora } from '../../../assets/modals/unidadejecutora/unidad-ejecutora';
import { ButtonDeleteComponent } from '../../../assets/customs/ButtonDeleteComponent';
import { ButtonDownloadComponent } from '../../../assets/customs/ButtonDownloadComponent';
import { DialogDownloadDocument, DialogOverviewDownloadDocument } from '../../../assets/modals/documentosadjuntos/documento-adjunto';
import { DialogDirectorProyecto, DialogOverviewDirectorProyecto } from '../../../assets/modals/directorproyecto/director-proyecto';
import { DialogImpacto, DialogOverviewImpacto } from '../../../assets/modals/impacto/modal-impacto';

@Component({
  selector: 'app-pep',
  templateUrl: './pep.component.html',
  styleUrls: ['./pep.component.css']
})
export class PepComponent implements OnInit {
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
  source : LocalDataSource;
  proyecto : Proyecto;
  esNuevo : boolean;
  esNuevoDocumento: boolean;
  tabActive : number;
  unidadejecutoranombre : string;
  unidadejecutoraid : number;
  congelado : number;
  modalProyectoTipo: DialogOverviewProyectoTipo;
  proyectotipoid : number;
  proyectotiponombre : string;
  camposdinamicos = [];
  amount:number = 0.0;
  formattedAmount: string = '';
  modalUnidadEjecutora: DialogOverviewUnidadEjecutora;
  montoTechos : number;
  fechaInicioTemp : string;
  fechaFinalTemp : string;
  fechaInicioRealTemp : string;
  fechaFinalRealTemp : string;
  duracionReal : number;
  m_organismosEjecutores = [];
  m_componentes = [];
  m_existenDatos: boolean;
  sourceArchivosAdjuntos: LocalDataSource;
  modalAdjuntarDocumento: DialogOverviewDownloadDocument;
  sobrepaso: boolean;
  desembolsoAFechaUsd: number;
  montoDesembolsadoUE: number;
  botones: boolean;
  directorProyectoNombre: string;
  directorProyectoId: number;
  modalDirectorProyecto : DialogOverviewDirectorProyecto;
  sourceImpacto: LocalDataSource;
  modalImpacto : DialogOverviewImpacto;

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
    this.modalUnidadEjecutora = new DialogOverviewUnidadEjecutora(dialog);
    this.modalAdjuntarDocumento = new DialogOverviewDownloadDocument(dialog);
    this.modalDirectorProyecto = new DialogOverviewDirectorProyecto(dialog);
    this.modalImpacto = new DialogOverviewImpacto(dialog);
    this.botones = true;
    this.sourceImpacto = new LocalDataSource();
    this.obtenerPrestamo();
  }

  ngOnInit() {
    this.mostrarcargando=true;
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
          this.mostrarcargando=false;
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

      this.mostrarcargando = false;
    })
  }

  filtrar(campo){  
    this.busquedaGlobal = campo;
    this.obtenerTotalProyectos();
  }

  nuevo(){
    this.esColapsado = true;
    this.tabActive = 0;
    this.esNuevo = true;
    this.unidadejecutoranombre = "";
    this.unidadejecutoraid = 0;
    this.sourceArchivosAdjuntos = new LocalDataSource();
  }

  editar(){
    if(this.proyecto.id != null){
      this.esColapsado = true;
      this.esNuevo = false;
      this.esNuevoDocumento = false;
      this.tabActive = 0;
      this.proyectotipoid = this.proyecto.proyectotipoid;
      this.proyectotiponombre = this.proyecto.proyectotipo;
      this.unidadejecutoranombre = this.proyecto.unidadejecutora;
      this.unidadejecutoraid = this.proyecto.unidadejecutoraid;
      this.directorProyectoNombre = this.proyecto.directorProyectoNmbre;

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

      if(this.prestamoid > 0){
        this.http.get('http://localhost:60064/api/Proyecto/MontoTechos/' + this.proyecto.id, { withCredentials : true }).subscribe(response =>{
          if(response['success'] == true){
            this.montoTechos = response['techoPep'];

            if(this.proyecto.costo > this.montoTechos)
              this.sobrepaso = true;
            else
              this.sobrepaso = false;

            var data = {
              codPrep: this.codigoPresupuestario,
              ejercicio: this.proyecto.ejercicio,
              entidad: this.proyecto.entidadentidad,
              ue: this.proyecto.unidadejecutoraid
            }
            this.http.post('http://localhost:60016/api/DataSigade/MontoDesembolsoUE', data, { withCredentials : true }).subscribe(response =>{
              if(response['success']==true){
                this.montoDesembolsadoUE = response['montoDesembolsadoUE'];
                this.montoDesembolsadoUE = this.montoTechos - this.montoDesembolsadoUE;

                this.http.post('http://localhost:60016/api/DataSigade/MontoDesembolsosUEALaFecha', data, { withCredentials : true }).subscribe(response =>{
                  if(response['success']==true){
                    this.desembolsoAFechaUsd = response['montoDesembolsadoUEALaFecha'];
                  }
                })
              }
            })
          }
        })
      }

      if(this.fechaInicioTemp == null){
        this.fechaInicioTemp = moment(this.proyecto.fechaInicio,'DD/MM/YYYY').format('DD/MM/YYYY');
        this.fechaFinalTemp = moment(this.proyecto.fechaFin,'DD/MM/YYYY').format('DD/MM/YYYY');
      }

      if(this.fechaInicioRealTemp == null){
        this.fechaInicioRealTemp = this.proyecto.fechaInicioReal != null ? moment(this.proyecto.fechaInicioReal,'DD/MM/YYYY').format('DD/MM/YYYY') : null;
        this.fechaFinalRealTemp = this.proyecto.fechaFinReal != null ? moment(this.proyecto.fechaFinReal,'DD/MM/YYYY').format('DD/MM/YYYY') : null;

        if(this.fechaInicioRealTemp != null && this.fechaFinalRealTemp != null){
          this.duracionReal = Number(moment(this.fechaFinalRealTemp,'DD/MM/YYYY').toDate()) - Number(moment(this.fechaInicioRealTemp,'DD/MM/YYYY').toDate());
          this.duracionReal = Number(this.duracionReal / (1000*60*60*24))+1;
        }
      }
      else{
        if(this.fechaInicioRealTemp != null && this.fechaFinalRealTemp != null){
          this.duracionReal = Number(moment(this.fechaFinalRealTemp,'DD/MM/YYYY').toDate()) - Number(moment(this.fechaInicioRealTemp,'DD/MM/YYYY').toDate());
          this.duracionReal = Number(this.duracionReal / (1000*60*60*24))+1;
        }
      }

      this.http.get('http://localhost:60064/api/Proyecto/Matriz/' + this.proyecto.id, { withCredentials : true }).subscribe(response =>{
        if(response['success'] == true){
          this.m_organismosEjecutores = response['unidadesEjecutoras'];
          this.m_componentes = response['componentes'];
          this.m_existenDatos = response['existenDatos'];
        }
      })

      this.getDocumentosAdjuntos(this.proyecto.id, 0);
      this.tabActive = 0;
    }
    else
      this.utils.mensaje("warning", "Debe de seleccionar el " + this.etiqueta.proyecto + " que desea editar");
  }

  borrar(){

  }

  guardar(){
    for(var i =0; i < this.camposdinamicos.length; i++){
      this.botones = false;
      if(this.camposdinamicos[i].tipo === 'fecha'){
        this.camposdinamicos[i].valor_f = this.camposdinamicos[i].valor != null ? moment(this.camposdinamicos[i].valor).format('DD/MM/YYYY') : "";        
      }


    }
  }

  IrATabla(){
    this.esColapsado = false;
    this.proyecto = new Proyecto();
    this.proyectotipoid = 0;
    this.proyectotiponombre = "";
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

  buscarUnidadEjecutora(){
    //if(this.prestamoid == null){
      this.modalUnidadEjecutora.dialog.open(DialogUnidadEjecutora, {
        width: '600px',
        height: '585px',
        data: { titulo: 'Unidades Ejecutoras', ejercicio: this.proyecto.ejercicio, entidad: this.proyecto.entidadentidad }
      }).afterClosed().subscribe(result => {
        if(result != null){
          this.unidadejecutoraid = result.id;
          this.unidadejecutoranombre = result.nombre;
        }
      })
    //}
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
    noDataMessage: 'No se obtuvo información...',
    attr: {
      class: 'table table-bordered grid estilo-letra'
    },
    hideSubHeader: true
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

  adjuntarDocumentos(){
    this.modalAdjuntarDocumento.dialog.open(DialogDownloadDocument, {
      width: '600px',
      height: '200px',
      data: { titulo: 'Documentos Adjuntos', idObjeto: this.proyecto.id, idTipoObjeto: 0 }
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

  customTrackBy(index: number, obj: any): any {
    return index;
  }

  buscarDirecotorProyecto(){
    this.modalDirectorProyecto.dialog.open(DialogDirectorProyecto, {
      width: '600px',
      height: '585px',
      data: { titulo: 'Director del ' + this.etiqueta.proyecto }
    }).afterClosed().subscribe(result=>{
      if(result != null){
        this.directorProyectoNombre = result.nombre;
        this.directorProyectoId = result.id;
      }
    })
  }

  agregarImpacto(){
    this.modalImpacto.dialog.open(DialogImpacto, {
      width: '600px',
      height: '400px',
      data: { titulo: 'Impacto' }
    }).afterClosed().subscribe(result =>{
      if(result != null){
         
      }
    })
  }

  settingsImpacto = {
    columns: {
      id: {
        title: 'ID',
        width: '10%',
        filter: false,
        type: 'html',
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + cell + "</div>";
        }
      },
      nombre: {
        title: 'Organización',
        filter: false
      },
      impacto: {
        title: 'Impacto y participación de la organización',
        filter: false
      },
      eliminar:{
        title: 'Eliminar',
        width: '10%',
        sort: false,
        type: 'custom',
        class: 'align-center',
        renderComponent: ButtonDeleteComponent,
        onComponentInitFunction: (instance) =>{
          instance.actionEmitter.subscribe(row => {
            this.sourceImpacto.remove(row);            
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
  }
}