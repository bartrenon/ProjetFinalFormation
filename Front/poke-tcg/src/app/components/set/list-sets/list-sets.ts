import { Component, computed, inject, signal } from '@angular/core';
import { SetService } from '../../../services/set-service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { SetWithPagination } from '../../../models/set/set-with-pagination';

@Component({
  selector: 'app-list-sets',
  imports: [RouterLink],
  templateUrl: './list-sets.html',
  styleUrl: './list-sets.scss',
})
export class ListSets {

  private _setService = inject(SetService);
  private _route = inject(ActivatedRoute);

  setsWithPagination = signal<SetWithPagination>({sets: [],totalSets: 0});
  page = signal(1);
  pageSize = this._setService.pageSize;
  isLoading = signal(false);
  error = signal<string | null>(null);
  extension = signal('webp');
  searchQuery = signal('');

   totalPages = computed(() => {
    const total = Number(this.setsWithPagination().totalSets);
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
        this.loadSets(this.page());
      });
  }

  goToPage(newPage: number): void {
    const total = this.totalPages();
    if (newPage < 1 || newPage > total || newPage === this.page()) {
      return;
    }
    this.loadSets(newPage);
  }

   previousPage(): void {
    this.goToPage(this.page() - 1);
  }

  nextPage(): void {
    this.goToPage(this.page() + 1);
  }

  loadSets(pageNumber: number): void {
    this.isLoading.set(true);
    this._setService.getAllSets(pageNumber, this.searchQuery()).subscribe({
      next: (sets) => {
        this.setsWithPagination.set(sets);
        this.page.set(pageNumber);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err);
        this.isLoading.set(false);
      }
    });
  }

  getSetLogoUrl(logoUrl?: string): string {
    if (!logoUrl) return '';
    return `${logoUrl}.${this.extension()}`;
  }

  getSetSymbolUrl(symbolUrl?: string): string {
    if (!symbolUrl) return '';
    return `${symbolUrl}.${this.extension()}`;
  }
}