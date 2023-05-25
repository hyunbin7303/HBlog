import { Component, OnInit } from '@angular/core';
import { PostsService } from '../_services/posts.service';
import { Post } from '../_models/post';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users: any;
  posts: Post[] = [];

  constructor(private postsService: PostsService) {
    this.postsService.getPosts().subscribe({ next: posts => { this.posts = posts; } })

  }
  ngOnInit(): void {
  }

  registerToggle(){
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean){
    this.registerMode = event;
  }
}
