import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Post } from '../_models/post';
import { Observable, catchError, map, of, take, throwError } from 'rxjs';
import { ServiceResult } from '../_models/serviceresult';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  baseUrl = environment.apiUrl;
  posts: Post[] = [];

  constructor(private http: HttpClient) { }
  getPosts() : Observable<Post[]> {
    return this.http.get<Post[]>(this.baseUrl + 'posts')
  }
  getPostByUserName(username: string): Observable<Post> {
    return this.http.get<Post>(this.baseUrl + 'posts/username/' + username);
  }
  getPostById(id: number): Observable<Post> {
    return this.http.get<Post>(this.baseUrl + 'posts/' + id).pipe(catchError(this.handleError));
  }
  updatePost(id:number, post: Post) {
    post.id = id;
    return this.http.put(this.baseUrl + 'posts/', post).pipe(
      map(() => {
        const index = this.posts.indexOf(post);
        this.posts[index] = { ...this.posts[index], ...post }
      })
    )
  }
  createPost(model: any): Observable<string> {
    return this.http.post<string>(this.baseUrl + 'posts', model).pipe(
      map(post => {
        if (post == "Success") {
          return post;
        }
        else{
          return "Failure";
        }
      })
    )
  }
  deletePost(id: number): Observable<boolean> {
    return this.http.delete<boolean>(this.baseUrl + 'posts/' + id).pipe(catchError(this.handleError));
  }
  private handleError(error: HttpErrorResponse) {
    console.error('server error:', error);
    if(error.error instanceof Error) {
      let errMsg = error.error.message;
      return throwError(() => new Error(errMsg));
    }
    return throwError(()=> new Error(error.message || 'Server Error.'));
  }
}
