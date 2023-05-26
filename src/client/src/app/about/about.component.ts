import { Component, OnInit } from '@angular/core';
import { BioService } from '../_services/bio.service';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {
  bio$ = this.bioService.getBio();
  constructor(private bioService: BioService) {}
  ngOnInit(): void {
  }

}
