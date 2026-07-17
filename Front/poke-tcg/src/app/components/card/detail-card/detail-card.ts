import { Component, inject, OnInit, signal } from '@angular/core';
import { CardServices } from '../../../services/card-services';
import { Card } from '../../../models/card/card';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { CollectionServices } from '../../../services/collection/collection-services';

@Component({
  selector: 'app-detail-card',
  imports: [DatePipe, RouterLink],
  templateUrl: './detail-card.html',
  styleUrl: './detail-card.scss',
})
export class DetailCard implements OnInit {
  
  private _cardService = inject(CardServices);
  private _collectionService = inject(CollectionServices);
  private _route = inject(ActivatedRoute);

  card = signal<Card>({} as Card);
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

   getSetSymbolUrl(symbolUrl?: string): string {
    if (!symbolUrl) return '';
    return `${symbolUrl}.${this.extension()}`;
  }

  onAddDuplicate(): void {
    if (this.card().collection == null) {
      this._collectionService.createCollection(this.card().id).subscribe({
        next: (id: number) => {
          this.card.update(card => ({ ...card,
            collection: {
              id: id,
              nbDuplicateCard: 1,
              createdAt: new Date() }
          }));
      }});
    }
    else {
        this._collectionService.updateCollection(this.card().collection!.id, true).subscribe({
          next: () => {
            this.card.update(card => ({...card,
              collection: { ...card.collection!,
                            nbDuplicateCard: card.collection!.nbDuplicateCard + 1
                          }}));
          }});
    }}

  onRemoveDuplicate(): void{
    if (this.card().collection!.nbDuplicateCard > 1 ) {
      this._collectionService.updateCollection(this.card().collection!.id, false).subscribe({
          next: () => {
            this.card.update(card => ({...card,
              collection: { ...card.collection!,
                            nbDuplicateCard: card.collection!.nbDuplicateCard - 1
                          }}));
      }});
    }
    else {
        this._collectionService.deleteCollection(this.card().collection!.id).subscribe({
          next: () => {
            this.card.update(card => ({...card,
             collection: undefined
            }));
          }});
    }}
}
