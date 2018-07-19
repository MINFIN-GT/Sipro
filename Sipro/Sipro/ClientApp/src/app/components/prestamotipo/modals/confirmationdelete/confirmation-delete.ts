import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HttpClient } from '@angular/common/http';

@Component({
    templateUrl: './confirmation-dialog.html'
})

export class DialogOverviewDelete {
    constructor(public dialog: MatDialog) {}
}

@Component({
    selector: 'confirmation-delete.ts',
    templateUrl: './confirmation-dialog.html'
})
export class DialogDeleteTipoPrestamo {
    id : number;
    titulo : string;
    textoCuerpo : string;
    textoBotonOk : string;
    textoBotonCancelar : string;

    constructor(public dialog: MatDialog,
        public dialogRef: MatDialogRef<DialogDeleteTipoPrestamo>,
        @Inject(MAT_DIALOG_DATA) public data: any, private http: HttpClient,) {
            this.id = data.id;
            this.titulo = data.titulo;
            this.textoCuerpo = data.textoCuerpo;
            this.textoBotonOk = data.textoBotonOk;
            this.textoBotonCancelar = data.textoBotonCancelar;
        }

    ngOnInit() { }

    aceptar(){
        this.http.delete('http://localhost:60057/api/PrestamoTipo/PrestamoTipo/'+ this.id, { withCredentials : true }).subscribe(response =>{
            if(response['success'] == true){
                this.dialogRef.close(true);
            }
            else
                alert('Warning, Error al borrar el tipo de Préstamo');
        })
    }

    cancelar(){
        this.dialogRef.close();
    }
}