import { ListingStatus } from "./listing-status";

export interface UpdateCardListing {
  price?: number | null;
  description?: string | null;
  status?: ListingStatus | null;
}
