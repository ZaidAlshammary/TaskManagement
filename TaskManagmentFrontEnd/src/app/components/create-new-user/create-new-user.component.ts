import { Component } from '@angular/core';
import { AuthService } from '../../Services/auth.service';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { Login } from '../../Models/Login';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-create-new-user',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './create-new-user.component.html',
  styleUrl: './create-new-user.component.css'
})
export class CreateNewUserComponent {

  form = this.fb.group({
    name: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])
  })

  constructor(
    private matDialogRef: MatDialogRef<CreateNewUserComponent>,
    private fb: FormBuilder,
    private authService: AuthService
  ) { }

  createUser() {
    const user: Login = {
      name: this.form.get("name")!.value!,
      password: this.form.get("password")!.value!
    }

    this.authService.createUser(user).subscribe(user => {
      this.matDialogRef.close(user);
    });
  }
}
