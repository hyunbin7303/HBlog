import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getUsersWithRoles() {
    return this.http.get<User[]>(this.baseUrl + 'admin/users-with-roles');
  }
}
 