import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { InterviewQuestionsComponent } from './programming/interview-questions/interview-questions.component';
import { BusinessesComponent } from './businesses/businesses.component';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    MemberDetailComponent,
    ListsComponent,
    MessagesComponent,
    InterviewQuestionsComponent,
    BusinessesComponent
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule, // gives power for Two-way binding
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    BrowserAnimationsModule, BsDropdownModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
