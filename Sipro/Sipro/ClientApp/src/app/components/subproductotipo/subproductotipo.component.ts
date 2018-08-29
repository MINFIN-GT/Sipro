import { Component, OnInit, ViewChild, ElementRef  } from '@angular/core';
import { ButtonDeleteComponent } from '../../../assets/customs/ButtonDeleteComponent';
import { ActivatedRoute } from "@angular/router";
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { SubproductoTipo } from './model/SubproductoTipo';
import { DialogDelete, DialogOverviewDelete } from '../../../assets/modals/deleteconfirmation/confirmation-delete';
import { DialogOverviewSubproductoPropiedad, DialogSubproductoPropiedad} from '../../../assets/modals/subproductopropiedad/modal-subproducto-propiedad';

@Component({
  selector: 'app-subproductotipo',
  templateUrl: './subproductotipo.component.html',
  styleUrls: ['./subproductotipo.component.css']
})
export class SubproductotipoComponent implements OnInit {
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
  totalSubProductotipos : number;
  busquedaGlobal : string;
  subproductotipo : SubproductoTipo;
  paginaActual : number;
  esNuevo : boolean;
  modalSubproductoPropiedad : DialogOverviewSubproductoPropiedad;
  propiedades = [];
  modalDelete: DialogOverviewDelete;

  constructor(private route: ActivatedRoute, private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog) { 
    this.sourcePropiedades = new LocalDataSource();
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.totalSubProductotipos = 0;
    this.busquedaGlobal = null;
    this.subproductotipo = new SubproductoTipo();
    this.modalSubproductoPropiedad = new DialogOverviewSubproductoPropiedad(dialog);
    this.modalDelete = new DialogOverviewDelete(dialog);
  }

  ngOnInit() {
    this.mostrarcargando = true;
    this.obtenerTotalSubproductostipos();
  }

  obtenerTotalSubproductostipos(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      t: new Date().getTime()      
    };

    this.http.post('http://localhost:60085/api/SubproductoTipo/TotalElementos', data, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.totalSubProductotipos = response['total'];
        this.paginaActual = 1;
        if(this.totalSubProductotipos > 0){
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

    this.http.post('http://localhost:60085/api/SubproductoTipo/SubproductoTipoPagina', filtro, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        var data = response['subproductoTipos'];

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
    if(this.subproductotipo.id > 0){
      this.esColapsado = true;
      this.esNuevo = false;

      this.http.get('http://localhost:60084/api/SubproductoPropiedad/SubProductoPropiedadPorTipoSubProducto/' + this.subproductotipo.id, { withCredentials : true }).subscribe(response =>{
        if(response['success'] == true){
          this.propiedades = response['subproductopropiedades'];
          this.sourcePropiedades = new LocalDataSource(this.propiedades);
        }
      })
    }
    else{
      this.utils.mensaje("warning", "Debe seleccionar un Tipo de subproducto que desea editar");
    }
  }

  borrar(){
    if(this.subproductotipo.id > 0){
      this.modalDelete.dialog.open(DialogDelete, {
        width: '600px',
        height: '200px',
        data: { 
          id: this.subproductotipo.id,
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar el tipo de subproducto?',
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result != null && result == true){
          this.http.delete('http://localhost:60085/api/SubproductoTipo/SubproductoTipo/'+ this.subproductotipo.id, { withCredentials : true }).subscribe(response =>{
            if(response['success'] == true){
              this.obtenerTotalSubproductostipos();
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
    this.obtenerTotalSubproductostipos();
  }

  onDblClickRow(event){
    this.subproductotipo = event.data;
    this.editar();
  }

  onSelectRow(event){
    this.subproductotipo = event.data;
  }

  handlePage(event){
    this.cargarTabla(event.pageIndex+1);
  }

  guardar(){
    if(this.subproductotipo != null){
      this.subproductotipo.propiedades = "";
      this.propiedades = [];

      this.sourcePropiedades.getAll().then(value =>{
        value.forEach(element => {
          this.propiedades.push(element); 
        });

        for(var x=0; x < this.propiedades.length; x++){
          this.subproductotipo.propiedades = this.subproductotipo.propiedades + (x > 0 ? "," : "") + this.propiedades[x].id;
        }

        var objetoHttp;

        if(this.subproductotipo.id > 0){
          objetoHttp = this.http.put("http://localhost:60085/api/SubproductoTipo/SubproductoTipo/" + this.subproductotipo.id, this.subproductotipo, { withCredentials: true });
        }
        else{
          objetoHttp = this.http.post("http://localhost:60085/api/SubproductoTipo/SubproductoTipo", this.subproductotipo, { withCredentials: true });
        }
  
        objetoHttp.subscribe(response =>{
          if(response['success'] == true){
            this.subproductotipo.usuarioCreo = response['usuarioCreo'];
            this.subproductotipo.fechaCreacion = response['fechaCreacion'];
            this.subproductotipo.fechaActualizacion = response['fechaActualizacion'];
            this.subproductotipo.usuarioActualizo = response['usuarioActualizo'];
            this.subproductotipo.id = response['id'];
  
            this.esNuevo = false;
            this.obtenerTotalSubproductostipos();
            this.utils.mensaje("success", "Tipo de subproducto guardado con éxito");
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
    this.subproductotipo = new SubproductoTipo();
    this.sourcePropiedades = new LocalDataSource();
  }

  buscarPropiedades(){
    this.modalSubproductoPropiedad.dialog.open(DialogSubproductoPropiedad,{
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
