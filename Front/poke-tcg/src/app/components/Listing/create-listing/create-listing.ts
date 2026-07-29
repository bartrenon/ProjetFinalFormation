import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CardListingService } from '../../../services/card-listing-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-listing',
  imports: [ReactiveFormsModule],
  templateUrl: './create-listing.html',
  styleUrl: './create-listing.scss',
})
export class CreateListing {
  private _fb = inject(FormBuilder);
  private _listingService = inject(CardListingService);
  private _router = inject(Router);
 
  isSubmitting = signal(false);
  error = signal<string | null>(null);
 
  form = this._fb.nonNullable.group({
    cardId: ['', Validators.required],
    price: [0, [Validators.required, Validators.min(0.01)]],
    description: [''],
  });
 
  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
 
    this.isSubmitting.set(true);
    this.error.set(null);
 
    const value = this.form.getRawValue();
    const cardId = value.cardId.trim();
    if (!cardId) {
      this.form.controls.cardId.setErrors({ required: true });
      this.form.controls.cardId.markAsTouched();
      this.isSubmitting.set(false);
      return;
    }

    this._listingService
      .create({
        cardId,
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
