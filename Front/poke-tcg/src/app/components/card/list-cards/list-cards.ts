import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Card } from '../../../models/card/card';
import { CardServices } from '../../../services/card-services';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-list-cards',
  imports: [RouterLink],
  templateUrl: './list-cards.html',
  styleUrl: './list-cards.scss',
})
export class ListCards implements OnInit {
  
  private _cardService = inject(CardServices);
  private _route = inject(ActivatedRoute);

  cards = signal<Card[]>([]);
  isLoading = signal(false);
  error = signal<string | null>(null);
  extension = signal('webp');
  searchQuery = signal('');

  ngOnInit(): void {
    this.loadCards();
  }

   constructor() {
    this._route.queryParamMap
      .pipe(takeUntilDestroyed())
      .subscribe((params) => {
        this.searchQuery.set(params.get('q') ?? '');
        this.loadCards();
      });
  }

  loadCards(): void {
    this.isLoading.set(true);
    this._cardService.getCards(this.searchQuery()).subscribe({
      next: (cards) => {
        this.cards.set(cards);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err);
        this.isLoading.set(false);
      }
    });
  }

   getCardImageUrl(imageUrl?: string): string {
    if (!imageUrl) return '';
    return `${imageUrl}/high.${this.extension()}`;
  }
}
