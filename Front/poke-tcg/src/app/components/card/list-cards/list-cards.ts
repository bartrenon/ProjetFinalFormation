import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Card } from '../../../models/card/card';
import { CardServices } from '../../../services/card-services';

@Component({
  selector: 'app-list-cards',
  imports: [RouterLink],
  templateUrl: './list-cards.html',
  styleUrl: './list-cards.scss',
})
export class ListCards implements OnInit {
  
  private _cardService = inject(CardServices);

  cards = signal<Card[]>([]);
  isLoading = signal(false);
  error = signal<string | null>(null);
  extension = signal('webp');

  ngOnInit(): void {
    this.loadCards();
  }

  loadCards(): void {
    this.isLoading.set(true);
    this._cardService.getCards().subscribe({
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
