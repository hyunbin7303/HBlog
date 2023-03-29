import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit{
  model: any = {}


  constructor(public accountService: AccountService, private router: Router) {}
  
  ngOnInit(): void {
  }
  login() {
    this.accountService.login(this.model).subscribe({
      next: _ => this.router.navigateByUrl('/members'),
      error: error => console.log(error)
    })
  }
  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

}
