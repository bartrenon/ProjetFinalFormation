import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CardListingService } from '../../../services/card-listing-service';
import { ActivatedRoute, Router } from '@angular/router';
import { ListingStatus } from '../../../models/Listing/listing-status';

@Component({
  selector: 'app-edit-listing',
  imports: [ReactiveFormsModule],
  templateUrl: './edit-listing.html',
  styleUrl: './edit-listing.scss',
})
export class EditListing {
  private _fb = inject(FormBuilder);
  private _listingService = inject(CardListingService);
  private _route = inject(ActivatedRoute);
  private _router = inject(Router);
 
  listingId = signal<number | null>(null);
  isLoading = signal(false);
  isSubmitting = signal(false);
  error = signal<string | null>(null);
 
  readonly statusOptions = [
    ListingStatus.Active,
    ListingStatus.Reserved,
    ListingStatus.Removed,
  ];
 
  form = this._fb.nonNullable.group({
    price: [0, [Validators.required, Validators.min(0.01)]],
    description: [''],
    status: [ListingStatus.Active, Validators.required],
  });
 
  constructor() {
    const idParam = this._route.snapshot.paramMap.get('id');
    const id = Number(idParam);
    if (idParam !== null && Number.isInteger(id) && id > 0) {
      this.listingId.set(id);
      this.loadListing(id);
    } else {
      this.error.set('Identifiant d’annonce invalide.');
    }
  }
 
  loadListing(id: number): void {
    this.isLoading.set(true);
    this.error.set(null);
    this._listingService.getById(id).subscribe({
      next: (listing) => {
        this.form.patchValue({
          price: listing.price,
          description: listing.description ?? '',
          status: listing.status,
        });
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err?.error ?? 'Annonce introuvable.');
        this.isLoading.set(false);
      },
    });
  }
 
  submit(): void {
    const id = this.listingId();
    if (id === null || this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
 
    this.isSubmitting.set(true);
    this.error.set(null);
    const value = this.form.getRawValue();
 
    this._listingService
      .update(id, {
        price: value.price,
        description: value.description || null,
        status: value.status,
      })
      .subscribe({
        next: () => {
          this.isSubmitting.set(false);
          this._router.navigate(['/listings', id]);
        },
        error: (err) => {
          this.isSubmitting.set(false);
          this.error.set(err?.error ?? 'Impossible de mettre à jour cette annonce.');
        },
      });
  }
}
