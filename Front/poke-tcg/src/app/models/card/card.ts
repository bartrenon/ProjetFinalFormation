import { Collection } from "../collection/collection";

export interface Card
{
  id: string;
  name: string;
  localId?: string;
  image?: string;
  collections?: Collection[];
}
