import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';
import * as moment from 'moment';
import { PrestamoTipo } from './model/model.prestamotipo';
import { DialogDeleteTipoPrestamo, DialogOverviewDelete } from './modals/confirmation-delete';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'app-prestamotipo',
  templateUrl: './prestamotipo.component.html',
  styleUrls: ['./prestamotipo.component.css']
})
export class PrestamotipoComponent implements OnInit {
  isLoggedIn : boolean;
  isMasterPage : boolean;
  esColapsado : boolean;
  elementosPorPagina : number;
  numeroMaximoPaginas : number;
  totalPrestamosTipos : number;
  busquedaGlobal: string;
  paginaActual : number;
  source: LocalDataSource;
  prestamotipo: PrestamoTipo;
  esNuevo : boolean;
  modalDelete: DialogOverviewDelete;

  @ViewChild('search') divSearch: ElementRef;

  constructor(private auth: AuthService, private utils: UtilsService, private http: HttpClient, private dialog: MatDialog) { 
    this.isMasterPage = this.auth.isLoggedIn();
    this.utils.setIsMasterPage(this.isMasterPage);
    this.elementosPorPagina = utils._elementosPorPagina;
    this.numeroMaximoPaginas = utils._numeroMaximoPaginas;
    this.totalPrestamosTipos = 0;
    this.prestamotipo = new PrestamoTipo();
    this.modalDelete = new DialogOverviewDelete(dialog);
  }

  ngOnInit() {
    this.obtenerTotalprestamotipos();
  }

  obtenerTotalprestamotipos(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      t:moment().unix()
    };
    this.http.post('http://localhost:60057/api/PrestamoTipo/numeroPrestamoTipos', data, { withCredentials : true }).subscribe(response =>{
      if(response['success'] == true){
        this.totalPrestamosTipos = response['totalprestamotipos'];
        this.paginaActual = 1;
        if(this.totalPrestamosTipos > 0)
          this.cargarTabla(this.paginaActual);
        else
          this.source = new LocalDataSource();
      }
    })
  }

  cargarTabla(pagina? : number){
    var filtro = {
      pagina: pagina,
      numeroprestamostipos: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      columna_ordenada: null,
      t:moment().unix()
    };

    this.http.post('http://localhost:60057/api/PrestamoTipo/PrestamoTipoPagina', filtro, { withCredentials : true }).subscribe(response =>{
      if(response['success'] == true){
        var data = response['prestamostipos'];
        this.source = new LocalDataSource(data);
        this.source.setSort([
          { field: 'id', direction: 'asc' }  // primary sort
        ]);
        this.busquedaGlobal = null;
      }
    })
  }

  nuevo(){
    this.esColapsado = true;
    this.esNuevo = true;
    this.prestamotipo = new PrestamoTipo();
  }

  editar(){
    if(this.prestamotipo.id > 0){
      this.esColapsado = true;
      this.esNuevo = false;
    }
    else
      alert('warning, Debe seleccionar el Tipo de préstamo que desea editar');
  }

  borrar(){
    if(this.prestamotipo.id > 0){
      this.modalDelete.dialog.open(DialogDeleteTipoPrestamo, {
        width: '600px',
        height: '200px',
        data: { 
          id: this.prestamotipo.id,
          titulo: 'Confirmación de Borrado', 
          textoCuerpo: '¿Desea borrar el tipo de préstamo ' + this.prestamotipo.nombre + "?",
          textoBotonOk: 'Borrar',
          textoBotonCancelar: 'Cancelar'
        }
      }).afterClosed().subscribe(result => {
        if(result != null){
          this.obtenerTotalprestamotipos();
        }
      });
    }
    else{
      alert('warning, Seleccione un préstamo');
    }
  }

  refresh(){
    this.busquedaGlobal = null;
    this.divSearch.nativeElement.value = null;
    this.obtenerTotalprestamotipos();
  }

  filtrar(campo){
    this.busquedaGlobal = campo;
    this.obtenerTotalprestamotipos();
  }

  onSelectRow(event) {
    this.prestamotipo = event.data;
  }

  onDblClickRow(event) {
    this.prestamotipo = event.data;
    this.editar();
  }

  guardar(){
    if(this.prestamotipo != null){
      var objetoHttp;

      if(this.prestamotipo.id > 0){
        if(this.prestamotipo.descripcion == null)
          this.prestamotipo.descripcion = "";
        objetoHttp = this.http.put("http://localhost:60057/api/PrestamoTipo/PrestamoTipo/" + this.prestamotipo.id, this.prestamotipo, { withCredentials: true });
      }
      else{
        if(this.prestamotipo.descripcion == null)
          this.prestamotipo.descripcion = "";
        objetoHttp = this.http.post("http://localhost:60057/api/PrestamoTipo/PrestamoTipo", this.prestamotipo, { withCredentials: true });
      }

      objetoHttp.subscribe(response =>{
        if(response['success'] == true){
          this.prestamotipo.usuarioCreo = response['usuarioCreo'];
          this.prestamotipo.fechaCreacion = response['fechaCreacion'];
          this.prestamotipo.fechaActualizacion = response['fechaActualizacion'];
          this.prestamotipo.usuarioActualizo = response['usuarioActualizo'];
          this.prestamotipo.id = response['id'];

          this.esNuevo = false;
          this.obtenerTotalprestamotipos();
        }
      })
    }
  }

  IrATabla(){
    this.esColapsado = false;
    this.prestamotipo = new PrestamoTipo();
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
      class: 'table table-bordered'
    },
    hideSubHeader: true
  };
}
