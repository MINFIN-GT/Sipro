import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { MatDialog } from '@angular/material';
import { SubproductoPropiedad } from './model/SubproductoPropiedad';
import { DialogDelete, DialogOverviewDelete } from '../../../assets/modals/deleteconfirmation/confirmation-delete';

@Component({
  selector: 'app-subproductopropiedad',
  templateUrl: './subproductopropiedad.component.html',
  styleUrls: ['./subproductopropiedad.component.css']
})
export class SubproductopropiedadComponent implements OnInit {
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
  totalSubProductoPropiedades: number;
  subproductopropiedad : SubproductoPropiedad;
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
    this.totalSubProductoPropiedades = 0;
    this.subproductopropiedad = new SubproductoPropiedad();
    this.source = new LocalDataSource();
    this.modalDelete = new DialogOverviewDelete(dialog);
  }

  ngOnInit() {
    this.mostrarcargando = true;
    this.obtenerTotalSubProductoPropiedades();
    this.obtenerDatosTipo();
  }

  obtenerTotalSubProductoPropiedades(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      t:moment().unix()
    };

    this.http.post('http://localhost:60084/api/SubproductoPropiedad/TotalElementos', data, { withCredentials : true }).subscribe(response =>{
      if(response['success'] == true){
        this.totalSubProductoPropiedades = response['total'];
        this.paginaActual = 1;
        if(this.totalSubProductoPropiedades > 0)
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

    this.http.post('http://localhost:60084/api/SubproductoPropiedad/SubproductoPropiedadPagina', filtro, { withCredentials : true }).subscribe(response =>{
      if(response['success'] == true){
        var data = response['subproductoPropiedades'];
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
    this.subproductopropiedad.datoTipoid = opcion;
  }

  handlePage(event){
    this.cargarTabla(event.pageIndex+1);
  }

  nuevo(){
    this.esColapsado = true;
    this.esNuevo = true;
    this.subproductopropiedad = new SubproductoPropiedad();
    this.datoTipoSelected = 0;
  }

  editar(){
    if(this.subproductopropiedad.id > 0){
      this.esColapsado = true;
      this.esNuevo = false;
      this.datoTipoSelected = this.subproductopropiedad.datoTipoid;
    }
    else{
      this.utils.mensaje('warning', 'Debe seleccionar la Propiedad de subproducto que desea editar');
    }
  }

  borrar(){
    if(this.subproductopropiedad.id > 0){
      this.modalDelete.dialog.open(DialogDelete, {
        width: '600px',
        height: '200px',
        data: { 
          id: this.subproductopropiedad.id,
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar la propiedad de subproducto?',
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result != null && result){
          this.http.delete('http://localhost:60084/api/SubproductoPropiedad/SubproductoPropiedad/'+ this.subproductopropiedad.id, { withCredentials : true }).subscribe(response =>{
            if(response['success'] == true){
              this.obtenerTotalSubProductoPropiedades();
            }
          })  
        }
      })
    }
    else{
      this.utils.mensaje('warning', 'Seleccione una propiedad de subproducto');
    }
  }

  filtrar(campo){
    this.busquedaGlobal = campo;
    this.obtenerTotalSubProductoPropiedades();
  }

  onDblClickRow(event){
    this.subproductopropiedad = event.data;
    this.editar();
  }

  onSelectRow(event){
    this.subproductopropiedad = event.data;
  }

  guardar(){
    if(this.subproductopropiedad != null && Number(this.datoTipoSelected) != 0){
      var objetoHttp;

      if(this.subproductopropiedad.id > 0){
        objetoHttp = this.http.put("http://localhost:60084/api/SubproductoPropiedad/SubproductoPropiedad/" + this.subproductopropiedad.id, this.subproductopropiedad, { withCredentials: true });
      }
      else{
        objetoHttp = this.http.post("http://localhost:60084/api/SubproductoPropiedad/SubproductoPropiedad", this.subproductopropiedad, { withCredentials: true });
      }

      objetoHttp.subscribe(response =>{
        if(response['success'] == true){
          this.subproductopropiedad.usuarioCreo = response['usuarioCreo'];
          this.subproductopropiedad.fechaCreacion = response['fechaCreacion'];
          this.subproductopropiedad.fechaActualizacion = response['fechaActualizacion'];
          this.subproductopropiedad.usuarioActualizo = response['usuarioActualizo'];
          this.subproductopropiedad.id = response['id'];

          this.esNuevo = false;
          this.obtenerTotalSubProductoPropiedades();
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
    this.subproductopropiedad = new SubproductoPropiedad();
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
