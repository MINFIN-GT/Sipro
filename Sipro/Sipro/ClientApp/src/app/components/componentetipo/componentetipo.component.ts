import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ButtonDeleteComponent } from '../../../assets/customs/ButtonDeleteComponent';
import { ActivatedRoute } from "@angular/router";
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { ComponenteTipo } from './model/ComponenteTipo';
import { DialogComponentePropiedad, DialogOverviewComponentePropiedad } from '../../../assets/modals/componentepropiedad/modal-componente-propiedad';
import { DialogDelete, DialogOverviewDelete } from '../../../assets/modals/deleteconfirmation/confirmation-delete';

@Component({
  selector: 'app-componentetipo',
  templateUrl: './componentetipo.component.html',
  styleUrls: ['./componentetipo.component.css']
})
export class ComponentetipoComponent implements OnInit {
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
  totalComponentetipos : number;
  busquedaGlobal : string;
  componentetipo : ComponenteTipo;
  paginaActual : number;
  esNuevo : boolean;
  modalComponentePropiedad : DialogOverviewComponentePropiedad;
  propiedades = [];
  modalDelete: DialogOverviewDelete;

  @ViewChild('search') divSearch: ElementRef;

  constructor(private route: ActivatedRoute, private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog) { 
    this.sourcePropiedades = new LocalDataSource();
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.totalComponentetipos = 0;
    this.busquedaGlobal = null;
    this.componentetipo = new ComponenteTipo();
    this.modalComponentePropiedad = new DialogOverviewComponentePropiedad(dialog);
    this.modalDelete = new DialogOverviewDelete(dialog);
  }

  ngOnInit() {
    this.mostrarcargando = true;
    this.obtenerTotalComponentestipos();
  }

  obtenerTotalComponentestipos(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      t: new Date().getTime()      
    };

    this.http.post('http://localhost:60014/api/ComponenteTipo/NumeroComponenteTipos', data, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.totalComponentetipos = response['totalcomponentetipos'];
        this.paginaActual = 1;
        if(this.totalComponentetipos > 0){
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
      numeroComponenteTipo: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      ordenDireccion: null,
      t:moment().unix()
    };

    this.http.post('http://localhost:60014/api/ComponenteTipo/ComponentetiposPagina', filtro, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        var data = response['componentetipos'];

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
    if(this.componentetipo.id > 0){
      this.esColapsado = true;
      this.esNuevo = false;

      this.http.get('http://localhost:60013/api/ComponentePropiedad/ComponentePropiedadPorTipo/'+ this.componentetipo.id, { withCredentials : true }).subscribe(response =>{
        if(response['success'] == true){
          this.propiedades = response['componentepropiedades'];
          this.sourcePropiedades = new LocalDataSource(this.propiedades);
        }
      })
    }
    else{
      this.utils.mensaje("warning", "Debe seleccionar un Tipo de componente que desea editar");
    }
  }

  borrar(){
    if(this.componentetipo.id > 0){
      this.modalDelete.dialog.open(DialogDelete, {
        width: '600px',
        height: '200px',
        data: { 
          id: this.componentetipo.id,
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar el tipo de componente?',
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result == true){
          this.http.delete('http://localhost:60014/api/ComponenteTipo/ComponenteTipo/'+ this.componentetipo.id, { withCredentials : true }).subscribe(response =>{
            if(response['success'] == true){
              this.obtenerTotalComponentestipos();
            }
        })
          
        }
      })
    }
    else{
      this.utils.mensaje("warning", "Seleccione el tipo de componente que desea borrar");
    }
  }

  filtrar(campo){
    this.busquedaGlobal = campo;
    this.obtenerTotalComponentestipos();
  }

  onDblClickRow(event){
    this.componentetipo = event.data;
    this.editar();
  }

  onSelectRow(event){
    this.componentetipo = event.data;
  }

  handlePage(event){
    this.cargarTabla(event.pageIndex+1);
  }

  guardar(){
    if(this.componentetipo != null){
      this.componentetipo.propiedades = "";
      this.propiedades = [];

      this.sourcePropiedades.getAll().then(value =>{
        value.forEach(element => {
          this.propiedades.push(element); 
        });

        for(var x=0; x < this.propiedades.length; x++){
          this.componentetipo.propiedades = this.componentetipo.propiedades + (x > 0 ? "," : "") + this.propiedades[x].id;
        }

        var objetoHttp;

        if(this.componentetipo.id > 0){
          objetoHttp = this.http.put("http://localhost:60014/api/ComponenteTipo/ComponenteTipo/" + this.componentetipo.id, this.componentetipo, { withCredentials: true });
        }
        else{
          objetoHttp = this.http.post("http://localhost:60014/api/ComponenteTipo/ComponenteTipo", this.componentetipo, { withCredentials: true });
        }
  
        objetoHttp.subscribe(response =>{
          if(response['success'] == true){
            this.componentetipo.usuarioCreo = response['usuarioCreo'];
            this.componentetipo.fechaCreacion = response['fechaCreacion'];
            this.componentetipo.fechaActualizacion = response['fechaActualizacion'];
            this.componentetipo.usuarioActualizo = response['usuarioActualizo'];
            this.componentetipo.id = response['id'];
  
            this.esNuevo = false;
            this.obtenerTotalComponentestipos();
            this.utils.mensaje("success", "Tipo de componente guardado con éxito");
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
    this.componentetipo = new ComponenteTipo();
    this.sourcePropiedades = new LocalDataSource();
  }

  buscarPropiedades(){
    this.modalComponentePropiedad.dialog.open(DialogComponentePropiedad,{
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
