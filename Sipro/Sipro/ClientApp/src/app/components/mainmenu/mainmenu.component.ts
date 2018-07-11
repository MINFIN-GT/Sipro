import { Component, OnInit } from '@angular/core';
import { UtilsService } from '../../utils.service';
import { AuthService } from '../../auth.service';
import { MatToolbar } from '@angular/material';
import { HttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { Etiqueta } from '../../../assets/models/Etiqueta';

@Component({
  selector: 'main-menu',
  templateUrl: './mainmenu.component.html',
  styleUrls: ['./mainmenu.component.css']
})
export class MainmenuComponent implements OnInit {

    isMasterPage : boolean;
    etiqueta : Etiqueta;

    constructor(private utils : UtilsService, private auth : AuthService, private http: HttpClient) { 
        this.isMasterPage = utils.isMasterPage();
        this.etiqueta = JSON.parse(localStorage.getItem("_etiqueta"));
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

    irPrestamo(){
      window.location.href = '/prestamo';
    }

    getEntidades(){
      this.http.get('http://localhost:60064/api/Proyecto/Proyecto/1', { withCredentials: true}).subscribe( response =>{
            if(response['success']==true){
              console.log(response['entidades']);
            }
            else{
                console.log('Error al hacer logout');
            }
      });

      /*var data = {  codigo: 11101, fechaCreacion:"01/01/2018", fechaActualizacion:"01/01/2018", descripcion: "prueba 1", ejercicio: 2017, estado: 1, nombre: "prueba 1", siglas: "LS", usuarioCreo : "admin", usuarioActualizo : "admin"};

      this.http.post('http://localhost:60015/api/Cooperante/Cooperante', data, { withCredentials: true }).subscribe(response => {
        if (response['success'] == true) {
          console.log(response['unidadesEjecutoras']);
        } else {
          console.log('Error');
        }
      },
        reponse => {
          this.auth.logoff();
          this.utils.setIsMasterPage(false);
        },
        () => {
        }
      );*/
    }

}
