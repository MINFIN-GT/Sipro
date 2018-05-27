import { Injectable, EventEmitter, Output } from '@angular/core';

@Injectable()
export class UtilsService {

    sistemaNombre : string = 'SIPRO';
    _isMasterPage = false;
    _isLoggedIn = false;

    @Output() changeMasterPage : EventEmitter<boolean> = new EventEmitter();

    constructor() { 

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
