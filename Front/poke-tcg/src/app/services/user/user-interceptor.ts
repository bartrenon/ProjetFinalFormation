import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { UserService } from './user-service';

export const UserInterceptor: HttpInterceptorFn = (req, next) => 
{
  const userService = inject(UserService);
  const token = userService.getToken();

  if (token) {
    const clonedReq = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
    return next(clonedReq);
  }

  return next(req);
}
