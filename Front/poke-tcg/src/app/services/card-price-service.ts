import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CardPriceService {
  
  private readonly _url = 'https://localhost:7009/apiTcg/CardPrice';

  constructor(private _http: HttpClient) {}

  getTotalValue(): Observable<number> {

    return this._http.get<number>(`${this._url}/value`);
  }

  getTotalValueBySet(setId: string): Observable<number>
  {
      return this._http.get<number>(`${this._url}/value/set/${encodeURIComponent(setId)}`);
  }

}
