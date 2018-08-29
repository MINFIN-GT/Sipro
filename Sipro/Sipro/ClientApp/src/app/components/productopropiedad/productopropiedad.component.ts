import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { MatDialog } from '@angular/material';
import { ProductoPropiedad } from './model/ProductoPropiedad';
import { DialogDelete, DialogOverviewDelete } from '../../../assets/modals/deleteconfirmation/confirmation-delete';

@Component({
  selector: 'app-productopropiedad',
  templateUrl: './productopropiedad.component.html',
  styleUrls: ['./productopropiedad.component.css']
})
export class ProductopropiedadComponent implements OnInit {
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
  totalProductoPropiedades: number;
  productopropiedad : ProductoPropiedad;
  busquedaGlobal: string;
  paginaActual : number;
  source: LocalDataSource;
  esNuevo : boolean;
  datoTipoSelected: number;
  tipodatos = [];
  modalDelete: DialogOverviewDelete;

  @ViewChild('search') divSearch: ElementRef;


  constructor(private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog) { 
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.totalProductoPropiedades = 0;
    this.productopropiedad = new ProductoPropiedad();
    this.source = new LocalDataSource();
    this.modalDelete = new DialogOverviewDelete(dialog);
  }

  ngOnInit() {
    this.mostrarcargando = true;
    this.obtenerTotalProductoPropiedades();
    this.obtenerDatosTipo();
  }

  obtenerTotalProductoPropiedades(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      t:moment().unix()
    };

    this.http.post('http://localhost:60059/api/ProductoPropiedad/TotalElementos', data, { withCredentials : true }).subscribe(response =>{
      if(response['success'] == true){
        this.totalProductoPropiedades = response['total'];
        this.paginaActual = 1;
        if(this.totalProductoPropiedades > 0)
          this.cargarTabla(this.paginaActual);
        else{
          this.source = new LocalDataSource();
          this.mostrarcargando = false;
        }
      }
      else{
        this.source = new LocalDataSource();
      }
    })
  }

  cargarTabla(pagina? : any){
    this.mostrarcargando = true;
    var filtro = {
      pagina: pagina,
      registros: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      t:moment().unix()
    };

    this.http.post('http://localhost:60059/api/ProductoPropiedad/ProductoPropiedadPagina', filtro, { withCredentials : true }).subscribe(response =>{
      if(response['success'] == true){
        var data = response['productoPropiedades'];
        this.source = new LocalDataSource(data);
        this.source.setSort([
          { field: 'id', direction: 'asc' }  // primary sort
        ]);
        this.busquedaGlobal = null;

        this.mostrarcargando = false;
      }
    })
  }

  obtenerDatosTipo(){
    this.http.get('http://localhost:60017/api/DatoTipo/Listar', { withCredentials : true }).subscribe(response =>{
      if(response['success'] == true){
        this.tipodatos = response['datoTipos']
      }
    })
  }

  cambioOpcionDatoTipo(opcion){
    this.productopropiedad.datoTipoid = opcion;
  }

  handlePage(event){
    this.cargarTabla(event.pageIndex+1);
  }

  nuevo(){
    this.esColapsado = true;
    this.esNuevo = true;
    this.productopropiedad = new ProductoPropiedad();
    this.datoTipoSelected = 0;
  }

  editar(){
    if(this.productopropiedad.id > 0){
      this.esColapsado = true;
      this.esNuevo = false;
      this.datoTipoSelected = this.productopropiedad.datoTipoid;
    }
    else{
      this.utils.mensaje('warning', 'Debe seleccionar la Propiedad de producto que desea editar');
    }
  }

  borrar(){
    if(this.productopropiedad.id > 0){
      this.modalDelete.dialog.open(DialogDelete, {
        width: '600px',
        height: '200px',
        data: { 
          id: this.productopropiedad.id,
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar la propiedad de producto?',
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result != null && result){
          this.http.delete('http://localhost:60059/api/ProductoPropiedad/ProductoPropiedad/'+ this.productopropiedad.id, { withCredentials : true }).subscribe(response =>{
            if(response['success'] == true){
              this.obtenerTotalProductoPropiedades();
            }
          })  
        }
      })
    }
    else{
      this.utils.mensaje('warning', 'Seleccione una propiedad de producto');
    }
  }

  filtrar(campo){
    this.busquedaGlobal = campo;
    this.obtenerTotalProductoPropiedades();
  }

  onDblClickRow(event){
    this.productopropiedad = event.data;
    this.editar();
  }

  onSelectRow(event){
    this.productopropiedad = event.data;
  }

  guardar(){
    if(this.productopropiedad != null && Number(this.datoTipoSelected) != 0){
      var objetoHttp;

      if(this.productopropiedad.id > 0){
        objetoHttp = this.http.put("http://localhost:60059/api/ProductoPropiedad/ProductoPropiedad/" + this.productopropiedad.id, this.productopropiedad, { withCredentials: true });
      }
      else{
        objetoHttp = this.http.post("http://localhost:60059/api/ProductoPropiedad/ProductoPropiedad", this.productopropiedad, { withCredentials: true });
      }

      objetoHttp.subscribe(response =>{
        if(response['success'] == true){
          this.productopropiedad.usuarioCreo = response['usuarioCreo'];
          this.productopropiedad.fechaCreacion = response['fechaCreacion'];
          this.productopropiedad.fechaActualizacion = response['fechaActualizacion'];
          this.productopropiedad.usuarioActualizo = response['usuarioActualizo'];
          this.productopropiedad.id = response['id'];

          this.esNuevo = false;
          this.obtenerTotalProductoPropiedades();
          this.utils.mensaje('success', 'Propiedad guardada exitosamente');
        }
      })
    }
    else{
      this.utils.mensaje('warning', 'Debe seleccionar un Tipo de dato');
    }
  }

  IrATabla(){
    this.esColapsado = false;
    this.productopropiedad = new ProductoPropiedad();
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
      datoTipoNombre: {
        title: 'Tipo dato',
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
}
