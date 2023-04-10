import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Post } from 'src/app/_models/post';
import { PostsService } from 'src/app/_services/posts.service';

@Component({
  selector: 'app-post-detail',
  templateUrl: './post-detail.component.html',
  styleUrls: ['./post-detail.component.css']
})
export class PostDetailComponent implements OnInit {
  post: Post | undefined;
  constructor(private postService: PostsService, private route: ActivatedRoute) { }
  ngOnInit(): void {
    this.loadPost();
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
