import { Injectable } from '@angular/core';

@Injectable()
export class Utilities {

    sistema_nombre: string = 'SIPRO';
    elementosPorPagina : number = 20;
    numeroMaximoPaginas : number = 5;
    etiquetas: Array<any>;

}