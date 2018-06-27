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
    
    constructor(public dialog: MatDialog,
        public dialogRef: MatDialogRef<DialogDownloadDocument>,
        @Inject(MAT_DIALOG_DATA) public data: any, private http: HttpClient,) {}

    ngOnInit() { 

    }

    upload(){

    }

    AgregarDocumento(){

    }

    cancelar(){
        this.dialogRef.close();
    }
}