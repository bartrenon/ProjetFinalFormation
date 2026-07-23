import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, catchError, filter, switchMap, take, throwError } from 'rxjs';
import { UserService } from './user-service';

let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const UserInterceptor: HttpInterceptorFn = (req, next) => {
  const userService = inject(UserService);
  const router = inject(Router);

  const token = userService.getToken();
  const authReq = token
    ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : req;

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      const isAuthEndpoint = req.url.includes('/login') || req.url.includes('/refresh-token');

      if (error.status !== 401 || isAuthEndpoint) {
        return throwError(() => error);
      }

      if (isRefreshing) {
        return refreshTokenSubject.pipe(
          filter(newToken => newToken !== null),
          take(1),
          switchMap(newToken => {
            const retriedReq = req.clone({ setHeaders: { Authorization: `Bearer ${newToken}` } });
            return next(retriedReq);
          })
        );
      }

      isRefreshing = true;
      refreshTokenSubject.next(null);

      return userService.refreshToken().pipe(
        switchMap(response => {
          isRefreshing = false;
          refreshTokenSubject.next(response.accessToken);

          const retriedReq = req.clone({ setHeaders: { Authorization: `Bearer ${response.accessToken}` } });
          return next(retriedReq);
        }),
        catchError(refreshError => {
          isRefreshing = false;
          userService.logout();
          router.navigate(['/login']);
          return throwError(() => refreshError);
        })
      );
    })
  );
};