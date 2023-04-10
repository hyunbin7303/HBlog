import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Post } from 'src/app/_models/post';
import { PostsService } from 'src/app/_services/posts.service';

@Component({
  selector: 'app-post-detail',
  templateUrl: './post-detail.component.html',
  styleUrls: ['./post-detail.component.css']
})
export class PostDetailComponent {
  post: Post | undefined;
  constructor(private postService: PostsService, private route: ActivatedRoute) { }

}
