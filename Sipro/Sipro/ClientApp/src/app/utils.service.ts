import { Injectable, EventEmitter, Output } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class UtilsService {

    sistemaNombre : string = 'SIPRO';
    _isMasterPage = false;
    _isLoggedIn = false;
    _elementosPorPagina = 20;
    _numeroMaximoPaginas = 5;

    @Output() changeMasterPage : EventEmitter<boolean> = new EventEmitter();

    constructor(private http: HttpClient) { 

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
}
