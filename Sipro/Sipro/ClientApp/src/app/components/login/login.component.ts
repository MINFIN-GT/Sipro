import { Component, OnInit, Input } from '@angular/core';
import { HttpParams, HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { ViewEncapsulation } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { Utilities } from  '../../utilities';

@Component({
    selector: 'login',
    encapsulation: ViewEncapsulation.None,
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

    @Input() username : string;
    @Input() password : string;
    showerror : boolean;
    _loginUrl = 'http://localhost:59999/api/Login/In'; 
    template : string;
    sistema_nombre : string;

    constructor(private _http: HttpClient,private utils: Utilities) { 
        window.document.title =  this.utils.sistema_nombre + ' - Login';
        this.username = "";
        this.password = "";
        this.utils.etiquetas = null;
        this.showerror = false;
        this.sistema_nombre = this.utils.sistema_nombre;
        _http.get("/api/Templates/Login", {responseType: 'text'}).subscribe(data => {  
            this.template=data.toString();
        });
    }

    ngOnInit() { 
        
    }

    login(){ 
        if(this.username!='' && this.password!=''){
          var data = { username: this.username, password: this.password};
          this._http.post(this._loginUrl, data).subscribe((response)=>{
            if(response['success']==true)
                window.location.href = '/main';
            else
                this.showerror = true;
            }, 
            reponse => {
              this.showerror = true;
            },
            () =>
            {
            }
         );
       }
    }
}
