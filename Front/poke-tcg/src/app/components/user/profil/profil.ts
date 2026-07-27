import { Component, inject, OnInit, signal } from '@angular/core';
import { UserService } from '../../../services/user/user-service';
import { UserSummary } from '../../../models/user/user-summary';
import { DatePipe, DecimalPipe } from '@angular/common';
import { CardPriceService } from '../../../services/card-price-service';

@Component({
  selector: 'app-profil',
  imports: [DatePipe, DecimalPipe],
  templateUrl: './profil.html',
  styleUrl: './profil.scss',
})
export class Profil implements OnInit{

  private _userService = inject(UserService);
  private _priceCardService = inject(CardPriceService);

  data = signal<UserSummary>({username: "",email: "", createdAt:  new Date()});
  isLoading = signal(false);
  error = signal<string | null>(null);
  valuePriceTotal = signal(0);

  ngOnInit(): void {
    this.loadData();
    this.loadPrice();
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

  loadPrice(): void {
    this._priceCardService.getTotalValue().subscribe({
      next: (price) => {
        this.valuePriceTotal.set(price);
      },
      error: (err) => {
        console.error('Impossible de charger la valeur du set.', err)
      }
    });
  }
}
