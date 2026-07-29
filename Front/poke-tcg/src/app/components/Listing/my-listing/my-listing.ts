import { Component, computed, inject, signal } from '@angular/core';
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
  private readonly userId = this._authService.getCurrentUserId();
 
  listings = signal<CardListing[]>([]);
  totalListings = signal(0);
  page = signal(1);
  pageSize = this._listingService.pageSize;
  isLoading = signal(false);
  error = signal<string | null>(null);

  totalPages = computed(() => Math.max(1, Math.ceil(this.totalListings() / this.pageSize)));
 
  constructor() {
    if (this.userId !== null) {
      this.loadListings(1);
    } else {
      this.error.set('Vous devez être connecté pour voir vos annonces.');
    }
  }
 
  loadListings(pageNumber: number): void {
    if (this.userId === null) return;
    this.isLoading.set(true);
    this.error.set(null);
    this._listingService.getBySeller(this.userId, pageNumber).subscribe({
      next: (result) => {
        this.listings.set(result.listings);
        this.totalListings.set(Number(result.totalListings));
        this.page.set(pageNumber);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err?.error?.message ?? err?.error ?? 'Impossible de charger vos annonces.');
        this.isLoading.set(false);
      },
    });
  }
 
  remove(listing: CardListing): void {
    if (!confirm(`Retirer l'annonce "${listing.cardName ?? listing.cardId}" ?`)) return;
 
    this._listingService.delete(listing.listingId).subscribe({
      next: () => {
        const nextPage = this.listings().length === 1 && this.page() > 1
          ? this.page() - 1
          : this.page();
        this.loadListings(nextPage);
      },
      error: (err) => this.error.set(err?.error?.message ?? err?.error ?? 'Suppression impossible.'),
    });
  }

  previousPage(): void {
    if (this.page() > 1) this.loadListings(this.page() - 1);
  }

  nextPage(): void {
    if (this.page() < this.totalPages()) this.loadListings(this.page() + 1);
  }
}
