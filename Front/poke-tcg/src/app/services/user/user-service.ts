import { UserCreate } from '../../models/user/userCreate';
import { UserLogin } from '../../models/user/userLogin';
import { HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { UserJwt } from '../../models/user/user-jwt';


@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly TOKEN_KEY = 'auth_token';
  private readonly REFRESH_TOKEN_KEY = 'refresh_token';
  private readonly _url = 'https://localhost:7009/apiTcg/User';

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasToken());
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(private _http: HttpClient) {}

  createUser(user: UserCreate): Observable<UserCreate> {
    return this._http.post<UserCreate>(`${this._url}/register`, user);
  }

  login(credentials: UserLogin): Observable<UserJwt> {
    return this._http.post<UserJwt>(`${this._url}/login`, credentials)
      .pipe(
        tap((response: UserJwt) => {
          this.setTokens(response);
          this.isAuthenticatedSubject.next(true);
        })
      );
  }

  refreshToken(): Observable<UserJwt> {
    const refreshToken = this.getRefreshToken();

    return this._http.post<UserJwt>(`${this._url}/refresh-token`, { refreshToken })
      .pipe(
        tap((response: UserJwt) => {
          this.setTokens(response);
        })
      );
  }

  logout(): void {
    const refreshToken = this.getRefreshToken();

    if (refreshToken) {
      this._http.post(`${this._url}/revoke-token`, { refreshToken }).subscribe({
        error: () => {}
      });
    }

    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
    this.isAuthenticatedSubject.next(false);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  private setTokens(response: UserJwt): void {
    localStorage.setItem(this.TOKEN_KEY, response.accessToken);
    localStorage.setItem(this.REFRESH_TOKEN_KEY, response.refreshToken);
  }

  private hasToken(): boolean {
    return !!localStorage.getItem(this.TOKEN_KEY);
  }

}
