import { CardSummary } from "./cardSummary";

export interface CardWithPagination {
  cards: CardSummary[];
  totalCards: number;
}
