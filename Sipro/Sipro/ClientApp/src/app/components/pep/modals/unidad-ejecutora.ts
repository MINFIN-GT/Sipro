import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';

@Component({
  templateUrl: '../../../../assets/modals/dialogsearch/modal-dialog.html',
})
export class DialogOverviewUnidadEjecutora {
  constructor(public dialog: MatDialog) {}
}

@Component({
  selector: 'modal-codigo-presupuestario.ts',
  templateUrl: '../../../../assets/modals/dialogsearch/modal-dialog.html'
})
export class DialogUnidadEjecutora {
    totalElementos : number;
    source: LocalDataSource;
    paginaActual : number;
    elementosPorPagina : number;
    id : number;
    nombre: string;
    color = 'primary';
    mode = 'indeterminate';
    value = 50;
    diameter = 45;
    strokewidth = 3;
    esColapsado: boolean;
    busquedaGlobal: string;
    ejercicio: number;
    codigoEntidad: number;

  constructor(public dialog: MatDialog,
    public dialogRef: MatDialogRef<DialogUnidadEjecutora>,
    @Inject(MAT_DIALOG_DATA) public data: any, private http: HttpClient) {
      this.elementosPorPagina = 7;
      this.busquedaGlobal = null;
      this.ejercicio = data.ejercicio;
      this.codigoEntidad = data.entidad;
    }

  ngOnInit() { 
    this.obtenerTotalUnidadesEjecutoras();
  }

  Ok(): void {    
    this.data = { nombre : this.nombre, id : this.id };
    this.dialogRef.close(this.data);
  }

  obtenerTotalUnidadesEjecutoras(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal,
      ejercicio: this.ejercicio,
      entidad: this.codigoEntidad
    };
    this.esColapsado = false;
    this.http.post('http://localhost:60089/api/UnidadEjecutora/TotalElementos',data,{withCredentials: true}).subscribe(response => {
      if (response['success'] == true) {   
        this.totalElementos = response["total"];
        this.paginaActual = 1;
        if(this.totalElementos > 0)
            this.cargarTabla(this.paginaActual);
      } else {
        console.log('Error');
      }
    });
  }

  cargarTabla(pagina? : number){
    var filtro = {
      pagina: pagina,
      numeroproyectotipo: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
      ejercicio: this.ejercicio,
      entidad: this.codigoEntidad,
      registros: 7
    }
    this.http.post('http://localhost:60089/api/UnidadEjecutora/UnidadEjecutoras', filtro, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        var data = response["unidadesEjecutoras"];        
        this.source = new LocalDataSource(data);
        this.esColapsado = true;
      } else {
        console.log('Error');
      }
    });
  }

  cancelar(){
    this.dialogRef.close();
  }

  onSelectRow(event){
    this.id = event.data.unidadEjecutora;
    this.nombre = event.data.nombre;
  }

  onDblClickRow(event){
    this.data = { nombre : this.nombre, id : this.id };
    this.dialogRef.close(this.data);
  }

  settings = {
   columns: {
    unidadEjecutora: {
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
        class: 'align-left'   
      }
    },
    actions: false,
    noDataMessage: 'No se encontró información.',
    attr: {
      class: 'table table-bordered'
    },
    hideSubHeader: true
  };

  handlePage(event){
    this.esColapsado = false;
    this.cargarTabla(event.pageIndex+1);
  }

  filtrar(campo){  
    this.busquedaGlobal = campo;
    this.obtenerTotalUnidadesEjecutoras();
  }
}