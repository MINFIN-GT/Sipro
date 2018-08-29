import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import * as moment from 'moment';
import { LocalDataSource } from 'ng2-smart-table';
import { Producto } from './model/Producto';
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
import { DialogOverviewProductoTipo, DialogProductoTipo } from '../../../assets/modals/productotipo/modal-producto-tipo';
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
  selector: 'app-producto',
  templateUrl: './producto.component.html',
  styleUrls: ['./producto.component.css']
})
export class ProductoComponent implements OnInit {
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
  totalProductos: number;
  elementosPorPagina: number;
  numeroMaximoPaginas: number;
  producto: Producto;
  objetoTipo: number;
  objetoId: number;
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
  modalProductoTipo: DialogOverviewProductoTipo;
  unidadejecutoraid: number;
  unidadejecutoranombre: string;
  entidadnombre: string;
  mostraringreso: boolean;
  camposdinamicos = [];
  botones: boolean;
  objetoNombre: string;
  objetoTipoNombre: string;
  modalDelete: DialogOverviewDelete;
  inversionNueva : boolean;

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
    this.totalProductos = 0;

    this.route.params.subscribe(param => {
      if(param.objeto_tipo == 1){
        this.objetoTipo = Number(param.objeto_tipo);
        this.objetoId = Number(param.objeto_id);
      }
      else if(param.objeto_tipo == 2){
        this.objetoTipo = Number(param.objeto_tipo);
        this.objetoId = Number(param.objeto_id);
      }
    })

    this.busquedaGlobal = null;
    this.tabActive = 0;
    this.congelado = 0;

    this.obtenerObjetoTipo();
    this.producto = new Producto();
    this.modalMapa = new DialogOverviewMapa(dialog);

    this.filteredAcumulacionCosto = this.myControl.valueChanges
      .pipe(
        startWith(''),
        map(value => value ? this._filterAcumulacionCosto(value) : this.acumulacion_costo.slice())
      );

