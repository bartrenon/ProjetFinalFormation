import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ImageUrlService {

  private extension = 'webp';
  private quality = 'high';

  getCardImageUrl(imageUrl?: string): string {
    if (!imageUrl) return '';
    return `${imageUrl}/${this.quality}.${this.extension}`;
  }

   getSetSymbolUrl(symbolUrl?: string): string {
    if (!symbolUrl) return '';
    return `${symbolUrl}.${this.extension}`;
  }

  getSetLogoUrl(logoUrl?: string): string {
    if (!logoUrl) return '';
    return `${logoUrl}.${this.extension}`;
  }

}
