import { Component, OnInit, inject, signal } from '@angular/core';
import { SetService } from '../../../services/set-service';
import { Set } from '../../../models/set/set';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-list-sets',
  imports: [RouterLink],
  templateUrl: './list-sets.html',
  styleUrl: './list-sets.scss',
})
export class ListSets implements OnInit {

  private _setService = inject(SetService);
  private _route = inject(ActivatedRoute);

  sets = signal<Set[]>([]);
  isLoading = signal(false);
  extension = signal('webp');
  searchQuery = signal('');

   ngOnInit(): void {
    this.loadSets();
  }

  constructor() {
    this._route.queryParamMap
      .pipe(takeUntilDestroyed())
      .subscribe((params) => {
        this.searchQuery.set(params.get('q') ?? '');
        this.loadSets();
      });
  }

  loadSets(): void {
    this.isLoading.set(true);

    this._setService.getAllSets(this.searchQuery()).subscribe({
      next: (sets) => {
        this.sets.set(sets);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error(err);
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