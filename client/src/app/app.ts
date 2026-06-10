import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { Nav } from "../layout/nav/nav";
import { AccountService } from '../core/services/account-service';
import { Home } from "../features/home/home";
import { User } from '../types/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [Nav, Home]
})
export class App implements OnInit {
  
  private accountServices = inject(AccountService);

  private http = inject(HttpClient);
  protected title = 'Dating app';
  protected members = signal<User[]>([])

   async ngOnInit() {

        this.members.set(await this.getMembers())
        this.setCuurentUser();
    } 

   
    

 setCuurentUser(){
  const  userString = localStorage.getItem('user');

  if(!userString) return;
  const user = JSON.parse(userString);
  this.accountServices.currentUser.set(user);
 }

  async getMembers(): Promise<User[]> {
    
    try {
          return lastValueFrom(this.http.get<User[]>('https://localhost:5001/api/members'));

    } catch (error) {
      
       console.log(error);
       return [];
    }
  }
  }




