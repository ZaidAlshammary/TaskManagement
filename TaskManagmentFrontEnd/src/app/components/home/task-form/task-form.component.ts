import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../Services/auth.service';
import { User } from '../../../Models/User';
import { MatDialog } from '@angular/material/dialog';
import { CreateNewUserComponent } from '../../create-new-user/create-new-user.component';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './task-form.component.html',
  styleUrl: './task-form.component.css'
})
export class TaskFormComponent implements OnInit {
  @Input() form: FormGroup = new FormGroup({});

  CREATE_USER = 'createUser';

  users: User[] = [];

  constructor(
    private authService: AuthService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.authService.getAllUsers()
      .subscribe(users => this.users = users);
  }

  openDialog($event: any) {
    const selected = $event.target.value;
    if (selected !== this.CREATE_USER) {
      return;
    }

    this.dialog.open(CreateNewUserComponent, {
      width: '250px'
    }).afterClosed().subscribe((user: User) => {
      if (user) {
        this.users.push(user);
        this.form.get('user')?.setValue(user.id);
      }
    });
  }
}
