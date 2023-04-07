import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { AccountService } from 'src/app/_services/account.service';
import { PostsService } from 'src/app/_services/posts.service';

@Component({
  selector: 'app-post-create',
  templateUrl: './post-create.component.html',
  styleUrls: ['./post-create.component.css']
})
export class PostCreateComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {}
 
  constructor(private accountService: AccountService, private postService: PostsService, private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      // next: user => this.users = user
    })
  }
  ngOnInit(): void {
  }
  createPosts() {
    // this.postService.createPost(this.editForm?.value).subscribe({
    //   next: _ => {
    //     this.toastr.success("Profile updated successfully.");
    //     this.editForm?.reset(this.member);
    //   }
    // })

    this.postService.createPost(this.model).subscribe({
      next: () => {
        this.cancel();
      },
      error: error => {
        this.toastr.error(error.error),
          console.log(error)
      }
    })
  }
  cancel() {
    
  }
}
