import { Component } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../Services/auth.service';
import { Login } from '../../Models/Login';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  form = this.fb.group({
    name: new FormControl(null, [Validators.required]),
    password: new FormControl(null, [Validators.required])
  });

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) { }

  login() {
    if (this.form.invalid) return;
    
    const login: Login = {
      name: this.form.value.name!,
      password: this.form.value.password!
    }

    this.authService.login(login).subscribe(jwt => {
      localStorage.setItem('access_token', jwt.token);
      this.router.navigate(['/home']);
    });
  }

}
