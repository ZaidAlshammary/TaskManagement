import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { TaskFormComponent } from '../task-form/task-form.component';
import { TaskService } from '../../../Services/task.service';
import { formatDate } from '@angular/common';
import { Task } from '../../../Models/Task';
import { TaskPayload } from '../../../Models/CreateTask';

@Component({
  selector: 'app-edit-task',
  standalone: true,
  imports: [ReactiveFormsModule, TaskFormComponent],
  templateUrl: './edit-task.component.html',
  styleUrl: './edit-task.component.css'
})
export class EditTaskComponent {

  private _taskId: number = 0;

  @Input() set taskId(value: number) {
    this._taskId = value;
    this.getTask(value);
  }

  get taskId() {
    return this._taskId;
  }

  form = this.fb.group({
    title: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
    dueDate: new FormControl('', [Validators.required]),
    status: new FormControl(0, [Validators.required]),
    user: new FormControl('', [Validators.required])
  })

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService
  ) { }

  private getTask(taskId: number) {
    this.taskService.getTask(taskId)
      .subscribe(task => {
        this.populateForm(task);
      });
  }

  private populateForm(task: Task) {
    this.form.get('title')?.setValue(task.title);
    this.form.get('description')?.setValue(task.description);
    this.form.get('status')?.setValue(task.status);
    this.form.get('user')?.setValue(task.assigneeId);
    const dueDate = formatDate(task.dueDate, 'yyyy-MM-dd', 'en-US');
    this.form.get('dueDate')?.setValue(dueDate);
  }

  editTask() {
    const task: TaskPayload = {
      title: this.form.get('title')?.value!,
      description: this.form.get('description')?.value!,
      dueDate: new Date(this.form.get('dueDate')?.value!),
      status: this.form.get('status')?.value!,
      assignedTo: this.form.get('user')?.value!
    }

    this.taskService.editTask(this.taskId, task).subscribe();
    location.reload();
  }
}
