import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Post } from 'src/app/_models/post';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { PostsService } from 'src/app/_services/posts.service';

@Component({
  selector: 'app-post-edit',
  templateUrl: './post-edit.component.html',
  styleUrls: ['./post-edit.component.css']
})
export class PostEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;

  post: Post | undefined;
  user: User | null = null;

  constructor(private accountService: AccountService, private postService: PostsService, private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    })
  }


  ngOnInit(): void {
    this.loadPost();

  }
  loadPost() {
    if (!this.user) return;
    // this.postService.getPost(this.user.username).subscribe({
    //   next: post => this.post = member
    // })
  }
}
