import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Login } from '../Models/Login';
import { JwtAuth } from '../Models/JwtAuth';
import { Observable, catchError, map } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../Models/User';
import { AppResponse } from './AppResponse';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  serviceUrl = `${environment.apiBaseUrl}/auth`;

  constructor(private http: HttpClient) { }

  login(login: Login): Observable<JwtAuth> {
    return this.http.post<JwtAuth>(`${this.serviceUrl}/login`, login);
  }

  getAllUsers(): Observable<User[]> {
    return this.http.get<AppResponse>(`${this.serviceUrl}/users`)
      .pipe(map(res => res.data as User[]));
  }

  createUser(user: Login): Observable<User> {
    return this.http.post<AppResponse>(`${this.serviceUrl}/create`, user)
      .pipe(map(res => res.data as User));
  }
  
}
