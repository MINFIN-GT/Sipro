import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';

@Component({
  templateUrl: './modal-dialog.html'
})
export class DialogOverviewCodigoPresupuestario {
  constructor(public dialog: MatDialog) {}
}

@Component({
  selector: 'modal-codigo-presupuestario.ts',
  templateUrl: './modal-dialog.html'
})
export class DialogCodigoPresupuestario {
  totalElementos : number;
  source: LocalDataSource;
  paginaActual : number;
  elementosPorPagina : number;
  codigoPresupuestario : number;
  numeroprestamo: string;
  color = 'primary';
  mode = 'indeterminate';
  value = 50;
  diameter = 45;
  strokewidth = 3;
  esColapsado: boolean;
  busquedaGlobal: string;

  constructor(public dialog: MatDialog,
    public dialogRef: MatDialogRef<DialogCodigoPresupuestario>,
    @Inject(MAT_DIALOG_DATA) public data: any, private http: HttpClient) {
      this.elementosPorPagina = 9;
      this.busquedaGlobal = null;
    }

  ngOnInit() { 
    this.obtenerTotalCodigos();
  }

  Ok(): void {    
    this.data = { codigoPresupuestario : this.codigoPresupuestario, numeroprestamo : this.numeroprestamo };
    this.dialogRef.close(this.data);
  }

  obtenerTotalCodigos(){
    var data = {  
      filtro_busqueda: this.busquedaGlobal
    };
    this.esColapsado = false;
    this.http.post('http://localhost:60016/api/DataSigade/TotalCodigos',data,{withCredentials: true}).subscribe(response => {
      if (response['success'] == true) {   
        this.totalElementos = response["totalCodigos"];
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
      elementosPorPagina: this.elementosPorPagina,
      filtro_busqueda: this.busquedaGlobal,
    }
    this.http.post('http://localhost:60016/api/DataSigade/Codigos', filtro, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        var data = response["prestamo"];        
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
    this.codigoPresupuestario = event.data.codigopresupuestario;
    this.numeroprestamo = event.data.numeroprestamo;
  }

  onDblClickRow(event){
    this.data = { codigoPresupuestario : this.codigoPresupuestario, numeroprestamo : this.numeroprestamo };
    this.dialogRef.close(this.data);
  }

  settings = {
   columns: {
      codigopresupuestario: {
        title: 'ID',
        width: '6%',
        filter: false,
        type: 'html',
        valuePrepareFunction : (cell) => {
          return "<div class=\"datos-numericos\">" + cell + "</div>";
        }
      },
      numeroprestamo: {
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
    this.obtenerTotalCodigos();
  }
}