import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HttpClient } from '@angular/common/http';
import { LocalDataSource } from 'ng2-smart-table';

@Component({
  templateUrl: 'modal-dialog.html'
})
export class DialogOverviewTipoPrestamo {
  constructor(public dialog: MatDialog) {}
}

@Component({
  selector: 'modal-tipo-prestamo.ts',
  templateUrl: 'modal-dialog.html'
})
export class DialogTipoPrestamo {
  totalCodigos : number;
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

  constructor(public dialog: MatDialog,
    public dialogRef: MatDialogRef<DialogTipoPrestamo>,
    @Inject(MAT_DIALOG_DATA) public data: any, private http: HttpClient) {
      this.elementosPorPagina = 8;
    }

  ngOnInit() { 
    this.obtenerTotalTiposPrestamos();
  }

  Ok(): void {    
    this.data = { tipoPrestamoNombre : this.nombre, tipoPrestamoId : this.id };
    this.dialogRef.close(this.data);
  }

  obtenerTotalTiposPrestamos(){
    this.esColapsado = false;
    var filtro = {};
    this.http.post('http://localhost:60057/api/PrestamoTipo/numeroPrestamoTipos',filtro, {withCredentials: true}).subscribe(response => {
      if (response['success'] == true) {   
        this.totalCodigos = response["totalprestamotipos"];
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
      numeroproyectotipos: this.elementosPorPagina
    }
    this.http.post('http://localhost:60057/api/PrestamoTipo/PrestamoTipoPagina', filtro, { withCredentials: true }).subscribe(response => {
      if (response['success'] == true) {
        var data = response["proyectotipos"];        
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
    this.data = { tipoPrestamoNombre : this.nombre, tipoPrestamoId : this.id };
    this.dialogRef.close(this.data);
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
}