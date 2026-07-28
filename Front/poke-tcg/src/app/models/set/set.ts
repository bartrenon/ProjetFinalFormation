import { CardSummary } from "../card/cardSummary";

export interface Set 
{
   id : string;
   name : string;
   logo : string;
   symbol  : string;
   cardCountTotal : number;
   cardCountOfficial : number;
   cards: CardSummary[];
   isCompleted: boolean; 
}
