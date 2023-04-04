import { Component, OnInit } from '@angular/core';
import { Post } from '../_models/post';
import { PostsService } from '../_services/posts.service';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.css']
})
export class PostsComponent implements OnInit {
  posts: Post[] = [];
  constructor(private postsService: PostsService){  }
  ngOnInit(): void {
    this.loadPosts();
  }

  loadPosts(){
    this.postsService.getPosts().subscribe({
      next: posts => this.posts = posts
    })

  }
}
