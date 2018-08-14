import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ButtonDeleteComponent } from '../../../assets/customs/ButtonDeleteComponent';
import { ActivatedRoute } from "@angular/router";
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { SubcomponenteTipo } from './model/SubcomponenteTipo';
import { DialogDelete, DialogOverviewDelete } from '../../../assets/modals/deleteconfirmation/confirmation-delete';
import { DialogOverviewSubComponentePropiedad, DialogSubComponentePropiedad } from '../../../assets/modals/subcomponentepropiedad/modal-subcomponente-propiedad';

@Component({
  selector: 'app-subcomponentetipo',
  templateUrl: './subcomponentetipo.component.html',
  styleUrls: ['./subcomponentetipo.component.css']
})
export class SubcomponentetipoComponent implements OnInit {
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
  totalSubComponentetipos : number;
  busquedaGlobal : string;
  subcomponentetipo : SubcomponenteTipo;
  paginaActual : number;
  esNuevo : boolean;
  modalSubComponentePropiedad : DialogOverviewSubComponentePropiedad;
  propiedades = [];
  modalDelete: DialogOverviewDelete;

  constructor(private route: ActivatedRoute, private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog) { 
    this.sourcePropiedades = new LocalDataSource();
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.totalSubComponentetipos = 0;
    this.busquedaGlobal = null;
    this.subcomponentetipo = new SubcomponenteTipo();
    this.modalSubComponentePropiedad = new DialogOverviewSubComponentePropiedad(dialog);
    this.modalDelete = new DialogOverviewDelete(dialog);
  }

  ngOnInit() {
    this.mostrarcargando = true;
    this.obtenerTotalSubComponentestipos();
  }

  obtenerTotalSubComponentestipos(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      t: new Date().getTime()      
    };

    this.http.post('http://localhost:60082/api/SubcomponenteTipo/NumeroSubComponenteTipos', data, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        this.totalSubComponentetipos = response['totalsubcomponentetipos'];
        this.paginaActual = 1;
        if(this.totalSubComponentetipos > 0){
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
      numerosubcomponentetipos: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      ordenDireccion: null,
      t:moment().unix()
    };

    this.http.post('http://localhost:60082/api/SubcomponenteTipo/SubComponentetiposPagina', filtro, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        var data = response['subcomponentetipos'];

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
    if(this.subcomponentetipo.id > 0){
      this.esColapsado = true;
      this.esNuevo = false;

      this.http.get('http://localhost:60081/api/SubcomponentePropiedad/SubComponentePropiedadPorTipo/' + this.subcomponentetipo.id, { withCredentials : true }).subscribe(response =>{
        if(response['success'] == true){
          this.propiedades = response['subcomponentepropiedades'];
          this.sourcePropiedades = new LocalDataSource(this.propiedades);
        }
      })
    }
    else{
      this.utils.mensaje("warning", "Debe seleccionar un Tipo de subcomponente que desea editar");
    }
  }

  borrar(){
    if(this.subcomponentetipo.id > 0){
      this.modalDelete.dialog.open(DialogDelete, {
        width: '600px',
        height: '200px',
        data: { 
          id: this.subcomponentetipo.id,
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar el tipo de subcomponente?',
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result != null && result == true){
          this.http.delete('http://localhost:60082/api/SubcomponenteTipo/SubComponenteTipo/'+ this.subcomponentetipo.id, { withCredentials : true }).subscribe(response =>{
            if(response['success'] == true){
              this.obtenerTotalSubComponentestipos();
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
    this.obtenerTotalSubComponentestipos();
  }

  onDblClickRow(event){
    this.subcomponentetipo = event.data;
    this.editar();
  }

  onSelectRow(event){
    this.subcomponentetipo = event.data;
  }

  handlePage(event){
    this.cargarTabla(event.pageIndex+1);
  }

  guardar(){
    if(this.subcomponentetipo != null){
      this.subcomponentetipo.propiedades = "";
      this.propiedades = [];

      this.sourcePropiedades.getAll().then(value =>{
        value.forEach(element => {
          this.propiedades.push(element); 
        });

        for(var x=0; x < this.propiedades.length; x++){
          this.subcomponentetipo.propiedades = this.subcomponentetipo.propiedades + (x > 0 ? "," : "") + this.propiedades[x].id;
        }

        var objetoHttp;

        if(this.subcomponentetipo.id > 0){
          objetoHttp = this.http.put("http://localhost:60082/api/SubcomponenteTipo/SubComponenteTipo/" + this.subcomponentetipo.id, this.subcomponentetipo, { withCredentials: true });
        }
        else{
          objetoHttp = this.http.post("http://localhost:60082/api/SubcomponenteTipo/SubComponenteTipo", this.subcomponentetipo, { withCredentials: true });
        }
  
        objetoHttp.subscribe(response =>{
          if(response['success'] == true){
            this.subcomponentetipo.usuarioCreo = response['usuarioCreo'];
            this.subcomponentetipo.fechaCreacion = response['fechaCreacion'];
            this.subcomponentetipo.fechaActualizacion = response['fechaActualizacion'];
            this.subcomponentetipo.usuarioActualizo = response['usuarioActualizo'];
            this.subcomponentetipo.id = response['id'];
  
            this.esNuevo = false;
            this.obtenerTotalSubComponentestipos();
            this.utils.mensaje("success", "Tipo de subcomponente guardado con éxito");
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
    this.subcomponentetipo = new SubcomponenteTipo();
    this.sourcePropiedades = new LocalDataSource();
  }

  buscarPropiedades(){
    this.modalSubComponentePropiedad.dialog.open(DialogSubComponentePropiedad,{
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