    this.modalUnidadEjecutora = new DialogOverviewUnidadEjecutora(dialog);
    this.modalProductoTipo = new DialogOverviewProductoTipo(dialog);
    this.unidadejecutoranombre = "";
    this.modalDelete = new DialogOverviewDelete(dialog);
  }

  private _filterAcumulacionCosto(value: string): AcumulacionCosto[] {
    const filterValue = value.toLowerCase();
    return this.acumulacion_costo.filter(c => c.nombre.toLowerCase().indexOf(filterValue) === 0);
  }

  acumulacionCostoSeleccionado(value){
    this.producto.acumulacionCostoid = value.id;
  }

  ngOnInit() {
    this.mostrarcargando=true;
    this.obtenerTotalProductos();
    this.obtenerAcumulacionCosto();
  }

  obtenerObjetoTipo(){
    var objetoHttp;

    if(this.objetoTipo == 1){
      objetoHttp = this.http.get('http://localhost:60012/api/Componente/ComponentePorId/' + this.objetoId, { withCredentials: true })
    }
    else if(this.objetoTipo == 2){
      objetoHttp = this.http.get('http://localhost:60080/api/Subcomponente/ObtenerSubComponentePorId/' + this.objetoId, { withCredentials: true })
    }

    objetoHttp.subscribe(response => {
      if (response['success'] == true) {
        this.objetoNombre = response['nombre'];
        this.objetoTipoNombre = this.objetoTipo == 1 ? 'Componente' : 'Subcomponente';
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

  obtenerTotalProductos(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      componenteid: this.objetoTipo == 1 ? this.objetoId : null,
      subcomponenteid: this.objetoTipo == 2 ? this.objetoId : null,
      t: new Date().getTime()      
    };
    this.http.post('http://localhost:60058/api/Producto/TotalElementos', data, { withCredentials : true}).subscribe(response =>{
      if(response['success'] == true){
        this.totalProductos = response['total'];
        this.paginaActual = 1;
        if(this.totalProductos > 0){
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
      componenteid: this.objetoTipo == 1 ? this.objetoId : null,
      subcomponenteid: this.objetoTipo == 2 ? this.objetoId : null,
      pagina: pagina,
      numerosubcomponentes: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      t:moment().unix()
    };

    this.http.post('http://localhost:60058/api/Producto/ProductoPagina', filtro, { withCredentials : true}).subscribe(
      response =>{
        if(response['success'] == true){
          var data = response['productos'];

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
    this.producto.duracionDimension = this.dimensiones[this.dimensionSelected-1].sigla;
  }

  editar(){
    if(this.producto.id != null){
      this.esColapsado = true;
      this.esNuevo = false;
      this.tabActive = 0;
      this.dimensionSelected = 0;
      this.inversionNueva = this.producto.inversionNueva == 1 ? true : false;
      this.unidadejecutoraid= this.producto.ueunidadEjecutora;
      this.unidadejecutoranombre= this.producto.nombreUnidadEjecutora;
      this.ejercicio = this.producto.ejercicio;
      this.entidad = this.producto.entidad;
      this.entidadnombre = this.producto.entidadnombre;

      if(this.producto.acumulacionCostoid==2)
        this.bloquearCosto = true;
      else
        this.bloquearCosto = false;

      this.mostraringreso = true;
      this.esNuevo = false;

      this.coordenadas = (this.producto.latitud !=null ?  this.producto.latitud : '') +
        (this.producto.latitud!=null ? ', ' : '') + (this.producto.longitud!=null ? this.producto.longitud : '');
        
      this.obtenerCamposDinamicos();

      this.getAsignado();      
    }
    else
      this.utils.mensaje("warning", "Debe de seleccionar el producto que desea editar");
  }

  borrar(){
    if(this.producto.id > 0){
      this.modalDelete.dialog.open(DialogDelete, {
        width: '600px',
        height: '200px',
        data: { 
          id: this.producto.id,
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar el producto?',
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result != null){
          if(result){
            this.http.delete('http://localhost:60058/api/Producto/Producto/'+ this.producto.id, { withCredentials : true }).subscribe(response =>{
              if(response['success'] == true){
                this.obtenerTotalProductos();
                this.utils.mensaje('success', 'Producto borrado exitosamente');
              }
            })
          } 
        }
      })
    }
    else{
      this.utils.mensaje('warning', 'Seleccione una propiedad de producto');
    }
  }

  filtrar(campo){
    this.busquedaGlobal = campo;
    this.obtenerTotalProductos();
  }

  onSelectRow(event) {
    this.producto = event.data;
  }

  onDblClickRow(event) {
    this.producto = event.data;
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
    if(this.producto != null){
      for(var i=0; i < this.camposdinamicos.length; i++){
        this.botones = false;
        if(this.camposdinamicos[i].tipo === 'fecha'){
          this.camposdinamicos[i].valor_f = this.camposdinamicos[i].valor != null ? moment(this.camposdinamicos[i].valor).format('DD/MM/YYYY') : "";        
        }
      }

      this.producto.inversionNueva = this.inversionNueva != null ? this.inversionNueva == true ? 1 : 0: 0;      
      this.producto.camposDinamicos = JSON.stringify(this.camposdinamicos);
      this.producto.componenteid = this.objetoTipo == 1 ? this.objetoId : null;
      this.producto.subcomponenteid = this.objetoTipo == 2 ? this.objetoId : null;

      var objetoHttp;

      if(this.producto.id > 0){
        objetoHttp = this.http.put("http://localhost:60058/api/Producto/Producto/" + this.producto.id, this.producto, { withCredentials: true });
      }
      else{        
        this.producto.id=0;
        this.producto.ejercicio = this.ejercicio;
        this.producto.entidad = this.entidad;
        this.producto.ueunidadEjecutora = this.unidadEjecutora;
        objetoHttp = this.http.post("http://localhost:60058/api/Producto/Producto", this.producto, { withCredentials: true });
      }

      objetoHttp.subscribe(response => {
        if(response['success'] == true){
          this.producto.id = response['id'];
          this.producto.usuarioCreo = response['usuarioCreo'];
          this.producto.fechaCreacion = response['fechaCreacion'];
          this.producto.usuarioActualizo = response['usuarioActualizo'];
          this.producto.fechaActualizacion = response['fechaActualizacion'];

          if(!this.esTreeview)
							this.obtenerTotalProductos();
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
            this.utils.mensaje('success', 'Producto '+(this.esNuevo ? 'creado' : 'guardado')+' con éxito');
            this.esNuevo = false;
        }
        else
          this.utils.mensaje('warning', 'No se pudo guardar el Producto.');
      },
      error => {
          this.utils.mensaje('danger', 'Ocurrió un error al guardar el Producto');
      }
    )
    }
  }

  IrATabla(){
    this.esColapsado = false;
    this.producto = new Producto();
  }

  buscarUnidadEjecutora(){
    if(this.prestamoid == null){
      this.modalUnidadEjecutora.dialog.open(DialogUnidadEjecutora, {
        width: '600px',
        height: '585px',
        data: { titulo: 'Unidades Ejecutoras', ejercicio: this.producto.ejercicio, entidad: this.producto.entidad }
      }).afterClosed().subscribe(result => {
        if(result != null){
          this.unidadejecutoraid = result.id;
          this.unidadejecutoranombre = result.nombre;
        }
      })
    }
  }

  buscarProductoTipo(){
    this.modalProductoTipo.dialog.open(DialogProductoTipo, {
      width: '600px',
      height: '585px',
      data: { titulo: 'Producto Tipo' }
    }).afterClosed().subscribe(result => {
      if(result != null){
        this.producto.productoTipoid = result.id;
        this.producto.productoTipoNombre = result.nombre;

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
        this.producto.latitud = result.latitud;
        this.producto.longitud = result.longitud;
        this.coordenadas = result.latitud + ", " + result.longitud;
      }else{
        this.coordenadas = '';
      }
    })
  }

  validarAsignado(){
    if(this.producto.costo != null){
      if(this.producto.programa != null){
        if(this.producto.costo <= this.asignado)
          this.sobrepaso = false;
        else
          this.sobrepaso = true;
      }
    }
  }

  getAsignado() {
    var params = {
      id: this.producto.id,
      programa: this.producto.programa,
      subprograma: this.producto.subprograma,
      proyecto: this.producto.proyecto,
      actividad: this.producto.actividad,
      obra: this.producto.obra,
      renglon: this.producto.renglon,
      geografico: this.producto.ubicacionGeografica,
      t: new Date().getDate()
    }
    this.http.post('http://localhost:60058/api/Producto/ValidacionAsignado', params, { withCredentials: true }).subscribe(response =>{
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
    this.producto.fechaFin = this.sumarDias(this.producto.fechaInicio, this.producto.duracion, dimension.sigla);
  }

  modelChangedFechaInicio(event, dimension){
    this.producto.fechaFin = this.sumarDias(event._d, this.producto.duracion, dimension.sigla);
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
      t: new Date().getTime() 
    }
    this.http.get('http://localhost:60059/api/ProductoPropiedad/ProductoPropiedadPorTipo/'+this.producto.id+'/'+this.producto.productoTipoid,  { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.camposdinamicos = response['productopropiedades'];
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

  irASubproducto(productoId){
    if(this.producto!=null){
      this.router.navigateByUrl('/main/subproducto/'+ productoId);
    }
  }

  irAActividad(productoId){
    if(this.producto!=null){
      this.router.navigateByUrl('/main/actividad/'+ productoId);
    }
  }
}
