import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../../services/user/user-service';

@Component({
  selector: 'app-navbar',
  imports: [FormsModule, RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {

  public searchTerm = '';
  private _userService = inject(UserService);

  constructor(private router: Router) {}

  onSearch() {
  }

  onLogout() {
    this._userService.logout();
  }
}
