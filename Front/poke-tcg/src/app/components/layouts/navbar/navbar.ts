import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { UserService } from '../../../services/user/user-service';

interface Generation {
  label: string;
  slug: string;
}

@Component({
  selector: 'app-navbar',
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {
  public searchTerm = signal('');
  public isMenuOpen = signal(false);

  private _userService = inject(UserService);
  private _router = inject(Router);

  onSearch() {
    
  }

  onLogout() {
    this._userService.logout();
    this._router.navigate(['/login']);
  }
}