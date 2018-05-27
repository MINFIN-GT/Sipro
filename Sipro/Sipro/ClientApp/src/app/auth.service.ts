import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthService {

  _loginUrl = 'http://localhost:59999/api'; 
  claims:string[];
  JWThelper = new JwtHelperService();

  constructor(private http: HttpClient) { }

  public isLoggedIn() {
    var token = localStorage.getItem('jwt');
    if(token!=null && token.length>0){
      if(!this.JWThelper.isTokenExpired(token))
        return true;
      else
        this.logoff();
    }
    return false;
  }

  public logoff(){
    localStorage.removeItem("jwt");
    localStorage.removeItem("claims");
  }

  public setJWT(val: string){
    localStorage.setItem("jwt", val);
    localStorage.setItem("claims",JSON.stringify(this.JWThelper.decodeToken(val)['sipro/permission']));
  }

  public hasClaim(claim:string){
    this.claims = JSON.parse(localStorage.getItem("claims"));
    return this.claims.indexOf(claim) > -1;
  }
}
