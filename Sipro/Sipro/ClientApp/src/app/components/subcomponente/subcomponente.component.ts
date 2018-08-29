import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import * as moment from 'moment';
import { LocalDataSource } from 'ng2-smart-table';
import { Subcomponente } from './model/Subcomponente';
import { ActivatedRoute } from "@angular/router";
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';
import { DialogMapa, DialogOverviewMapa } from '../../../assets/modals/cargamapa/modal-carga-mapa';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { DialogOverviewUnidadEjecutora, DialogUnidadEjecutora } from '../../../assets/modals/unidadejecutora/unidad-ejecutora';
import { DialogSubComponenteTipo, DialogOverviewSubComponenteTipo } from '../../../assets/modals/subcomponentetipo/modal-subcomponente-tipo';
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
  selector: 'app-subcomponente',
  templateUrl: './subcomponente.component.html',
  styleUrls: ['./subcomponente.component.css']
})
export class SubcomponenteComponent implements OnInit {
  mostrarcargando : boolean;
  color = 'primary';
  mode = 'indeterminate';
  value = 50;
  diameter = 45;
  strokewidth = 3;

  isLoggedIn : boolean;
  isMasterPage : boolean;
  esTreeview: boolean;
  esColapsado: boolean;
  congelado: number;  
  source : LocalDataSource;
  totalSubComponentes: number;
  elementosPorPagina: number;
  numeroMaximoPaginas: number;
  subcomponente: Subcomponente;
  componenteid: number;
  busquedaGlobal : string;
  paginaActual : number;
  tabActive: number;
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
  modalUnidadEjecutora: DialogOverviewUnidadEjecutora;
  modalSubComponenteTipo: DialogOverviewSubComponenteTipo;
  unidadejecutoraid: number;
  unidadejecutoranombre: string;
  entidadnombre: string;
  mostraringreso: boolean;
  camposdinamicos = [];
  botones: boolean;
  componenteNombre: string;
  objetoTipoNombre: string;
  modalDelete: DialogOverviewDelete;

  dimensiones = [
    {value:1,nombre:'Dias',sigla:'d'}
  ];

  @ViewChild('search') divSearch: ElementRef;
  myControl = new FormControl();
  filteredAcumulacionCosto: Observable<AcumulacionCosto[]>;

  constructor(private route: ActivatedRoute, private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog, private router: Router) {
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.totalSubComponentes = 0;

    this.route.params.subscribe(param => {
      this.componenteid = Number(param.id);
    })

    this.busquedaGlobal = null;
    this.tabActive = 0;
    this.congelado = 0;
    this.obtenerComponente();
    this.subcomponente = new Subcomponente();
    this.modalMapa = new DialogOverviewMapa(dialog);

    this.filteredAcumulacionCosto = this.myControl.valueChanges
      .pipe(
        startWith(''),
        map(value => value ? this._filterAcumulacionCosto(value) : this.acumulacion_costo.slice())
      );

    this.modalUnidadEjecutora = new DialogOverviewUnidadEjecutora(dialog);
    this.modalSubComponenteTipo = new DialogOverviewSubComponenteTipo(dialog);
    this.unidadejecutoranombre = "";
    this.modalDelete = new DialogOverviewDelete(dialog);
   }

   private _filterAcumulacionCosto(value: string): AcumulacionCosto[] {
    const filterValue = value.toLowerCase();
    return this.acumulacion_costo.filter(c => c.nombre.toLowerCase().indexOf(filterValue) === 0);
  }

  acumulacionCostoSeleccionado(value){
    this.subcomponente.acumulacionCostoid = value.id;
  }

  ngOnInit() {
    this.mostrarcargando=true;
    this.obtenerTotalSubcomponentes();
    this.obtenerAcumulacionCosto();
  }

