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
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { DialogComponenteTipo, DialogOverviewComponenteTipo } from '../../../assets/modals/componentetipo/componente-tipo';
import { DialogOverviewUnidadEjecutora, DialogUnidadEjecutora } from '../../../assets/modals/unidadejecutora/unidad-ejecutora';
import { DialogDelete, DialogOverviewDelete } from '../../../assets/modals/deleteconfirmation/confirmation-delete';

export interface AcumulacionCosto {
  id: number;
  nombre: string;
  usuarioActualizo: string;
  usuarioCreo: string;
  fechaActualizacion : Date;
  fechaCreacion: Date;
  estado: number;
}

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
  bloquearCosto: boolean;
  asignado: number;
  sobrepaso: boolean;
  acumulacion_costo = [];
  dimensionSelected: number;
  modalComponenteTipo: DialogOverviewComponenteTipo;
  unidadejecutoraid: number;
  unidadejecutoranombre: string;
  entidadnombre: string;
  mostraringreso: boolean;
  camposdinamicos = [];
  modalUnidadEjecutora: DialogOverviewUnidadEjecutora;
  botones: boolean;
  modalDelete: DialogOverviewDelete;

  dimensiones = [
    {value:1,nombre:'Dias',sigla:'d'}
  ];

  @ViewChild('search') divSearch: ElementRef;
  myControl = new FormControl();
  filteredAcumulacionCosto: Observable<AcumulacionCosto[]>;
  
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

    this.filteredAcumulacionCosto = this.myControl.valueChanges
      .pipe(
        startWith(''),
        map(value => value ? this._filterAcumulacionCosto(value) : this.acumulacion_costo.slice())
      );

    this.modalComponenteTipo = new DialogOverviewComponenteTipo(dialog);
    this.modalUnidadEjecutora = new DialogOverviewUnidadEjecutora(dialog);
    this.modalDelete = new DialogOverviewDelete(dialog);
  }

  private _filterAcumulacionCosto(value: string): AcumulacionCosto[] {
    const filterValue = value.toLowerCase();
    return this.acumulacion_costo.filter(c => c.nombre.toLowerCase().indexOf(filterValue) === 0);
  }

  acumulacionCostoSeleccionado(value){
    this.componente.acumulacionCostoid = value.id;
  }

  ngOnInit() {
    this.mostrarcargando=true;
    this.obtenerTotalComponentes();
    this.obtenerAcumulacionCosto();
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

        for(var i =0; i<data.length; i++){
          data[i].fechaInicio = data[i].fechaInicio != null ? moment(data[i].fechaInicio,'DD/MM/YYYY').toDate() : null;
          data[i].fechaFin = data[i].fechaFin != null ? moment(data[i].fechaFin,'DD/MM/YYYY').format('DD/MM/YYYY') : null;
        }

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
    this.dimensionSelected = 1;
    this.coordenadas = null;
    this.camposdinamicos = [];
    this.unidadejecutoraid= this.prestamoid != null ? this.unidadEjecutora :  null;
    this.unidadejecutoranombre= this.prestamoid != null ? this.unidadEjecutoraNombre : null;
    this.componente.duracionDimension = this.dimensiones[this.dimensionSelected-1].sigla;
  }

  editar(){
    if(this.componente.id != null){
      this.esColapsado = true;
      this.esNuevo = false;
      this.tabActive = 0;
      this.dimensionSelected = 0;

      this.unidadejecutoraid= this.componente.ueunidadEjecutora;
      this.unidadejecutoranombre= this.componente.unidadejecutoranombre;
      this.ejercicio = this.componente.ejercicio;
      this.entidad = this.componente.entidad;
      this.entidadnombre = this.componente.entidadnombre;

      if(this.componente.acumulacionCostoid==2)
        this.bloquearCosto = true;
      else
        this.bloquearCosto = false;

      this.mostraringreso = true;
      this.esNuevo = false;

      this.coordenadas = (this.componente.latitud !=null ?  this.componente.latitud : '') +
        (this.componente.latitud!=null ? ', ' : '') + (this.componente.longitud!=null ? this.componente.longitud : '');
        
      this.obtenerCamposDinamicos();

      this.getAsignado();      
    }
    else
      this.utils.mensaje("warning", "Debe de seleccionar el componente que desea editar");
  }

  borrar(){
    if(this.componente.id > 0){
      this.modalDelete.dialog.open(DialogDelete, {
        width: '600px',
        height: '200px',
        data: { 
          id: this.componente.id,
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar el componente?',
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result != null){
          if(result){
            this.http.delete('http://localhost:60012/api/Componente/Componente/'+ this.componente.id, { withCredentials : true }).subscribe(response =>{
              if(response['success'] == true){
                this.obtenerTotalComponentes();
                this.utils.mensaje('success', 'Componente borrado exitosamente');
              }
            })
          } 
        }
      })
    }
    else{
      this.utils.mensaje('warning', 'Seleccione una propiedad de componente');
    }
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
    if(this.componente != null){
      for(var i=0; i < this.camposdinamicos.length; i++){
        this.botones = false;
        if(this.camposdinamicos[i].tipo === 'fecha'){
          this.camposdinamicos[i].valor_f = this.camposdinamicos[i].valor != null ? moment(this.camposdinamicos[i].valor).format('DD/MM/YYYY') : "";        
        }
      }

      this.componente.camposDinamicos = JSON.stringify(this.camposdinamicos);

      var objetoHttp;

      if(this.componente.id > 0){
        objetoHttp = this.http.put("http://localhost:60012/api/Componente/Componente/" + this.componente.id, this.componente, { withCredentials: true });
      }
      else{
        this.componente.proyectoid = this.pepid;
        this.componente.id=0;
        this.componente.ejercicio = this.ejercicio;
        this.componente.entidad = this.entidad;
        this.componente.ueunidadEjecutora = this.unidadEjecutora;
        objetoHttp = this.http.post("http://localhost:60012/api/Componente/Componente", this.componente, { withCredentials: true });
      }

      objetoHttp.subscribe(response => {
        if(response['success'] == true){
          this.componente.id = response['id'];
          this.componente.usuarioCreo = response['usuarioCreo'];
          this.componente.fechaCreacion = response['fechaCreacion'];
          this.componente.usuarioActualizo = response['usuarioActualizo'];
          this.componente.fechaActualizacion = response['fechaActualizacion'];

          if(!this.esTreeview)
							this.obtenerTotalComponentes();
						/*else{
							//if(!this.esNuevo)
								//mi.t_cambiarNombreNodo();
							//else
								//mi.t_crearNodo(mi.componente.id,mi.componente.nombre,1,true);
						}
						if(this.child_riesgos!=null){
							//ret = mi.child_riesgos.guardar('Componente '+(mi.esNuevo ? 'creado' : 'guardado')+' con éxito',
							//		'Error al '+(mi.esNuevo ? 'creado' : 'guardado')+' el Componente');
						}
						else
              $utilidades.mensaje('success','Componente '+(this.esNuevo ? 'creado' : 'guardado')+' con éxito');*/
            this.utils.mensaje('success', 'Componente '+(this.esNuevo ? 'creado' : 'guardado')+' con éxito');
            this.esNuevo = false;
        }
        else
          this.utils.mensaje('danger', 'No se pudo guardar el Componente.');
      })
    }
  }

  IrATabla(){
    this.esColapsado = false;
    this.componente = new Componente();
  }

  buscarUnidadEjecutora(){
    if(this.prestamoid == null){
      this.modalUnidadEjecutora.dialog.open(DialogUnidadEjecutora, {
        width: '600px',
        height: '585px',
        data: { titulo: 'Unidades Ejecutoras', ejercicio: this.componente.ejercicio, entidad: this.componente.entidad }
      }).afterClosed().subscribe(result => {
        if(result != null){
          this.unidadejecutoraid = result.id;
          this.unidadejecutoranombre = result.nombre;
        }
      })
    }
  }

  buscarComponenteTipo(){
    this.modalComponenteTipo.dialog.open(DialogComponenteTipo, {
      width: '600px',
      height: '585px',
      data: { titulo: 'Componente Tipo' }
    }).afterClosed().subscribe(result => {
      if(result != null){
        this.componente.componenteTipoid = result.id;
        this.componente.componentetiponombre = result.nombre;

        this.obtenerCamposDinamicos();
      }
    })
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

  validarAsignado(){
    if(this.componente.costo != null){
      if(this.componente.programa != null){
        if(this.componente.costo <= this.asignado)
          this.sobrepaso = false;
        else
          this.sobrepaso = true;
      }
    }
  }

  getAsignado() {
    var params = {
      id: this.componente.id,
      programa: this.componente.programa,
      subprograma: this.componente.subprograma,
      proyecto: this.componente.proyecto,
      actividad: this.componente.actividad,
      obra: this.componente.obra,
      renglon: this.componente.renglon,
      geografico: this.componente.ubicacionGeografica,
      t: new Date().getDate()
    }
    this.http.post('http://localhost:60012/api/Componente/ValidacionAsignado', params, { withCredentials: true }).subscribe(response =>{
      if(response['success']==true){
        this.asignado = response['asignado'];
        this.sobrepaso = response['sobrepaso'];
      }
    })
  }

  obtenerAcumulacionCosto(){
    this.http.get('http://localhost:60004/api/AcumulacionCosto/AcumulacionesCosto', { withCredentials: true}).subscribe(response => {
      if (response['success'] == true) {
        this.acumulacion_costo = response["acumulacionesTipos"];        
      }
    })
  }

  cambioDuracion(dimension){
    this.componente.fechaFin = this.sumarDias(this.componente.fechaInicio, this.componente.duracion, dimension.sigla);
  }

  modelChangedFechaInicio(event, dimension){
    this.componente.fechaFin = this.sumarDias(event._d, this.componente.duracion, dimension.sigla);
  }

  sumarDias(fecha, dias, dimension){
    if(dimension != undefined && dias != undefined && fecha != ""){
      var cnt = 0;
      var tmpDate = moment(fecha,'DD/MM/YYYY');
        while (cnt < (dias -1 )) {
          if(dimension=='d'){
            tmpDate = tmpDate.add(1,'days');	
          }
            if (tmpDate.weekday() != moment().day("Sunday").weekday() && tmpDate.weekday() != moment().day("Saturday").weekday()) {
                cnt = cnt + 1;
            }
        }
        tmpDate = moment(tmpDate,'DD/MM/YYYY');
        return tmpDate.format('DD/MM/YYYY');
    }
  }

  obtenerCamposDinamicos(){
    var parametros ={
      idProyecto : this.componente.id,
      idComponenteTipo : this.componente.componenteTipoid,
      t: new Date().getTime() 
    }
    this.http.get('http://localhost:60013/api/ComponentePropiedad/ComponentePropiedadPorTipo/'+this.componente.id+'/'+this.componente.componenteTipoid,  { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.camposdinamicos = response['componentepropiedades'];
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

  irASubcomponentes(componenteId){
    if(this.componente!=null){
      this.router.navigateByUrl('/main/subcomponente/'+ componenteId);
    }
  }
}
