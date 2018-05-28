import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { HttpParams, HttpClient, HttpHeaders, HttpErrorResponse, HttpResponse  } from '@angular/common/http';
import { ViewEncapsulation } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router} from '@angular/router';

import { AuthService} from '../../auth.service';
import { UtilsService } from '../../utils.service';


@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

    @Input() username : string;
    @Input() password : string;
    showerror : boolean;
    _loginUrl = 'http://localhost:59999/api/Login/In'; 
    sistemaNombre : string;
    isMasterPage: boolean;
    isLoggedIn: boolean;

    constructor(private _http: HttpClient, private router : Router, 
        private auth : AuthService, private utils: UtilsService) { 

    }

    ngOnInit() { 
        window.document.title =  this.utils.getSistemaNombre() + ' - Login';
        this.username = "";
        this.password = "";
        this.showerror = false;
        this.sistemaNombre = this.utils.getSistemaNombre()
        this.isMasterPage = false;
        this.utils.setIsMasterPage(false);
    }

    login(){ 
        if(this.username!='' && this.password!=''){
          var data = { username: this.username, password: this.password};
          this._http.post(this._loginUrl, data, { withCredentials: true}).subscribe((response)=>{
                if(response['success']==true){
                    this.isLoggedIn = true;
                    this.isMasterPage = true;
                    this.auth.setJWT(response['jwt']);
                    window.location.href = '/main';
                }
                else{
                    this.showerror = true;
                    this.isLoggedIn = false;
                    this.auth.logoff();
                    this.utils.setIsMasterPage(false);
                }
            }, 
            reponse => {
                this.showerror = true;
                this.isLoggedIn = false;
                this.auth.logoff();
                this.utils.setIsMasterPage(false);
            },
            () =>
            {
            }
         );
       }
    }
}
