import { Component, OnInit } from '@angular/core';
import { utils } from 'protractor';
import { AuthService } from '../../auth.service';
import { UtilsService } from '../../utils.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  isLoggedIn : boolean;
  isMasterPage : boolean;

  constructor(private auth: AuthService, private utils: UtilsService, private http: HttpClient) { }

    ngOnInit() {
        this.isMasterPage = this.auth.isLoggedIn();
        this.utils.setIsMasterPage(this.isMasterPage);
        if(localStorage.getItem("_etiqueta") == null)
          this.getEtiqueta();
    }

    getEtiqueta(){
      var usuario;
      this.http.get('http://localhost:60091/api/Usuario/Usuario',{ withCredentials: true}).subscribe((response)=>{
        if(response['success']==true){
          usuario = response['usuario'];

          this.http.get('http://localhost:60091/api/Usuario/SistemasUsuario',{ withCredentials: true}).subscribe((response2)=>{
            if(response['success']==true){
              var data = response2['etiquetas'];
              for(var i=0; i< data.length; i++){
                if(data[i].id==usuario.sistemaUsuario){
                  localStorage.setItem("_etiqueta", JSON.stringify(data[i]));
                }
              }            
            }
          });
        }
      });
  }
}
