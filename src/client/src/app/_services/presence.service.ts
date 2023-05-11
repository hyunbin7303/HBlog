import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  private onlineUsersSource = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUsersSource.asObservable();

  constructor(private toastr: ToastrService) { }

  createHubConnection(user: User) {

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

      this.hubConnection.start().catch(error => console.log(error));
      this.hubConnection.on('UserIsOnline', username => {
        this.toastr.info(username + ' has connected.');
      })

      this.hubConnection.on('UserIsOffline', username => {
        this.toastr.warning(username + ' has disconnected.');
      })

      this.hubConnection.on('GetOnlineUsers', usernames=> {
        this.onlineUsersSource.next(usernames);
      })
  }


  stopHubConnection(){
    this.hubConnection?.stop().catch(error => console.log(error));
  }
}
