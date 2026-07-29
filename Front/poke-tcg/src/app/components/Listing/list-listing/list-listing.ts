import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CardListingService } from '../../../services/card-listing-service';
import { ImageUrlService } from '../../../services/tools/image-url-service';
import { CardListingWithPagination } from '../../../models/Listing/card-listing-with-pagination';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DecimalPipe } from '@angular/common';
import { catchError, finalize, of, switchMap } from 'rxjs';

@Component({
  selector: 'app-list-listing',
  imports: [RouterLink, DecimalPipe],
  templateUrl: './list-listing.html',
  styleUrl: './list-listing.scss',
})
export class ListListing {

  private _listingService = inject(CardListingService);
  private _route = inject(ActivatedRoute);
  public _imageUrlService = inject(ImageUrlService);

   listingsWithPagination = signal<CardListingWithPagination>({ listings: [], totalListings: 0 });
  page = signal(1);
  pageSize = this._listingService.pageSize;
  isLoading = signal(false);
  error = signal<string | null>(null);
  searchQuery = signal('');
 
  totalPages = computed(() => {
    const total = Number(this.listingsWithPagination().totalListings);
    return Math.max(1, Math.ceil(total / this.pageSize));
  });
 
  pageNumbers = computed(() => {
    const current = this.page();
    const total = this.totalPages();
    const delta = 2;
 
    const start = Math.max(1, current - delta);
    const end = Math.min(total, current + delta);
 
    const pages: number[] = [];
    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    return pages;
  });
 
  constructor() {
    this._route.queryParamMap
      .pipe(
        takeUntilDestroyed(),
        switchMap((params) => {
          const query = (params.get('q') ?? '').trim();
          this.searchQuery.set(query);
          this.page.set(1);
          this.isLoading.set(true);
          this.error.set(null);

          return this._listingService.getActiveListings(1, query).pipe(
            catchError((err) => {
              this.error.set(this.getErrorMessage(err, 'Impossible de charger les annonces.'));
              return of(null);
            }),
            finalize(() => this.isLoading.set(false))
          );
        })
      )
      .subscribe((result) => {
        if (result) {
          this.listingsWithPagination.set(result);
        }
      });
  }
 
  goToPage(newPage: number): void {
    const total = this.totalPages();
    if (newPage < 1 || newPage > total || newPage === this.page()) {
      return;
    }
    this.loadListings(newPage);
  }
 
  previousPage(): void {
    this.goToPage(this.page() - 1);
  }
 
  nextPage(): void {
    this.goToPage(this.page() + 1);
  }
 
  loadListings(pageNumber: number): void {
    this.isLoading.set(true);
    this.error.set(null);
    this._listingService.getActiveListings(pageNumber, this.searchQuery()).subscribe({
      next: (result) => {
        this.listingsWithPagination.set(result);
        this.page.set(pageNumber);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(this.getErrorMessage(err, 'Impossible de charger les annonces.'));
        this.isLoading.set(false);
      },
    });
  }

  private getErrorMessage(error: unknown, fallback: string): string {
    if (typeof error === 'object' && error !== null && 'error' in error) {
      const response = (error as { error?: unknown }).error;
      if (typeof response === 'string') return response;
      if (typeof response === 'object' && response !== null && 'message' in response) {
        const message = (response as { message?: unknown }).message;
        if (typeof message === 'string') return message;
      }
    }
    return fallback;
  }
}
