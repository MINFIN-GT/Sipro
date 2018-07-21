import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ButtonDeleteComponent } from '../../../assets/customs/ButtonDeleteComponent';
import { ActivatedRoute } from "@angular/router";
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { Etiqueta } from '../../../assets/models/Etiqueta';
import { ProyectoTipo } from './model/ProyectoTipo'
import { DialogProyectoPropiedad, DialogOverviewProyectoPropiedad } from '../../../assets/modals/proyectopropiedad/modal-proyecto-propiedad';
import { DialogDelete, DialogOverviewDelete } from '../../../assets/modals/deleteconfirmation/confirmation-delete';

@Component({
  selector: 'app-peptipo',
  templateUrl: './peptipo.component.html',
  styleUrls: ['./peptipo.component.css']
})
export class PeptipoComponent implements OnInit {
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
  etiqueta : Etiqueta;
  totalProyectotipos : number;
  busquedaGlobal : string;
  proyectotipo : ProyectoTipo;
  paginaActual : number;
  esNuevo : boolean;
  modalProyectoPropiedad : DialogOverviewProyectoPropiedad;
  propiedades = [];
  modalDelete: DialogOverviewDelete;

  @ViewChild('search') divSearch: ElementRef;

  constructor(private route: ActivatedRoute, private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog) { 
    this.etiqueta = JSON.parse(localStorage.getItem("_etiqueta"));
    this.sourcePropiedades = new LocalDataSource();
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.totalProyectotipos = 0;
    this.busquedaGlobal = null;
    this.proyectotipo = new ProyectoTipo();
    this.modalProyectoPropiedad = new DialogOverviewProyectoPropiedad(dialog);
    this.modalDelete = new DialogOverviewDelete(dialog);
  }

  ngOnInit() {
    this.mostrarcargando = true;
    this.obtenerTotalProyectotipos();
  }

  obtenerTotalProyectotipos(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      t: new Date().getTime()      
    };

    this.http.post('http://localhost:60068/api/ProyectoTipo/NumeroProyectoTipos', data, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.totalProyectotipos = response['totalproyectotipos'];
        this.paginaActual = 1;
        if(this.totalProyectotipos > 0){
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
      numeroproyectotipo: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      ordenDireccion: null,
      t:moment().unix()
    };

    this.http.post('http://localhost:60068/api/ProyectoTipo/ProyectoTipoPagina', filtro, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        var data = response['proyectotipos'];

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
    if(this.proyectotipo.id > 0){
      this.esColapsado = true;
      this.esNuevo = false;

      var data = {
        pagina : null,
        idProyectoTipo: this.proyectotipo.id
      }
      this.http.post('http://localhost:60067/api/ProyectoPropiedad/ProyectoPropiedadPaginaPorTipoProy', data, { withCredentials : true }).subscribe(response =>{
        if(response['success'] == true){
          this.propiedades = response['proyectopropiedades'];
          this.sourcePropiedades = new LocalDataSource(this.propiedades);
        }
      })
    }
    else{
      this.utils.mensaje("warning", "Debe seleccionar un Tipo de " + this.etiqueta.proyecto + " que desea editar");
    }
  }

  borrar(){
    if(this.proyectotipo.id > 0){
      this.modalDelete.dialog.open(DialogDelete, {
        width: '600px',
        height: '200px',
        data: { 
          id: this.proyectotipo.id,
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar el tipo de ' + this.etiqueta.proyecto + ' ' +this.proyectotipo.nombre + "?",
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result == true){
          this.http.delete('http://localhost:60068/api/ProyectoTipo/ProyectoTipo/'+ this.proyectotipo.id, { withCredentials : true }).subscribe(response =>{
            if(response['success'] == true){
              this.obtenerTotalProyectotipos();
            }
        })
          
        }
      })
    }
    else{
      this.utils.mensaje("warning", "Seleccione el tipo de " + this.etiqueta.proyecto + " que desea borrar");
    }
  }

  filtrar(campo){
    this.busquedaGlobal = campo;
    this.obtenerTotalProyectotipos();
  }

  onDblClickRow(event){
    this.proyectotipo = event.data;
    this.editar();
  }

  onSelectRow(event){
    this.proyectotipo = event.data;
  }

  handlePage(event){
    this.cargarTabla(event.pageIndex+1);
  }

  guardar(){
    if(this.proyectotipo != null){
      this.proyectotipo.propiedades = "";
      this.propiedades = [];

      this.sourcePropiedades.getAll().then(value =>{
        value.forEach(element => {
          this.propiedades.push(element); 
        });

        for(var x=0; x < this.propiedades.length; x++){
          this.proyectotipo.propiedades = this.proyectotipo.propiedades + (x > 0 ? "," : "") + this.propiedades[x].id;
        }

        var objetoHttp;

        if(this.proyectotipo.id > 0){
          objetoHttp = this.http.put("http://localhost:60068/api/ProyectoTipo/Proyectotipo/" + this.proyectotipo.id, this.proyectotipo, { withCredentials: true });
        }
        else{
          objetoHttp = this.http.post("http://localhost:60068/api/ProyectoTipo/Proyectotipo", this.proyectotipo, { withCredentials: true });
        }
  
        objetoHttp.subscribe(response =>{
          if(response['success'] == true){
            this.proyectotipo.usuarioCreo = response['usuarioCreo'];
            this.proyectotipo.fechaCreacion = response['fechaCreacion'];
            this.proyectotipo.fechaActualizacion = response['fechaActualizacion'];
            this.proyectotipo.usuarioActualizo = response['usuarioActualizo'];
            this.proyectotipo.id = response['id'];
  
            this.esNuevo = false;
            this.obtenerTotalProyectotipos();
            this.utils.mensaje("success", "Tipo de " + this.etiqueta.proyecto + " guardado con éxito");
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
    this.proyectotipo = new ProyectoTipo();
    this.sourcePropiedades = new LocalDataSource();
  }

  buscarPropiedades(){
    this.modalProyectoPropiedad.dialog.open(DialogProyectoPropiedad,{
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
    noDataMessage: 'Cargando, por favor espere...',
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
