import { ListingStatus } from './listing-status';
 
export interface CardListing {
  listingId: number;
  cardId: string;
  price: number;
  sellerId: number;
  buyerId: number | null;
  status: ListingStatus;
  createdDate: string;
  modifiedDate: string | null;
  description: string | null;
 
  cardName?: string;
  cardImage?: string;
  setId?: string;
}
