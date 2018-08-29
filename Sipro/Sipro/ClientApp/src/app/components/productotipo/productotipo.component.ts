import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ButtonDeleteComponent } from '../../../assets/customs/ButtonDeleteComponent';
import { ActivatedRoute } from "@angular/router";
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { ProductoTipo } from './model/ProductoTipo';
import { DialogDelete, DialogOverviewDelete } from '../../../assets/modals/deleteconfirmation/confirmation-delete';
import { DialogOverviewProductoPropiedad, DialogProductoPropiedad } from '../../../assets/modals/productopropiedad/modal-producto-propiedad';

@Component({
  selector: 'app-productotipo',
  templateUrl: './productotipo.component.html',
  styleUrls: ['./productotipo.component.css']
})
export class ProductotipoComponent implements OnInit {
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
  sourcePropiedades: LocalDataSource;
  source: LocalDataSource;
  totalProductotipos: number;
  busquedaGlobal : string;
  productotipo : ProductoTipo;
  paginaActual : number;
  esNuevo : boolean;
  modalProductoPropiedad : DialogOverviewProductoPropiedad;
  propiedades = [];
  modalDelete: DialogOverviewDelete;

  constructor(private route: ActivatedRoute, private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog) {
    this.sourcePropiedades = new LocalDataSource();
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.totalProductotipos = 0;
    this.busquedaGlobal = null;
    this.productotipo = new ProductoTipo();
    this.modalProductoPropiedad = new DialogOverviewProductoPropiedad(dialog);
    this.modalDelete = new DialogOverviewDelete(dialog);
  }

  ngOnInit() {
    this.mostrarcargando = true;
    this.obtenerTotalProductostipos();
  }

  obtenerTotalProductostipos(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      t: new Date().getTime()      
    };

