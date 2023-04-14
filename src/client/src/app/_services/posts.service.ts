import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Post } from '../_models/post';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  baseUrl = environment.apiUrl;
  posts: Post[] = [];

  constructor(private http: HttpClient) { }

  getPosts() {
    return this.http.get<Post[]>(this.baseUrl + 'posts')
  }
  getPostByUserName(username: string) {
    return this.http.get<Post>(this.baseUrl + 'posts/username/' + username);
  }
  getPost(postId: number) {
    const post = this.posts.find(x => x.id == postId);
    console.log(post);
    if (post) return of(post);

    return this.http.get<Post>(this.baseUrl + 'posts/' + postId);
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
}
