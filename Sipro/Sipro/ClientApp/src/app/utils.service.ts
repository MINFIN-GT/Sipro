import { Injectable, EventEmitter, Output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FlashMessagesService } from 'angular2-flash-messages';

@Injectable()
export class UtilsService {

    sistemaNombre : string = 'SIPRO';
    _isMasterPage = false;
    _isLoggedIn = false;
    _elementosPorPagina = 20;
    _numeroMaximoPaginas = 5;

    @Output() changeMasterPage : EventEmitter<boolean> = new EventEmitter();

    constructor(private http: HttpClient, private _flashMessagesService: FlashMessagesService) { 

    }

    public isMasterPage() {
        return localStorage.getItem("isMasterPage")=='true';
    }
    
    public setIsMasterPage(val: boolean){
        localStorage.setItem("isMasterPage",val+'');
        this._isMasterPage = val;
        this.changeMasterPage.emit(this._isMasterPage);
    }

    public getSistemaNombre(){
        return this.sistemaNombre;
    }

    public cleanStorage(){
        localStorage.clear();
    }

    public mensaje(tipo, texto){
        if(tipo == 'success'){
            this._flashMessagesService.show(texto, { cssClass: 'alert-successm', timeout: 4000 });
        }
        else if(tipo == 'warning'){
            this._flashMessagesService.show(texto, { cssClass: 'alert-warningm', timeout: 4000 });
        }
        else if(tipo == 'danger'){
            this._flashMessagesService.show(texto, { cssClass: 'alert-dangerm', timeout: 4000 });
        }
    }
}
