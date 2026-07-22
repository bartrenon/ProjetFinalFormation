import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Card } from '../models/card/card';
import { CardWithPagination } from '../models/card/card-with-pagination';

@Injectable({
  providedIn: 'root',
})
export class CardServices {

  private readonly _url = 'https://localhost:7009/apiTcg/Card';

  readonly defaultPage  = 1;
  readonly pageSize = 24;

  constructor(private _http: HttpClient) {}

  getCardById(id: string): Observable<Card>
  {
      return this._http.get<Card>(`${this._url}/${encodeURIComponent(id)}`);
  }

  getCards(pageNumber: number = this.defaultPage, name?: string): Observable<CardWithPagination> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)   
      .set('pageSize', this.pageSize);

    if (name && name != '') {
      params = params.set('name', name);
    }

    return this._http.get<CardWithPagination>(this._url, { params });
  }

}
