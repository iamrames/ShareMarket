import { AuthService } from './../services/auth.service';
import { AlertifyService } from './../services/alertify.service';
import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root'
})

export class AuthGuard implements CanActivate {

    constructor(private authservice: AuthService, private router: Router, private alertify: AlertifyService) {}

    canActivate(next: ActivatedRouteSnapshot): boolean {
      if (this.authservice.loggedIn()) {
        return true;
      }

      this.alertify.error('You shall not pass!!!');
      this.router.navigate(['/home']);
      return false;
    }
  }