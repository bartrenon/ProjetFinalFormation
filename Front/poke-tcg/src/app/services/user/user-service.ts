import { UserCreate } from '../../models/user/userCreate';
import { UserLogin } from '../../models/user/userLogin';
import { HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';


@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly TOKEN_KEY = 'auth_token';
  private readonly _url = 'https://localhost:7009/apiTcg/User';

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasToken());
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(private _http: HttpClient) {}

  createUser(user: UserCreate): Observable<UserCreate> {
    return this._http.post<UserCreate>(`${this._url}/register`, user);
  }

  login(credentials: UserLogin): Observable<UserLogin> {
    return this._http.post<UserLogin>(`${this._url}/login`, credentials)
      .pipe(
        tap((response: any) => {
          this.setToken(response.token); 
          this.isAuthenticatedSubject.next(true);
        })
      );
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    this.isAuthenticatedSubject.next(false);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  private setToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  private hasToken(): boolean {
    return !!localStorage.getItem(this.TOKEN_KEY);
  }

}
