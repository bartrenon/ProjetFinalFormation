import { Component, inject, OnInit, signal } from '@angular/core';
import { CardServices } from '../../../services/card-services';
import { CardSummary } from '../../../models/card/cardSummary';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-detail-card',
  imports: [],
  templateUrl: './detail-card.html',
  styleUrl: './detail-card.scss',
})
export class DetailCard implements OnInit {
  
  private _cardService = inject(CardServices);
  private _route = inject(ActivatedRoute);

  card = signal<CardSummary | null>(null);
  isLoading = signal(false);
  error = signal<string | null>(null);
  extension = signal('webp');

  ngOnInit(): void {
    this.loadCard();
  }

  loadCard(): void {
    const cardId = this._route.snapshot.paramMap.get('id');

    if (!cardId) {
      this.error.set('Aucun identifiant de carte fourni.');
      return;
    }

    this.isLoading.set(true);

    this._cardService.getCardById(cardId).subscribe({
      next: (card) => {
        this.card.set(card);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set('Erreur lors du chargement de la carte.');
        this.isLoading.set(false);
      }
    });

  }

  getCardImageUrl(imageUrl?: string): string {
    if (!imageUrl) return '';
    return `${imageUrl}/high.${this.extension()}`;
  }

}
