import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HttpClient } from '@angular/common/http';

@Component({
    templateUrl: 'documento-adjunto.html'
})

export class DialogOverviewDownloadDocument {
    constructor(public dialog: MatDialog) {}
}

@Component({
    selector: 'documento-adjunto.ts',
    templateUrl: 'documento-adjunto.html'
})
export class DialogDownloadDocument {
    documentos : File;
    idObjeto: string;
    idTipoObjeto: string;

    constructor(public dialog: MatDialog,
        public dialogRef: MatDialogRef<DialogDownloadDocument>,
        @Inject(MAT_DIALOG_DATA) public data: any, private http: HttpClient,) {
            this.idObjeto = data.idObjeto;
            this.idTipoObjeto = data.idTipoObjeto;
        }

    ngOnInit() { 

    }

    AgregarDocumento(){
        if(this.documentos != null){
            var formData = new FormData();
            formData.append("file", this.documentos);
            this.http.post('http://localhost:60021/api/DocumentoAdjunto/Documento/' + this.idObjeto + '/' + this.idTipoObjeto,formData,{ withCredentials: true }).subscribe(response => {
                if (response['success'] == true){
                    this.dialogRef.close(response["documentos"]);
                }
                else{
                    if(response['existe_archivo'])
                        alert("Ya existe el archivo");
                    else
                        alert("Error al subir el archivo");
                }
                    
            })
        }        
    }

    cargarDocumento(event){
        this.documentos = event.currentTarget.files[0]; 
    }

    cancelar(){
        this.dialogRef.close();
    }
}