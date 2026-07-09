import { Injectable } from '@angular/core';
import { CardSummary } from '../models/card/cardSummary';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class CardServices {

  private readonly _url = 'https://localhost:7009/apiTcg/Card';

   constructor(private _http: HttpClient) {}

  getCardById(id: string): Observable<CardSummary>
  {
      return this._http.get<CardSummary>
      (`${this._url}/${encodeURIComponent(id)}`);
  }
}
