import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { SetService } from '../../../services/set-service';
import { Set } from '../../../models/set/set';

@Component({
  selector: 'app-list-cards-of-set',
  imports: [RouterLink],
  templateUrl: './list-cards-of-set.html',
  styleUrl: './list-cards-of-set.scss',
})
export class ListCardsOfSet implements OnInit {

  private _setService = inject(SetService);
  private _route = inject(ActivatedRoute);

  set = signal<Set | null>(null);
  isLoading = signal(false);
  error = signal<string | null>(null);
  extension = signal('webp');

  ngOnInit(): void {
    this.loadSet();
  }

  loadSet(): void {
    const setId = this._route.snapshot.paramMap.get('id');

    if (!setId) {
      this.error.set('Aucun identifiant de set fourni.');
      return;
    }

    this.isLoading.set(true);

    this._setService.getAllCardsOfSet(setId).subscribe({
      next: (data) => {
        this.set.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error(err);
        this.error.set('Impossible de charger le set.');
        this.isLoading.set(false);
      }
    });
  }

  getCardImageUrl(imageUrl?: string): string {
    if (!imageUrl) return '';
    return `${imageUrl}/high.${this.extension()}`;
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