import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Post } from '../_models/post';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getPosts() {
    return this.http.get<Post[]>(this.baseUrl + 'posts', this.getHttpOptions())
  }
  getMember(username: string) {
    return this.http.get<Post>(this.baseUrl + 'posts/' + username, this.getHttpOptions());
  }
  getHttpOptions() {
    const userString = localStorage.getItem('user');
    if (!userString) return;

    const user = JSON.parse(userString);
    return {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + user.token
      })
    }
  }
}
