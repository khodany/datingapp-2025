import { inject, Injectable } from '@angular/core';
import { AccountService } from './account-service';
import { Observable, of } from 'rxjs';
import { ɵNullViewportScroller } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class InitService {
  private accountServices = inject(AccountService);

  init() {
    const userString = localStorage.getItem('user');

    if (!userString) return of(null);
    const user = JSON.parse(userString);
    this.accountServices.currentUser.set(user);

    return of(null)
  }
}
