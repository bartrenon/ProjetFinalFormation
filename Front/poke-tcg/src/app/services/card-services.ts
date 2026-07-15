import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Card } from '../models/card/card';

@Injectable({
  providedIn: 'root',
})
export class CardServices {

  private readonly _url = 'https://localhost:7009/apiTcg/Card';

   constructor(private _http: HttpClient) {}

  getCardById(id: string): Observable<Card>
  {
      return this._http.get<Card>
      (`${this._url}/${encodeURIComponent(id)}`);
  }
}