  obtenerComponente(){
    this.http.get('http://localhost:60012/api/Componente/ComponentePorId/' + this.componenteid, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.componenteNombre = response['nombre'];
        this.objetoTipoNombre = 'Componente';
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

  obtenerTotalSubcomponentes(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      componenteid: this.componenteid,
      t: new Date().getTime()      
    };
    this.http.post('http://localhost:60080/api/Subcomponente/NumeroSubComponentesPorComponente', data, { withCredentials : true}).subscribe(response =>{
      if(response['success'] == true){
        this.totalSubComponentes = response['totalsubcomponentes'];
        this.paginaActual = 1;
        if(this.totalSubComponentes > 0){
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
      componenteid: this.componenteid,
      pagina: pagina,
      numerosubcomponentes: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      t:moment().unix()
    };

    this.http.post('http://localhost:60080/api/Subcomponente/SubComponentesPaginaPorComponente', filtro, { withCredentials : true}).subscribe(
      response =>{
        if(response['success'] == true){
          var data = response['subcomponentes'];

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
    this.subcomponente.duracionDimension = this.dimensiones[this.dimensionSelected-1].sigla;
  }

  editar(){
    if(this.subcomponente.id != null){
      this.esColapsado = true;
      this.esNuevo = false;
      this.tabActive = 0;
      this.dimensionSelected = 0;

      this.unidadejecutoraid= this.subcomponente.ueunidadEjecutora;
      this.unidadejecutoranombre= this.subcomponente.unidadejecutoranombre;
      this.ejercicio = this.subcomponente.ejercicio;
      this.entidad = this.subcomponente.entidad;
      this.entidadnombre = this.subcomponente.entidadnombre;

      if(this.subcomponente.acumulacionCostoid==2)
        this.bloquearCosto = true;
      else
        this.bloquearCosto = false;

      this.mostraringreso = true;
      this.esNuevo = false;

      this.coordenadas = (this.subcomponente.latitud !=null ?  this.subcomponente.latitud : '') +
        (this.subcomponente.latitud!=null ? ', ' : '') + (this.subcomponente.longitud!=null ? this.subcomponente.longitud : '');
        
      this.obtenerCamposDinamicos();

      this.getAsignado();      
    }
    else
      this.utils.mensaje("warning", "Debe de seleccionar el subcomponente que desea editar");
  }

  borrar(){
    if(this.subcomponente.id > 0){
      this.modalDelete.dialog.open(DialogDelete, {
        width: '600px',
        height: '200px',
        data: { 
          id: this.subcomponente.id,
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar el subcomponente?',
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result != null){
          if(result){
            this.http.delete('http://localhost:60080/api/Subcomponente/SubComponente/'+ this.subcomponente.id, { withCredentials : true }).subscribe(response =>{
              if(response['success'] == true){
                this.obtenerTotalSubcomponentes();
                this.utils.mensaje('success', 'Subcomponente borrado exitosamente');
              }
            })
          } 
        }
      })
    }
    else{
      this.utils.mensaje('warning', 'Seleccione una propiedad de subcomponente');
    }
  }

  filtrar(campo){
    this.busquedaGlobal = campo;
    this.obtenerTotalSubcomponentes();
  }

  onSelectRow(event) {
    this.subcomponente = event.data;
  }

  onDblClickRow(event) {
    this.subcomponente = event.data;
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
    if(this.subcomponente != null){
      for(var i=0; i < this.camposdinamicos.length; i++){
        this.botones = false;
        if(this.camposdinamicos[i].tipo === 'fecha'){
          this.camposdinamicos[i].valor_f = this.camposdinamicos[i].valor != null ? moment(this.camposdinamicos[i].valor).format('DD/MM/YYYY') : "";        
        }
      }

      this.subcomponente.inversionNueva = this.subcomponente.inversionNueva == 1 ? 1 : this.subcomponente.inversionNueva.toString() == "true" ? 1 : 0;
      this.subcomponente.camposDinamicos = JSON.stringify(this.camposdinamicos);
      this.subcomponente.componenteid = this.componenteid;

      var objetoHttp;

      if(this.subcomponente.id > 0){
        objetoHttp = this.http.put("http://localhost:60080/api/Subcomponente/SubComponente/" + this.subcomponente.id, this.subcomponente, { withCredentials: true });
      }
      else{        
        this.subcomponente.id=0;
        this.subcomponente.ejercicio = this.ejercicio;
        this.subcomponente.entidad = this.entidad;
        this.subcomponente.ueunidadEjecutora = this.unidadEjecutora;
        objetoHttp = this.http.post("http://localhost:60080/api/Subcomponente/SubComponente", this.subcomponente, { withCredentials: true });
      }

      objetoHttp.subscribe(response => {
        if(response['success'] == true){
          this.subcomponente.id = response['id'];
          this.subcomponente.usuarioCreo = response['usuarioCreo'];
          this.subcomponente.fechaCreacion = response['fechaCreacion'];
          this.subcomponente.usuarioActualizo = response['usuarioActualizo'];
          this.subcomponente.fechaActualizacion = response['fechaActualizacion'];

          if(!this.esTreeview)
							this.obtenerTotalSubcomponentes();
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
            this.utils.mensaje('success', 'Subcomponente '+(this.esNuevo ? 'creado' : 'guardado')+' con éxito');
            this.esNuevo = false;
        }
        else
          this.utils.mensaje('warning', 'No se pudo guardar el Subcomponente.');
      },
      error => {
          this.utils.mensaje('danger', 'Ocurrió un error al guardar el Subcomponente');
      }
    )
    }
  }

  IrATabla(){
    this.esColapsado = false;
    this.subcomponente = new Subcomponente();
  }

  buscarUnidadEjecutora(){
    if(this.prestamoid == null){
      this.modalUnidadEjecutora.dialog.open(DialogUnidadEjecutora, {
        width: '600px',
        height: '585px',
        data: { titulo: 'Unidades Ejecutoras', ejercicio: this.subcomponente.ejercicio, entidad: this.subcomponente.entidad }
      }).afterClosed().subscribe(result => {
        if(result != null){
          this.unidadejecutoraid = result.id;
          this.unidadejecutoranombre = result.nombre;
        }
      })
    }
  }

  buscarSubcomponenteTipo(){
    this.modalSubComponenteTipo.dialog.open(DialogSubComponenteTipo, {
      width: '600px',
      height: '585px',
      data: { titulo: 'Subcomponente Tipo' }
    }).afterClosed().subscribe(result => {
      if(result != null){
        this.subcomponente.subcomponenteTipoid = result.id;
        this.subcomponente.subcomponentetiponombre = result.nombre;

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
        this.subcomponente.latitud = result.latitud;
        this.subcomponente.longitud = result.longitud;
        this.coordenadas = result.latitud + ", " + result.longitud;
      }else{
        this.coordenadas = '';
      }
    })
  }

  validarAsignado(){
    if(this.subcomponente.costo != null){
      if(this.subcomponente.programa != null){
        if(this.subcomponente.costo <= this.asignado)
          this.sobrepaso = false;
        else
          this.sobrepaso = true;
      }
    }
  }

  getAsignado() {
    var params = {
      id: this.subcomponente.id,
      programa: this.subcomponente.programa,
      subprograma: this.subcomponente.subprograma,
      proyecto: this.subcomponente.proyecto,
      actividad: this.subcomponente.actividad,
      obra: this.subcomponente.obra,
      renglon: this.subcomponente.renglon,
      geografico: this.subcomponente.ubicacionGeografica,
      t: new Date().getDate()
    }
    this.http.post('http://localhost:60080/api/Subcomponente/ValidacionAsignado', params, { withCredentials: true }).subscribe(response =>{
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
    this.subcomponente.fechaFin = this.sumarDias(this.subcomponente.fechaInicio, this.subcomponente.duracion, dimension.sigla);
  }

  modelChangedFechaInicio(event, dimension){
    this.subcomponente.fechaFin = this.sumarDias(event._d, this.subcomponente.duracion, dimension.sigla);
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
      idProyecto : this.subcomponente.id,
      idComponenteTipo : this.subcomponente.subcomponenteTipoid,
      t: new Date().getTime() 
    }
    this.http.get('http://localhost:60081/api/SubcomponentePropiedad/SubComponentePropiedadPorTipo/'+this.subcomponente.id+'/'+this.subcomponente.subcomponenteTipoid,  { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.camposdinamicos = response['subcomponentepropiedades'];
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

  irAProductos(subcomponenteId){
    if(this.subcomponente!=null){
      this.router.navigateByUrl('/main/producto/'+subcomponenteId+'/2');
    }
  }

  irAActividad(productoId){
    if(this.subcomponente!=null){
      this.router.navigateByUrl('/main/actividad/'+ productoId);
    }
  }
}
