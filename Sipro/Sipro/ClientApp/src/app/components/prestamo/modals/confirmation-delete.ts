import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HttpClient } from '@angular/common/http';

@Component({
    templateUrl: '../../../../assets/modals/dialogconfirmation/confirmation-dialog.html'
})

export class DialogOverviewDelete {
    constructor(public dialog: MatDialog) {}
}

@Component({
    selector: 'confirmation-delete.ts',
    templateUrl: '../../../../assets/modals/dialogconfirmation/confirmation-dialog.html'
})
export class DialogDelete {
    titulo : string;
    textoCuerpo : string;
    textoBotonOk: string;
    textoBotonCancelar: string;

    constructor(public dialog: MatDialog,
        public dialogRef: MatDialogRef<DialogDelete>,
        @Inject(MAT_DIALOG_DATA) public data: any, private http: HttpClient,) {
            this.titulo = data.titulo;
            this.textoCuerpo = data.textoCuerpo;
            this.textoBotonOk = data.textoBotonOk;
            this.textoBotonCancelar = data.textoBotonCancelar;
        }

    ngOnInit() { 

    }

    aceptar(){
        
    }

    cancelar(){
        this.dialogRef.close();
    }
}