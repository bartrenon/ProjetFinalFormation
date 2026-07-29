import { CardListing } from "./card-listing";

export interface  CardListingWithPagination {
  listings: CardListing[];
  totalListings: number;
}
