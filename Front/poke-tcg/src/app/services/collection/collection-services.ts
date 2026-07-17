import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CollectionServices {

  private readonly _url = 'https://localhost:7009/apiTcg/Collection';

  constructor(private _http: HttpClient) {}

  createCollection(cardId: string): Observable<number> 
  {
    return this._http.post<number>(`${this._url}/${encodeURIComponent(cardId)}`,{})
  }

  deleteCollection(id: number): Observable<void> {
    return this._http.delete<void>(`${this._url}/delete/${id}`);
  }

  updateCollection(id: number, isAdding: boolean): Observable<void> {
    return this._http.patch<void>(`${this._url}/${id}/${isAdding}`,{});
  }

}
