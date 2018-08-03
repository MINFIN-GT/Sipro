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
import { DialogColaborador, DialogOverviewColaborador } from '../../../assets/modals/colaborador/modal-colaborador';
import { DialogImpacto, DialogOverviewImpacto } from '../../../assets/modals/impacto/modal-impacto';
import { Router } from '@angular/router';
import { DialogCargarProject, DialogOverviewCargarProject } from '../../../assets/modals/cargarproject/modal-cargar-project';

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
  modalDirectorProyecto : DialogOverviewColaborador;
  sourceImpacto: LocalDataSource;
  modalImpacto : DialogOverviewImpacto;
  sourceMiembro: LocalDataSource;
  esTreeview: boolean;
  entidadnombre: string;
  modalCargarProject : DialogOverviewCargarProject;

  @ViewChild('search') divSearch: ElementRef;

  constructor(private route: ActivatedRoute, private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog, private router: Router) { 
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
    this.modalDirectorProyecto = new DialogOverviewColaborador(dialog);
    this.modalImpacto = new DialogOverviewImpacto(dialog);
    this.modalCargarProject = new DialogOverviewCargarProject(dialog);
    this.botones = true;
    this.sourceImpacto = new LocalDataSource();
    this.sourceMiembro = new LocalDataSource();
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
    this.fechaInicioTemp = "";
    this.fechaFinalTemp = "";
    this.fechaInicioRealTemp = "";
    this.fechaFinalRealTemp = "";
    this.duracionReal = 0;
    this.directorProyectoNombre = "";
    this.directorProyectoId = 0;
  }

  editar(){
    if(this.proyecto.id != null){
      this.esColapsado = true;
      this.esNuevo = false;
      this.esNuevoDocumento = false;
      this.tabActive = 0;
      this.proyectotipoid = this.proyecto.proyectoTipoid;
      this.proyectotiponombre = this.proyecto.proyectotipo;
      this.unidadejecutoranombre = this.proyecto.unidadejecutora;
      this.unidadejecutoraid = this.proyecto.ueunidadEjecutora;
      this.directorProyectoNombre = this.proyecto.directorProyectoNmbre;
      this.directorProyectoId = this.proyecto.directorProyecto;
      this.entidadnombre = this.proyecto.entidadnombre;

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
              entidad: this.proyecto.entidad,
              ue: this.proyecto.ueunidadEjecutora
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
        });

        this.http.get('http://localhost:60066/api/ProyectoMiembro/MiembrosPorProyecto/' + this.proyecto.id, { withCredentials : true }).subscribe(response =>{
          if(response['success'] == true){
            let miembros = [];
            let tablaMiembro = [];
            miembros = response['miembros'];
            for(var i=0; i<miembros.length; i++)
            {
              tablaMiembro.push({ nombre: miembros[i].nombre, id: miembros[i].id });
            }

            this.sourceMiembro = new LocalDataSource(tablaMiembro);
          }
        });

        this.http.get('http://localhost:60065/api/ProyectoImpacto/ImpactosPorProyecto/' + this.proyecto.id, { withCredentials : true}).subscribe(response =>{
          if(response['success'] == true){
            let impactos = [];
            let tablaImpacto = [];
            impactos = response['impactos'];
            for(var i=0; i<impactos.length; i++){
              tablaImpacto.push({ id: impactos[i].entidadId, nombre: impactos[i].entidadNombre, impacto: impactos[i].impacto });
            }

            this.sourceImpacto = new LocalDataSource(tablaImpacto);
          }
        })
      }

      if(this.fechaInicioTemp == null || this.fechaInicioTemp == ""){
        this.fechaInicioTemp = moment(this.proyecto.fechaInicio,'DD/MM/YYYY').format('DD/MM/YYYY');
        this.fechaFinalTemp = moment(this.proyecto.fechaFin,'DD/MM/YYYY').format('DD/MM/YYYY');
      }

      if(this.fechaInicioRealTemp == null || this.fechaInicioRealTemp == ""){
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
    if(this.proyecto != null){
      for(var i=0; i < this.camposdinamicos.length; i++){
        this.botones = false;
        if(this.camposdinamicos[i].tipo === 'fecha'){
          this.camposdinamicos[i].valor_f = this.camposdinamicos[i].valor != null ? moment(this.camposdinamicos[i].valor).format('DD/MM/YYYY') : "";        
        }
      }

      this.sourceImpacto.getAll().then(value =>{
        var impactos = "";
        value.forEach(element => {
          impactos += (impactos.length > 0 ? "~" : "") + element.id + "," + element.impacto;
        });     
        
        this.sourceMiembro.getAll().then(value => {
          var miembros = "";
          value.forEach(element => {
            miembros += (miembros.length > 0 ? "," : "") + element.id;
          });

          this.proyecto.camposDinamicos = JSON.stringify(this.camposdinamicos);
          this.proyecto.impactos = impactos;
          this.proyecto.miembros = miembros;
          this.proyecto.directorProyecto = this.directorProyectoId;
          this.proyecto.duracion = this.duracionReal;

          var objetoHttp;

          if(this.proyecto.id > 0){
            objetoHttp = this.http.put("http://localhost:60064/api/Proyecto/Proyecto/" + this.proyecto.id, this.proyecto, { withCredentials: true });
          }
          else{
            this.proyecto.id=0;            
            //this.proyecto.entidad = 11110001;
            //this.proyecto.ejercicio = new Date().getFullYear();
            objetoHttp = this.http.post("http://localhost:60064/api/Proyecto/Proyecto", this.proyecto, { withCredentials: true });
          }

          objetoHttp.subscribe(response => {
            if(response['success'] == true){
              this.proyecto.id = response['id'];
              this.proyecto.usuarioCreo = response['usuarioCreo'];
              this.proyecto.fechaCreacion = response['fechaCreacion'];
              this.proyecto.usuarioActualizo = response['usuarioActualizo'];
              this.proyecto.fechaActualizacion = response['fechaActualizacion'];

              if(this.esTreeview){
                //this.t_cambiarNombreNodo();
              }
              else
                this.obtenerTotalProyectos();

              /*if(this.child_desembolso!=null || this.child_riesgos!=null){
                if(this.child_desembolso)
                  ret = this.child_desembolso.guardar($rootScope.etiquetas.proyecto+' '+(this.esNuevo ? 'creado' : 'guardado')+' con Éxito',
                      'Error al '+(mi.esNuevo ? 'creado' : 'guardado')+' el '+$rootScope.etiquetas.proyecto,
                      this.child_riesgos!=null ? this.child_riesgos.guardar :  null);
                else if(this.child_riesgos)
                  ret = this.child_riesgos.guardar($rootScope.etiquetas.proyecto+' '+(this.esNuevo ? 'creado' : 'guardado')+' con Éxito',
                      'Error al '+(this.esNuevo ? 'creado' : 'guardado')+' el '+$rootScope.etiquetas.proyecto);
              }
              else{
                $utilidades.mensaje('success',$rootScope.etiquetas.proyecto+' '+(this.esNuevo ? 'creado' : 'guardado')+' con Éxito');
                this.botones=true;
              }*/
              this.utils.mensaje('success', this.etiqueta.proyecto + ' ' + (this.esNuevo ? 'creado' : 'guardado') + ' con Éxito');
            }
            else{
              this.utils.mensaje('warning', 'Ocurrió un error al guardar el ' + this.etiqueta.proyecto);
            }
          });
        })
      })
    }
  }

  IrATabla(){
    this.esColapsado = false;
    this.proyecto = new Proyecto();
    this.proyectotipoid = 0;
    this.proyectotiponombre = "";
    this.sourceArchivosAdjuntos = new LocalDataSource();
    this.sourceImpacto = new LocalDataSource();
    this.sourceMiembro = new LocalDataSource();
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
    if(this.prestamoid == null){
      this.modalUnidadEjecutora.dialog.open(DialogUnidadEjecutora, {
        width: '600px',
        height: '585px',
        data: { titulo: 'Unidades Ejecutoras', ejercicio: this.proyecto.ejercicio, entidad: this.proyecto.entidad }
      }).afterClosed().subscribe(result => {
        if(result != null){
          this.unidadejecutoraid = result.id;
          this.unidadejecutoranombre = result.nombre;
        }
      })
    }
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

  buscarDirectorProyecto(){
    this.modalDirectorProyecto.dialog.open(DialogColaborador, {
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
        let tablaImpacto = [];
        this.sourceImpacto.getAll().then(value =>{
          value.forEach(element =>{
            tablaImpacto.push(element);
          })

          let existe = false;
          if(tablaImpacto.length==0)
            tablaImpacto.push({ id: result.id, nombre: result.nombre, impacto: result.impacto });
          else{
            tablaImpacto.forEach(element => {
              if(element.id==result.tipoPrestamoId)
                existe = true;              
            });

            if(!existe)
              tablaImpacto.push({ id: result.id, nombre: result.nombre, impacto: result.impacto });
          }  
        })
              
        this.sourceImpacto = new LocalDataSource(tablaImpacto);
      }
    })
  }

  settingsImpacto = {
    columns: {
      id: {
        title: 'ID',
        width: '8%',
        filter: false,
        type: 'html',
        class: 'align-center',
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

  agregarMiembro(){
    this.modalDirectorProyecto.dialog.open(DialogColaborador, {
      width: '600px',
      height: '585px',
      data: { titulo: 'Miembro' }
    }).afterClosed().subscribe(result=>{
      if(result != null){
        let tablaMiembro = [];
        this.sourceMiembro.getAll().then(value =>{
          value.forEach(element =>{
            tablaMiembro.push(element);
          })

          let existe = false;
          if(tablaMiembro.length==0)
            tablaMiembro.push({ nombre: result.nombre, id: result.id });
          else{
            tablaMiembro.forEach(element => {
              if(element.id==result.tipoPrestamoId)
                existe = true;              
            });

            if(!existe)
              tablaMiembro.push({ nombre: result.nombre, id: result.id });
          }  
        })
              
        this.sourceMiembro = new LocalDataSource(tablaMiembro);
      }
    })
  }

  settingsMiembro = {
    columns: {
      nombre: {
        title: 'Nombre',
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
            this.sourceMiembro.remove(row);            
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

  cargarArchivo(){
    this.modalCargarProject.dialog.open(DialogCargarProject, {
      width: '600px',
      height: '250px',
      data: { titulo: 'Cargar desde Project', proyectoid : 0, prestamoid: this.prestamoid }
    }).afterClosed().subscribe(result=>{
      if(result != null){
        if(result)
          this.proyecto.projectCargado = 1;
      }
    })
  }

  irAComponentes(proyectoId){
    if(this.proyecto!=null){
      this.router.navigateByUrl('/main/componente/'+ proyectoId);
    }
  }

  completarConArchivo(proyectoId){
    this.modalCargarProject.dialog.open(DialogCargarProject, {
      width: '600px',
      height: '250px',
      data: { titulo: 'Completar desde Project', proyectoid : this.proyecto.id, prestamoid: this.prestamoid }
    }).afterClosed().subscribe(result=>{
      if(result != null){
        if(result)
          this.proyecto.projectCargado = 1;
        else
          this.utils.mensaje('danger', 'Error al importar el archivo de Project');
      }
    })
  }
}