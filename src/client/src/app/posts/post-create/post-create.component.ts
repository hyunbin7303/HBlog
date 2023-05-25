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
    })
  }
  ngOnInit(): void {
  }
  createPosts(): void {
    this.postService.createPost(this.model).subscribe({
      next: (result) => {
        if(result == "Success")
          this.toastr.success("Post created successfully.");
        else {
          this.toastr.error(result);
        }
      },
      error: error => {
        this.toastr.error(error.error)
      }
    })
  }


  cancel() {
    
  }
}
