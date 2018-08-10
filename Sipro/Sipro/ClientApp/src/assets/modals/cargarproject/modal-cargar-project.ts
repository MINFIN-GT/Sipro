import { Component, Inject } from '@angular/core';
import { Subscription } from 'rxjs'
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HttpClient, HttpRequest, HttpResponse, HttpEvent } from '@angular/common/http';

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
    etiqueta: string;
    color = 'primary';
    mode = 'indeterminate';
    value = 50;
    diameter = 20;
    strokewidth = 3;
    mostrarcargando: boolean;
    nombreArchivo: string;
    documento: File;
    proyectoid : number;
    prestamoid : number;
    multiproyecto : boolean;
    progress:number;
    httpEmitter:Subscription;
    httpEvent:HttpEvent<Event>;
    mensaje: string;

    constructor(public dialog: MatDialog,
        public dialogRef: MatDialogRef<DialogCargarProject>,
        @Inject(MAT_DIALOG_DATA) public data: any, private http: HttpClient) {
            this.id = data.id;
            this.titulo = data.titulo;
            this.textoCuerpo = data.textoCuerpo;
            this.textoBotonOk = data.textoBotonOk;
            this.textoBotonCancelar = data.textoBotonCancelar;
            this.proyectoid = data.proyectoid;
            this.prestamoid = data.prestamoid;
            dialogRef.disableClose = true;
            this.progress = 0;
            this.etiqueta = data.etiqueta;
        }

    ngOnInit() { 
        this.mostrarcargando = false;
    }

    aceptar():Subscription{
        if(this.documento != null){
            var formData = new FormData();
            formData.append("file", this.documento);

            const req = new HttpRequest<FormData>('POST', 'http://localhost:60030/api/Gantt/Importar/'+ (this.multiproyecto ? '1' : '0') + '/0/' + this.proyectoid + '/' + this.prestamoid, formData, {
                reportProgress: true,
                withCredentials: true
            })

            return this.httpEmitter = this.http.request(req).subscribe(
                event=>{
                    if(event["type"] == 1){
                        this.mostrarcargando = true;
                        this.progress = (event["loaded"]/event["total"])*100;
                        this.mensaje = "Cargando archivo al servidor";
                    }
                    else if(event["type"]==2 && event["ok"]==true && event["status"]==200){
                        this.dialogRef.close(true);                         
                    }
                    if(this.progress==100)
                        this.mensaje = "Actualizando " + this.etiqueta;
                },
                error=>console.log('Error Uploading',error)                
            )
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