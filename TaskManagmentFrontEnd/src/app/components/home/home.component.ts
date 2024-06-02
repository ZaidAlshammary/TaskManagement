import { DatePipe, NgFor } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { LogoutComponent } from '../logout/logout.component';
import { AddTaskComponent } from './add-task/add-task.component';
import { TaskService } from '../../Services/task.service';
import { Task } from '../../Models/Task';
import { AuthService } from '../../Services/auth.service';
import { User } from '../../Models/User';
import { EditTaskComponent } from './edit-task/edit-task.component';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [NgFor, LogoutComponent, AddTaskComponent, EditTaskComponent, DatePipe],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {

  tasks: Task[] = [];
  users: User[] = [];

  taskIdToEdit: number = 0;

  constructor(
    private taskService: TaskService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.taskService.getTasks()
    .subscribe(tasks => this.tasks = tasks);

    this.authService.getAllUsers()
    .subscribe(users => this.users = users);
  }

  reAssignUser(task: number, $event: any) {
    const userId = $event.target.value;
    if (userId) {
      this.taskService.reAssignTask(task, userId).subscribe();
    }
  }

  changeStatus(task: any, $event: any) {
    const status = $event.target.value;
    if (status) {
      this.taskService.changeTaskStatus(task, status).subscribe();
    }
  }

  completeTask(task: any) {
    task.isComplete = true;
  }

  deleteTask(task: any) {
    const confirm = window.confirm(`Are you sure you want to delete task#${task.id}?`);
    if (!confirm) {
      return;
    }

    this.taskService.deleteTask(task.id).subscribe();
  }

  editTask(taskId: number) {
    if (this.taskIdToEdit === taskId) {
      this.taskIdToEdit = 0;
      return;
    }

    this.taskIdToEdit = 0;
    this.taskIdToEdit = taskId;
  }
}
