import { Component, OnInit } from '@angular/core';
import { UtilsService } from '../../utils.service';
import { AuthService } from '../../auth.service';
import { MatToolbar } from '@angular/material';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'main-menu',
  templateUrl: './mainmenu.component.html',
  styleUrls: ['./mainmenu.component.css']
})
export class MainmenuComponent implements OnInit {

    isMasterPage : boolean;

    constructor(private utils : UtilsService, private auth : AuthService, private http: HttpClient) { 
        this.isMasterPage = utils.isMasterPage();
    } 

    ngOnInit() {
        this.utils.changeMasterPage.subscribe(
            _isMasterPage=>{
                this.isMasterPage = _isMasterPage;
            });
    }

    ngOnDestroy(){
        this.utils.changeMasterPage.unsubscribe();
    }

    hasClaim(val:string){
        return this.auth.hasClaim(val);
    }

    logoff(){
        this.auth.logoffRemote();
    }

    getEntidades(){
        this.http.get('http://localhost:60005/api/Entidad/EntidadesPorEjercicio/2017', { withCredentials: true}).subscribe( response =>{
            if(response['success']==true){
                console.log(response['entidades']);
            }
            else{
                console.log('Error al hacer logout');
            }
        });
    }

}
