import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CardListingService } from '../../../services/card-listing-service';
import { ImageUrlService } from '../../../services/tools/image-url-service';
import { UserService } from '../../../services/user/user-service';
import { CardListing } from '../../../models/Listing/card-listing';
import { ListingStatus } from '../../../models/Listing/listing-status';
import { DatePipe, DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-detail-listing',
  imports: [RouterLink, DatePipe, DecimalPipe],
  templateUrl: './detail-listing.html',
  styleUrl: './detail-listing.scss',
})
export class DetailListing {

  private _listingService = inject(CardListingService);
  private _route = inject(ActivatedRoute);
  private _router = inject(Router);
  private _authService = inject(UserService);
  public _imageUrlService = inject(ImageUrlService);
 
  listing = signal<CardListing | null>(null);
  isLoading = signal(false);
  isBuying = signal(false);
  error = signal<string | null>(null);
 
  readonly ListingStatus = ListingStatus;
 
  isOwner = computed(() => {
    const l = this.listing();
    const userId = this._authService.getCurrentUserId();
    return l !== null && userId !== null && l.sellerId === userId;
  });
 
  canBuy = computed(() => {
    const l = this.listing();
    return l !== null && l.status === ListingStatus.Active && !this.isOwner();
  });
 
  constructor() {
    const idParam = this._route.snapshot.paramMap.get('id');
    const id = Number(idParam);
    if (idParam !== null && Number.isInteger(id) && id > 0) {
      this.loadListing(id);
    } else {
      this.error.set('Identifiant d’annonce invalide.');
    }
  }
 
  loadListing(id: number): void {
    this.isLoading.set(true);
    this.error.set(null);
    this._listingService.getById(id).subscribe({
      next: (listing) => {
        this.listing.set(listing);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err?.error?.message ?? err?.error ?? 'Annonce introuvable.');
        this.isLoading.set(false);
      },
    });
  }
 
 buy(): void {
    const l = this.listing();
    if (!l) return;
 
    this.isBuying.set(true);
    this._listingService.buy(l.listingId).subscribe({
      next: () => {
        this.isBuying.set(false);
        this.loadListing(l.listingId);
      },
      error: (err) => {
        this.error.set(err?.error ?? "Impossible d'acheter cette carte.");
        this.isBuying.set(false);
      },
    });
  }
 
  delete(): void {
    const l = this.listing();
    if (!l) return;
    if (!confirm('Retirer cette annonce ?')) return;
 
    this._listingService.delete(l.listingId).subscribe({
      next: () => this._router.navigate(['/my-listings']),
      error: (err) => this.error.set(err?.error ?? 'Suppression impossible.'),
    });
  }
}
