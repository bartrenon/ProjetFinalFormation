import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CardServices } from '../../../services/card-services';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CardWithPagination } from '../../../models/card/card-with-pagination';
import { ImageUrlService } from '../../../services/tools/image-url-service';

@Component({
  selector: 'app-list-cards',
  imports: [RouterLink],
  templateUrl: './list-cards.html',
  styleUrl: './list-cards.scss',
})
export class ListCards {
  
  private _cardService = inject(CardServices);
  private _route = inject(ActivatedRoute);
  public _imageUrlService = inject(ImageUrlService);

  cardsWithPagination = signal<CardWithPagination>({cards: [],totalCards: 0});
  page = signal(1);
  pageSize = this._cardService.pageSize;
  isLoading = signal(false);
  error = signal<string | null>(null);
  searchQuery = signal('');

  totalPages = computed(() => {
    const total = Number(this.cardsWithPagination().totalCards);
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
      .pipe(takeUntilDestroyed())
      .subscribe((params) => {
        this.searchQuery.set(params.get('q') ?? '');
        this.loadCards(this.page());
      });
  }

  goToPage(newPage: number): void {
    const total = this.totalPages();
    if (newPage < 1 || newPage > total || newPage === this.page()) {
      return;
    }
    this.loadCards(newPage);
  }

  previousPage(): void {
    this.goToPage(this.page() - 1);
  }

  nextPage(): void {
    this.goToPage(this.page() + 1);
  }

  loadCards(pageNumber: number): void {
    this.isLoading.set(true);
    this._cardService.getCards(pageNumber, this.searchQuery()).subscribe({
      next: (cards) => {
        this.cardsWithPagination.set(cards);
        this.page.set(pageNumber);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err);
        this.isLoading.set(false);
      }
    });
  }
}
