import { Component, Input, OnInit } from '@angular/core';
import { Post } from 'src/app/_models/post';

@Component({
  selector: 'app-post-article',
  templateUrl: './post-article.component.html',
  styleUrls: ['./post-article.component.css']
})
export class PostArticleComponent implements OnInit {
  @Input() post: Post | undefined;
  ngOnInit(): void {
  }
  onRouteToLinkPost() {
    window.location.href = this.post?.linkForPost as string;
  }
  onClickBody() {
    console.log('body click: ' + this.post?.title)
  }
}
