import { UserCreate } from '../models/user/userCreate';

import { HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root',
})
export class userService {
  private readonly _url : string = '';

  constructor(private _http: HttpClient) {}

  createUser(user: UserCreate) : Observable<UserCreate>
  {
    return this._http.post<UserCreate>(this._url, user);
  }

}
