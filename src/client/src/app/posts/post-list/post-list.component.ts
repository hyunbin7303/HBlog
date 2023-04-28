import { Component, OnInit } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Post } from 'src/app/_models/post';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { PostsService } from 'src/app/_services/posts.service';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.css']
})

export class PostListComponent implements OnInit {
  user: User | null = null;
  posts: Post[] = [];
  isUserEditable: boolean = false;
  constructor(private postsService: PostsService, private accountService: AccountService) {
    this.postsService.getPosts().subscribe({ next: posts => { this.posts = posts; }})
    this.accountService.currentUser$.pipe(take(1)).subscribe({ next: user => { if (user) this.user = user; }}) 
  }
  ngOnInit(): void {
  }
}
