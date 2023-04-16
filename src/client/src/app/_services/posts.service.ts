import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Post } from '../_models/post';
import { Observable, catchError, map, of, take, throwError } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  user: User | undefined;
  baseUrl = environment.apiUrl;
  posts: Post[] = [];

  constructor(private http: HttpClient, private accountService: AccountService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) {
          this.user = user;
        }
      }
    })
  }
  getPosts() : Observable<Post[]> {
    return this.http.get<Post[]>(this.baseUrl + 'posts')
  }
  getPostByUserName(username: string): Observable<Post> {
    return this.http.get<Post>(this.baseUrl + 'posts/username/' + username);
  }
  getPostById(id: number): Observable<Post> {
    return this.http.get<Post>(this.baseUrl + 'posts/' + id).pipe(catchError(this.handleError));
  }
  updatePost(post: Post) {
    return this.http.put(this.baseUrl + 'posts', post).pipe(
      map(() => {
        const index = this.posts.indexOf(post);
        this.posts[index] = { ...this.posts[index], ...post }
      })
    )
  }
  createPost(model: any) {

    return this.http.post<Post>(this.baseUrl + 'posts', model).pipe(
      map(post => {
        if (post) {
          localStorage.setItem('post', JSON.stringify(post));
          // this.currentUserSource.next(post);
        }
        return post;
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
