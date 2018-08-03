import { Component, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    templateUrl: './mapa.html'
})

export class DialogOverviewMapa {
    constructor(public dialog: MatDialog) {}
}

@Component({
    selector: 'modal-carga-mapa.ts',
    templateUrl: './mapa.html'
})
export class DialogMapa {
    positions = [];
    lat: number;
    lng: number;
    LatLng = {};

    constructor(public dialog: MatDialog, public dialogRef: MatDialogRef<DialogMapa>,
        @Inject(MAT_DIALOG_DATA) public data: any) {
            dialogRef.disableClose = true;
            
        }
    
    ngOnInit() { 

    }

    aceptar(){
        if(this.lat != null && this.lng != null)
            this.dialogRef.close({ success : true, latitud : this.lat, longitud : this.lng });
        else
            this.dialogRef.close({ success : false });
    }

    cancelar(){
        this.dialogRef.close({ success : false });
    }

    onMapReady(map) {
        console.log('map', map);
        console.log('markers', map.markers);  // to get all markers as an array 
    }

    onIdle(event) {
        console.log('map', event.target);
    }

    onMarkerInit(marker) {
        console.log('marker', marker);
    }

    onMapClick(event) {
        this.positions = [];
        if(this.positions.length < 1){           
            this.lat = event.latLng.lat();
            this.lng= event.latLng.lng();
            this.positions.push(event.latLng);
        }
    }

    mapOptions ={
        zoom: 14,
        center: "14.628031216467617, -90.51299594697338",
        mapTypeId: "satellite",
        disableDefaultUI: true
    }
}