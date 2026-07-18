import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Card } from '../models/card/card';

@Injectable({
  providedIn: 'root',
})
export class CardServices {

  private readonly _url = 'https://localhost:7009/apiTcg/Card';

  private readonly offset = 5;
  private readonly pageSize = 20;

  constructor(private _http: HttpClient) {}

  getCardById(id: string): Observable<Card>
  {
      return this._http.get<Card>
      (`${this._url}/${encodeURIComponent(id)}`);
  }

  getCards(name?: string): Observable<Card[]> {
    let params = new HttpParams()
      .set('pageNumber', this.offset)   
      .set('pageSize', this.pageSize);

    if (name) {
      params = params.set('name', name);
    }

    return this._http.get<Card[]>(this._url, { params });
  }

}
