import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CardListingService } from '../../../services/card-listing-service';
import { CardListing } from '../../../models/Listing/card-listing';
import { UserService } from '../../../services/user/user-service';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-my-listing',
  imports: [RouterLink, DecimalPipe],
  templateUrl: './my-listing.html',
  styleUrl: './my-listing.scss',
})
export class MyListing {
   private _listingService = inject(CardListingService);
  private _authService = inject(UserService);
 
  listings = signal<CardListing[]>([]);
  isLoading = signal(false);
  error = signal<string | null>(null);
 
  constructor() {
    const userId = this._authService.getCurrentUserId();
    if (userId !== null) {
      this.loadListings(userId);
    } else {
      this.error.set('Vous devez être connecté pour voir vos annonces.');
    }
  }
 
  loadListings(sellerId: number): void {
    this.isLoading.set(true);
    this._listingService.getBySeller(sellerId).subscribe({
      next: (result) => {
        this.listings.set(result.listings);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err?.error ?? 'Impossible de charger vos annonces.');
        this.isLoading.set(false);
      },
    });
  }
 
  remove(listing: CardListing): void {
    if (!confirm(`Retirer l'annonce "${listing.cardName ?? listing.cardId}" ?`)) return;
 
    this._listingService.delete(listing.listingId).subscribe({
      next: () => {
        this.listings.update((list) =>
          list.filter((l) => l.listingId !== listing.listingId)
        );
      },
      error: (err) => this.error.set(err?.error ?? 'Suppression impossible.'),
    });
  }
}
