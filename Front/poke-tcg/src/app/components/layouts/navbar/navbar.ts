import { Component, inject, signal, computed } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive, NavigationEnd, ActivatedRoute } from '@angular/router';
import { filter, map, startWith } from 'rxjs';
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
  isInfoMenuOpen = signal(false);

  private _userService = inject(UserService);
  private _router = inject(Router);
  private _route = inject(ActivatedRoute);

  private currentUrl = toSignal(
    this._router.events.pipe(
      filter((event) => event instanceof NavigationEnd),
      map((event) => (event as NavigationEnd).urlAfterRedirects),
      startWith(this._router.url)
    ),
    { initialValue: this._router.url }
  );

  public searchPlaceholder = computed(() => {
    const url = this.currentUrl();

    if (url.startsWith('/sets')) return 'Rechercher un set';
    if (url.startsWith('/set/') || url.startsWith('/cards')) return 'Rechercher une carte';

    return 'Rechercher';
  });

  public isSearchDisabled = computed(() => {
  const url = this.currentUrl();

  return url.startsWith('/profil') || url.startsWith('/card/');
  });

  onSearch() {
    const term = this.searchTerm().trim();

    if (this.isSearchDisabled()) return;
    
    this._router.navigate([], {
      relativeTo: this._route,
      queryParams: { q: term || null },
      queryParamsHandling: 'merge',
    });
  }

  onLogout() {
    this._userService.logout();
    this._router.navigate(['/login']);
  }
}