import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Post } from 'src/app/_models/post';
import { PostsService } from 'src/app/_services/posts.service';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-post-edit',
  templateUrl: './post-edit.component.html',
  styleUrls: ['./post-edit.component.css']
})
export class PostEditComponent implements OnInit {
  post: Post | undefined;
  user: User | null = null;
  typeList = [{ value: 'programming', display: 'Programming' }, { value: 'architecture', display: 'Architecture' }]

  @ViewChild('editForm') editForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private postService: PostsService, private accountService: AccountService, private toastr: ToastrService, private route: ActivatedRoute)  {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    })
  }


  ngOnInit(): void {
    this.loadPost();
  }
  loadPost() {
    const postid = this.route.snapshot.paramMap.get('id');
    var postId: number = Number(postid);
    this.postService.getPostById(postId).subscribe({
      next: post => {
        if (post) {
          if(post.userName == this.user?.username)
            this.post = post;
        }
      }
    });
  }
  updatePost() {
    this.postService.updatePost(Number(this.post?.id), this.editForm?.value).subscribe({
      next: _ => {
        this.toastr.success("Post updated successfully.");
        this.editForm?.reset(this.post);
      }
    })
  }
  onAddPostTag(){

  }
}
