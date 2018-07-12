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
export class DialogDeleteProyectoPropiedad {
    id : number;
    titulo : string;
    textoCuerpo : string;
    textoBotonOk: string;
    textoBotonCancelar: string;

    constructor(public dialog: MatDialog,
        public dialogRef: MatDialogRef<DialogDeleteProyectoPropiedad>,
        @Inject(MAT_DIALOG_DATA) public data: any, private http: HttpClient,) {
            this.id = data.id;
            this.titulo = data.titulo;
            this.textoCuerpo = data.textoCuerpo;
            this.textoBotonOk = data.textoBotonOk;
            this.textoBotonCancelar = data.textoBotonCancelar;
        }

    ngOnInit() { 

    }

    aceptar(){
        this.http.delete('http://localhost:60067/api/ProyectoPropiedad/ProyectoPropiedad/'+ this.id, { withCredentials : true }).subscribe(response =>{
            if(response['success'] == true){
                this.dialogRef.close(true);
            }
        })
    }

    cancelar(){
        this.dialogRef.close();
    }
}