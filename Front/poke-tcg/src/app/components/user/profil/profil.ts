import { Component, inject, OnInit, signal } from '@angular/core';
import { UserService } from '../../../services/user/user-service';
import { UserSummary } from '../../../models/user/user-summary';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-profil',
  imports: [DatePipe],
  templateUrl: './profil.html',
  styleUrl: './profil.scss',
})
export class Profil implements OnInit{

  private _userService = inject(UserService);

  data = signal<UserSummary>({username: "",email: "", createdAt:  new Date()});
  isLoading = signal(false);
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.loadData();
  }

   loadData(): void {
    this.isLoading.set(true);
    this._userService.getById().subscribe({
      next: (data) => {
        this.data.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err);
        this.isLoading.set(false);
      }
    });
  }
}
