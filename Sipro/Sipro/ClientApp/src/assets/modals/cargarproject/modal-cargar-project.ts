import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HttpClient } from '@angular/common/http';

@Component({
    templateUrl: './modal-dialog.html'
})

export class DialogOverviewCargarProject {
    constructor(public dialog: MatDialog) {}
}

@Component({
    selector: 'modal-cargar-project.ts',
    templateUrl: './modal-dialog.html'
})
export class DialogCargarProject {
    id : number;
    titulo : string;
    textoCuerpo : string;
    textoBotonOk: string;
    textoBotonCancelar: string;
    color = 'primary';
    mode = 'indeterminate';
    value = 50;
    diameter = 45;
    strokewidth = 3;
    mostrarcargando: boolean;
    nombreArchivo: string;
    documento: File;

    constructor(public dialog: MatDialog,
        public dialogRef: MatDialogRef<DialogCargarProject>,
        @Inject(MAT_DIALOG_DATA) public data: any, private http: HttpClient) {
            this.id = data.id;
            this.titulo = data.titulo;
            this.textoCuerpo = data.textoCuerpo;
            this.textoBotonOk = data.textoBotonOk;
            this.textoBotonCancelar = data.textoBotonCancelar;
        }

    ngOnInit() { 
        this.mostrarcargando = false;
    }

    aceptar(){
        if(this.documento != null){
            var formData = new FormData();
            formData.append("file", this.documento);
            this.http.post('http://localhost:60030/api/Gantt/Importar/'+0+'/'+0+'/'+1+'/'+1, formData, { withCredentials : true }).subscribe(response => {
                if(response['success'] == true){
                    this.dialogRef.close(true);   
                }
            })
        }           
    }

    cancelar(){
        this.dialogRef.close(false);
    }

    cargarArchivo(event){
        this.documento = event.currentTarget.files[0];
        this.nombreArchivo = this.documento.name;
    }
}