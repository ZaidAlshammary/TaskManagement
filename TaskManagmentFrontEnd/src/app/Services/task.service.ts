import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Login } from '../Models/Login';
import { TaskPayload } from '../Models/CreateTask';
import { Status, Task } from '../Models/Task';
import { AppResponse } from './AppResponse';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  serviceUrl = `${environment.apiBaseUrl}/tasks`;

  constructor(private http: HttpClient) { }

  getTasks(){
    return this.http.get<AppResponse>(this.serviceUrl)
    .pipe(map(res => res.data as Task[]));
  }

  getTask(taskId: number){
    return this.http.get<AppResponse>(`${this.serviceUrl}/${taskId}`)
    .pipe(map(res => res.data as Task));
  }

  addTask(task: TaskPayload): Observable<any> {
    return this.http.post<any>(`${this.serviceUrl}/create`, task);
  }
  
  deleteTask(taskId: number): Observable<any> {
    return this.http.delete<any>(`${this.serviceUrl}/${taskId}/delete`);
  }

  editTask(taskId: number, task: TaskPayload): Observable<any> {
    return this.http.put<any>(`${this.serviceUrl}/${taskId}/update`, task);
  }

  reAssignTask(taskId: number, userId: string): Observable<any> {
    return this.http.put<any>(`${this.serviceUrl}/${taskId}/reassign/${userId}`, null);
  }

  changeTaskStatus(taskId: number, status: string): Observable<any> {
    return this.http.put<any>(`${this.serviceUrl}/${taskId}/status/${status}`, null);
  }
  
  getTaskStatus(status: string){
    switch(status){
      case "0":
        return Status.pending;
      case "1":
        return Status.completed;
      default:
        return Status.pending;
    }
  }
}
