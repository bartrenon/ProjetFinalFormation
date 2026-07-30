export interface CollectionCard {
  id: number;
  cardId: string;
  cardName?: string | null;
  cardImage?: string | null;
  nbDuplicateCard: number;
}