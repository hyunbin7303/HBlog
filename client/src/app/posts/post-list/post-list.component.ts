import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Post } from 'src/app/_models/post';
import { PostsService } from 'src/app/_services/posts.service';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.css']
})

export class PostListComponent implements OnInit {
  posts$: Observable<Post[]> | undefined;

  constructor(private postsService: PostsService) { }
  ngOnInit(): void {
    this.posts$ = this.postsService.getPosts();
  }
}
