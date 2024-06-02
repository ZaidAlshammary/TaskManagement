import { Component, OnInit } from '@angular/core';
import { TaskService } from '../../../Services/task.service';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { TaskPayload } from '../../../Models/CreateTask';
import { MatSnackBar } from '@angular/material/snack-bar';
import { User } from '../../../Models/User';
import { TaskFormComponent } from '../task-form/task-form.component';

@Component({
  selector: 'app-add-task',
  standalone: true,
  imports: [ReactiveFormsModule, TaskFormComponent],
  templateUrl: './add-task.component.html',
  styleUrl: './add-task.component.css'
})
export class AddTaskComponent {
  addingTask: boolean = false;
  users: User[] = [];

  form = this.fb.group({
    title: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
    dueDate: new FormControl('', [Validators.required]),
    status: new FormControl('', [Validators.required]),
    user: new FormControl('', [Validators.required])
  })

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService
  ) { }

  addTask() {
    const task: TaskPayload = {
      title: this.form.get("title")!.value!,
      description: this.form.get("description")!.value!,
      dueDate: new Date(this.form.get("dueDate")!.value!),
      status: this.taskService.getTaskStatus(this.form.get("status")!.value!),
      assignedTo: this.form.get("user")!.value!
    }

    this.taskService.addTask(task)
      .subscribe(
        () => {
          this.form.reset();
          this.toggleForm();
        })
  }

  toggleForm() {
    this.addingTask = !this.addingTask;
  }

}
