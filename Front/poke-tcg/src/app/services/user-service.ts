import { UserCreate } from '../models/user/userCreate';
import { UserLogin } from '../models/user/userLogin';

import { HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly _url = 'https://localhost:7009/apiTcg/User';

  constructor(private _http: HttpClient) {}

  createUser(user: UserCreate): Observable<UserCreate> {
    return this._http.post<UserCreate>(`${this._url}/register`, user);
  }

  login(credentials: UserLogin): Observable<UserLogin> {
    return this._http.post<UserLogin>(`${this._url}/login`, credentials);
  }
}
