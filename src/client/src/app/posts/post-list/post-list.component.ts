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
  posts$: Observable<Post[]> | undefined;
  user: User | null = null;

  constructor(private postsService: PostsService, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    })
   }
  ngOnInit(): void {
    this.posts$ = this.postsService.getPosts();
  }
}
