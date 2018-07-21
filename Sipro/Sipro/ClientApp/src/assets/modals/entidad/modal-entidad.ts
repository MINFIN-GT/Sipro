import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';

@Component({
  templateUrl: './modal-dialog.html',
})
export class DialogOverviewEntidad {
  constructor(public dialog: MatDialog) {}
}

@Component({
  selector: 'modal-impacto.ts',
  templateUrl: './modal-dialog.html'
})
export class DialogEntidad {
    totalElementos : number;
    source: LocalDataSource;
    paginaActual : number;
    elementosPorPagina : number;
    entidad : number;
    nombre: string;
    color = 'primary';
    mode = 'indeterminate';
    value = 50;
    diameter = 45;
    strokewidth = 3;
    esColapsado: boolean;
    busquedaGlobal: string;

  constructor(public dialog: MatDialog,
    public dialogRef: MatDialogRef<DialogEntidad>,
    @Inject(MAT_DIALOG_DATA) public data: any, private http: HttpClient) {
      this.elementosPorPagina = 10;
      this.busquedaGlobal = null;
    }

  ngOnInit() { 
    this.obtenerTotalCodigos();
  }

  Ok(): void {    
    this.data = { nombre : this.nombre, id : this.entidad };
    this.dialogRef.close(this.data);
  }

  obtenerTotalCodigos(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal
    };
    this.esColapsado = false;
    this.http.post('http://localhost:60024/api/Entidad/totalEntidades',data,{withCredentials: true}).subscribe(response => {
      if (response['success'] == true) {   
        this.totalElementos = response["total"];
        this.paginaActual = 1;
        this.cargarTabla(this.paginaActual);
      } else {
        console.log('Error');
      }
    });
  }

  cargarTabla(pagina? : number){
    var filtro = {
      pagina: pagina,
      registros: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
    }
    this.http.post('http://localhost:60024/api/Entidad/Entidades', filtro, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        var data = response["entidades"];        
        this.source = new LocalDataSource(data);
        this.esColapsado = true;

        this.source.setSort([
          { field: 'id', direction: 'asc' }  // primary sort
        ]);
      } else {
        console.log('Error');
      }
    });
  }

  cancelar(){
    this.dialogRef.close();
  }

  onSelectRow(event){
    this.entidad = event.data.entidad;
    this.nombre = event.data.nombre;
  }

  onDblClickRow(event){
    this.data = { nombre : this.nombre, id : this.entidad };
    this.dialogRef.close(this.data);
  }

  settings = {
   columns: {
      entidad: {
        title: 'ID',
        width: '10%',
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
      class: 'table table-bordered grid estilo-letra'
    },
    hideSubHeader: true,
    pager:{
        display: false
    }
  };

  handlePage(event){
    this.esColapsado = false;
    this.cargarTabla(event.pageIndex+1);
  }

  filtrar(campo){  
    this.busquedaGlobal = campo;
    this.obtenerTotalCodigos();
  }
}