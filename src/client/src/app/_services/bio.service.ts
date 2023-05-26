import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { Bio } from '../_models/bio';

@Injectable({
  providedIn: 'root'
})
export class BioService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getBio() {
    return this.http.get<Bio>('assets/json/bio.json');
  }
}
 