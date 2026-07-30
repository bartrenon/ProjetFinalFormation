import { Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CardListingService } from '../../../services/card-listing-service';
import { CollectionCard } from '../../../models/collection/collection-card';
import { ImageUrlService } from '../../../services/tools/image-url-service';
import { Router } from '@angular/router';
import { CollectionServices } from '../../../services/collection/collection-services';

@Component({
  selector: 'app-create-listing',
  imports: [ReactiveFormsModule],
  templateUrl: './create-listing.html',
  styleUrl: './create-listing.scss',
})
export class CreateListing {
  private _fb = inject(FormBuilder);
  private _listingService = inject(CardListingService);
  private _collectionService = inject(CollectionServices);
  _imageUrlService = inject(ImageUrlService);
  private _router = inject(Router);

  isSubmitting = signal(false);
  isLoadingCollection = signal(false);
  error = signal<string | null>(null);
  myCards = signal<CollectionCard[]>([]);

  searchTerm = signal('');
  showSuggestions = signal(false);
  selectedCard = signal<CollectionCard | null>(null);

  filteredCards = computed(() => {
    const term = this.searchTerm().trim().toLowerCase();
    if (!term) return this.myCards();
    return this.myCards().filter((c) =>
      (c.cardName ?? c.cardId).toLowerCase().includes(term)
    );
  });

  form = this._fb.nonNullable.group({
    cardId: ['', Validators.required],
    price: [0, [Validators.required, Validators.min(0.01)]],
    description: [''],
  });

  constructor() {
    this.loadMyCollection();
  }

  loadMyCollection(): void {
    this.isLoadingCollection.set(true);
    this._collectionService.getMyCollection().subscribe({
      next: (cards) => {
        this.myCards.set(cards);
        this.isLoadingCollection.set(false);
      },
      error: (err) => {
        this.error.set(err?.error?.message ?? err?.error ?? 'Impossible de charger votre collection.');
        this.isLoadingCollection.set(false);
      },
    });
  }

  onSearchInput(value: string): void {
    this.searchTerm.set(value);
    this.showSuggestions.set(true);
    // si l'utilisateur modifie le texte, on invalide la sélection précédente
    if (this.selectedCard() && value !== (this.selectedCard()?.cardName ?? this.selectedCard()?.cardId)) {
      this.selectedCard.set(null);
      this.form.controls.cardId.setValue('');
    }
  }

  selectCard(card: CollectionCard): void {
    this.selectedCard.set(card);
    this.searchTerm.set(card.cardName ?? card.cardId);
    this.form.controls.cardId.setValue(card.cardId);
    this.showSuggestions.set(false);
  }

  onSearchBlur(): void {
    // léger délai pour laisser le temps au clic sur une suggestion de s'exécuter
    setTimeout(() => this.showSuggestions.set(false), 150);
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.error.set(null);

    const value = this.form.getRawValue();

    this._listingService
      .create({
        cardId: value.cardId,
        price: value.price,
        description: value.description || null,
      })
      .subscribe({
        next: (created) => {
          this.isSubmitting.set(false);
          this._router.navigate(['/listings', created.listingId]);
        },
        error: (err) => {
          this.isSubmitting.set(false);
          this.error.set(err?.error?.message ?? err?.error ?? "Impossible de créer l'annonce.");
        },
      });
  }
}