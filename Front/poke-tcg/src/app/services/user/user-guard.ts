import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { UserService } from "./user-service";
import { map, take } from "rxjs";

export const  UserGuard : CanActivateFn = () => {

  const userService = inject(UserService);
  const router = inject(Router);

  return userService.isAuthenticated$.pipe(
    take(1),
    map(isAuth => {
      if (!isAuth) {
        router.navigate(['/login']);
        return false;
      }
      return true;
    })
  );
}
