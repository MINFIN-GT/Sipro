import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HttpClient } from '@angular/common/http';
import { DialogEntidad, DialogOverviewEntidad } from '../entidad/modal-entidad';

@Component({
  templateUrl: './modal-dialog.html',
})
export class DialogOverviewImpacto {
  constructor(public dialog: MatDialog) {}
}

@Component({
  selector: 'modal-impacto.ts',
  templateUrl: './modal-dialog.html'
})
export class DialogImpacto {
    entidad : number;
    nombre: string;
    modalDialogEntidad : DialogOverviewEntidad;
    impacto: Impacto;

  constructor(public dialog: MatDialog,
    public dialogRef: MatDialogRef<DialogImpacto>,
    @Inject(MAT_DIALOG_DATA) public data: any, private http: HttpClient) {
      this.modalDialogEntidad = new DialogOverviewEntidad(dialog);
      this.impacto = new Impacto();
    }

  ngOnInit() { }

  Ok(): void {    
    this.data = { nombre : this.impacto.entidadNombre, id : this.impacto.entidadId, impacto: this.impacto.impacto };
    this.dialogRef.close(this.data);
  }

  buscarEntidad(){
    this.modalDialogEntidad.dialog.open(DialogEntidad, {
      width: '600px',
      height: '585px',
      data: { titulo: 'Entidad' }
    }).afterClosed().subscribe(result =>{
      if(result != null){
        this.impacto.entidadNombre = result.nombre;
        this.impacto.entidadId = result.id;
      }
    })
  }

  cancelar(){
    this.dialogRef.close();
  }
}

export class Impacto{
  entidadNombre : string;
  entidadId : number;
  impacto : string;
}