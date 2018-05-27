import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, Router } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable()
export class RouteguardService implements CanActivate, CanActivateChild {

  constructor(private auth: AuthService, private router: Router){ }

  canActivate() {
    var isLoggedIn=this.auth.isLoggedIn();
    if (isLoggedIn) {
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }

  canActivateChild() {
    return this.auth.isLoggedIn();
  }

}