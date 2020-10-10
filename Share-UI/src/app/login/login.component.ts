import { AlertifyService } from './../services/alertify.service';
import { AuthService } from './../services/auth.service';
import { User } from './../models/user';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();
  user: User;
  loginForm: FormGroup;

  constructor(private authService: AuthService, private alertify: AlertifyService,
              private fb: FormBuilder, private router: Router) {
                if (this.authService.loggedIn()){
                  this.router.navigate(['/dashboard']);
                }
              }

  ngOnInit() {
    this.createRegisterForm();
  }

  createRegisterForm(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]]
    });
  }

  isInvalid(element: string): boolean {
    return this.loginForm.get(element).errors && this.loginForm.get(element).touched;
  }

  hasError(element: string, error: string): boolean {
    return this.loginForm.get(element).hasError(error) && this.loginForm.get(element).touched;
  }

  isPasswordMismatch(): boolean {
    return this.loginForm.hasError('mismatch') && this.loginForm.get('confirmPassword').touched;
  }

  login(): void {
    this.authService.login(this.loginForm.value).subscribe(next => {
      this.alertify.success('logged in successfully');
    }, error => {
      localStorage.removeItem('token');
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['/dashboard']);
    });
  }

  cancel(): void {
    this.cancelRegister.emit(false);
  }

}
