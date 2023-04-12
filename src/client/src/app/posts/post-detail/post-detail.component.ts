import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { take } from 'rxjs';
import { Post } from 'src/app/_models/post';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { PostsService } from 'src/app/_services/posts.service';

@Component({
  selector: 'app-post-detail',
  templateUrl: './post-detail.component.html',
  styleUrls: ['./post-detail.component.css']
})
export class PostDetailComponent implements OnInit {
  post: Post | undefined;
  username: string = '';
  user: User | null = null;

  constructor(private postService: PostsService,private accountService: AccountService, private route: ActivatedRoute) { }
  
  ngOnInit(): void {

    this.loadPost();
    this.username = this.route.snapshot.paramMap.get('username') as string;
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    })
  }
  loadPost() {
    const postid = this.route.snapshot.paramMap.get('id');
    var postId: number = Number(postid);

    console.log(postId);
    if (!postid) return;

    this.postService.getPost(postId).subscribe({
      next: post => {
        this.post = post;
      }
    });
  }
}
