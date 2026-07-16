import { Collection } from "../collection/collection";
import { SetSummary } from "../set/set-summary";

export interface Card
{
  id: string;
  name: string;
  setId: string;
  localId: string;
  image?: string;
  set: SetSummary;
  collection?: Collection;
}