    this.http.post('http://localhost:60060/api/ProductoTipo/TotalElementos', data, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.totalProductotipos = response['total'];
        this.paginaActual = 1;
        if(this.totalProductotipos > 0){
          this.cargarTabla(this.paginaActual);
        }
        else{
          this.source = new LocalDataSource();
          this.mostrarcargando = false;
        }
      }
    })
  }

  cargarTabla(pagina? : number){
    this.mostrarcargando = true;
    var filtro = {
      pagina: pagina,
      registros: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      ordenDireccion: null,
      t:moment().unix()
    };

    this.http.post('http://localhost:60060/api/ProductoTipo/ProductoTipoPagina', filtro, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        var data = response['productoTipos'];

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
    this.sourcePropiedades = new LocalDataSource();
  }

  editar(){
    if(this.productotipo.id > 0){
      this.esColapsado = true;
      this.esNuevo = false;

      this.http.get('http://localhost:60059/api/ProductoPropiedad/ProductoPropiedadPorTipoProducto/' + this.productotipo.id, { withCredentials : true }).subscribe(response =>{
        if(response['success'] == true){
          this.propiedades = response['productopropiedades'];
          this.sourcePropiedades = new LocalDataSource(this.propiedades);
        }
      })
    }
    else{
      this.utils.mensaje("warning", "Debe seleccionar un Tipo de producto que desea editar");
    }
  }

  borrar(){
    if(this.productotipo.id > 0){
      this.modalDelete.dialog.open(DialogDelete, {
        width: '600px',
        height: '200px',
        data: { 
          id: this.productotipo.id,
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar el tipo de producto?',
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result != null && result == true){
          this.http.delete('http://localhost:60060/api/ProductoTipo/ProductoTipo/'+ this.productotipo.id, { withCredentials : true }).subscribe(response =>{
            if(response['success'] == true){
              this.obtenerTotalProductostipos();
            }
        })
          
        }
      })
    }
    else{
      this.utils.mensaje("warning", "Seleccione el tipo de producto que desea borrar");
    }
  }

  filtrar(campo){
    this.busquedaGlobal = campo;
    this.obtenerTotalProductostipos();
  }

  onDblClickRow(event){
    this.productotipo = event.data;
    this.editar();
  }

  onSelectRow(event){
    this.productotipo = event.data;
  }

  handlePage(event){
    this.cargarTabla(event.pageIndex+1);
  }

  guardar(){
    if(this.productotipo != null){
      this.productotipo.propiedades = "";
      this.propiedades = [];

      this.sourcePropiedades.getAll().then(value =>{
        value.forEach(element => {
          this.propiedades.push(element); 
        });

        for(var x=0; x < this.propiedades.length; x++){
          this.productotipo.propiedades = this.productotipo.propiedades + (x > 0 ? "," : "") + this.propiedades[x].id;
        }

        var objetoHttp;

        if(this.productotipo.id > 0){
          objetoHttp = this.http.put("http://localhost:60060/api/ProductoTipo/ProductoTipo/" + this.productotipo.id, this.productotipo, { withCredentials: true });
        }
        else{
          objetoHttp = this.http.post("http://localhost:60060/api/ProductoTipo/ProductoTipo", this.productotipo, { withCredentials: true });
        }
  
        objetoHttp.subscribe(response =>{
          if(response['success'] == true){
            this.productotipo.usuarioCreo = response['usuarioCreo'];
            this.productotipo.fechaCreacion = response['fechaCreacion'];
            this.productotipo.fechaActualizacion = response['fechaActualizacion'];
            this.productotipo.usuarioActualizo = response['usuarioActualizo'];
            this.productotipo.id = response['id'];
  
            this.esNuevo = false;
            this.obtenerTotalProductostipos();
            this.utils.mensaje("success", "Tipo de producto guardado con éxito");
          }
        })
      })
    }
    else{
      this.utils.mensaje("warning", "Debe seleccionar un Tipo de dato");
    }
  }

  IrATabla(){
    this.esColapsado = false;
    this.productotipo = new ProductoTipo();
    this.sourcePropiedades = new LocalDataSource();
  }

  buscarPropiedades(){
    this.modalProductoPropiedad.dialog.open(DialogProductoPropiedad,{
      width: '600px',
      height: '585px',
      data: { titulo: 'Propiedades' }
    }).afterClosed().subscribe(result => {
      if(result != null){
        let tablaPropiedades = [];
        this.sourcePropiedades.getAll().then(value =>{
          value.forEach(element =>{
            tablaPropiedades.push(element);
          })

          let existe = false;
          if(tablaPropiedades.length==0)
          tablaPropiedades.push({ id: result.id, nombre: result.nombre, datoTiponombre : result.datoTiponombre });
          else{
            tablaPropiedades.forEach(element => {
              if(element.id==result.id)
                existe = true;              
            });

            if(!existe)
            tablaPropiedades.push({ id: result.id, nombre: result.nombre, datoTiponombre : result.datoTiponombre });
          }  
        })

        this.sourcePropiedades = new LocalDataSource(tablaPropiedades);
        this.sourcePropiedades.setSort([
          { field: 'id', direction: 'asc' }  // primary sort
        ]);
      }
    })
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
      descripcion: {
        title: 'Descripción',
        filter: false,       
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

  settingsPropiedades = {
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
        width: '28%',
        filter: false,
        class: 'align-left'
      },
      descripcion: {
        title: 'Descripción',
        align: 'left',
        width: '28%',
        filter: false,
        class: 'align-left'
      },
      datoTiponombre: {
        title: 'Tipo Dato',
        align: 'left',
        width: '28%',
        filter: false,
        class: 'align-left'
      },
      eliminar:{
        title: 'Eliminar',
        sort: false,
        type: 'custom',
        width: '10%',
        renderComponent: ButtonDeleteComponent,
        onComponentInitFunction: (instance) =>{
          instance.actionEmitter.subscribe(row => {
            this.sourcePropiedades.remove(row);
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
}
