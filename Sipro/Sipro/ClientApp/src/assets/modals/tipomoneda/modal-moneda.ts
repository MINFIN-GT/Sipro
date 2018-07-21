import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';

@Component({
  templateUrl: './modal-dialog.html'
})
export class DialogOverviewMoneda {
  constructor(public dialog: MatDialog) {}
}

@Component({
  selector: 'modal-moneda.ts',
  templateUrl: './modal-dialog.html'
})
export class DialogMoneda {
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

  constructor(public dialog: MatDialog,
    public dialogRef: MatDialogRef<DialogMoneda>,
    @Inject(MAT_DIALOG_DATA) public data: any, private http: HttpClient) {
      this.elementosPorPagina = 10;
      this.busquedaGlobal = null;
    }

  ngOnInit() { 
    this.obtenerTotalMonedas();
  }

  Ok(): void {    
    this.data = { tipoMonedaNombre : this.nombre, tipoMonedaId : this.id };
    this.dialogRef.close(this.data);
  }

  obtenerTotalMonedas(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal
    };
    this.esColapsado = false;
    this.http.post('http://localhost:60088/api/TipoMoneda/numeroTipoMonedas',data, {withCredentials: true}).subscribe(response => {
      if (response['success'] == true) {   
        this.totalElementos = response["totalTipoMonedas"];
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
      numeroTipoMoneda: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal
    }
    this.http.post('http://localhost:60088/api/TipoMoneda/TipoMonedaPagina', filtro, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        var data = response["tipoMonedas"];        
        this.source = data;
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
    this.nombre = event.data.nombre;
    this.id = event.data.id;
  }

  onDblClickRow(event){
    this.data = { tipoMonedaNombre : this.nombre, tipoMonedaId : this.id };
    this.dialogRef.close(this.data);
  }

  settings = {
   columns: {
      id: {
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
    hideSubHeader: true
  };

  handlePage(event){
    this.esColapsado = false;
    this.cargarTabla(event.pageIndex+1);
  }

  filtrar(campo){  
    this.busquedaGlobal = campo;
    this.obtenerTotalMonedas();
  }
}