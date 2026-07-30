import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ImageUrlService {

  private extension = 'webp';
  private quality = 'high';
  private fallbackCardImage = '/imageDefaultCard.webp';
  private fallbackCardLogo = '/logoDefaultSet.png';
  private fallbackCardSymbol = '/symbolDefaultSet.png';

  getCardImageUrl(imageUrl?: string | null): string {
  if (!imageUrl) return this.fallbackCardImage;
  return `${imageUrl}/${this.quality}.${this.extension}`;
}

getSetSymbolUrl(symbolUrl?: string | null): string {
  if (!symbolUrl) return this.fallbackCardSymbol;
  return `${symbolUrl}.${this.extension}`;
}

getSetLogoUrl(logoUrl?: string | null): string {
  if (!logoUrl) return this.fallbackCardLogo;
  return `${logoUrl}.${this.extension}`;
}

}
