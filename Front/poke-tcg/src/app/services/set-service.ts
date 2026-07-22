import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Set } from '../models/set/set';
import { SetWithPagination } from '../models/set/set-with-pagination';

@Injectable({
  providedIn: 'root',
})
export class SetService {

  private readonly _url = 'https://localhost:7009/apiTcg/Set';

  readonly defaultPage = 1;
  readonly pageSize = 20;

  constructor(private _http: HttpClient) {}

  getAllSets(pageNumber: number = this.defaultPage, name?: string): Observable<SetWithPagination> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)   
      .set('pageSize', this.pageSize);

    if (name && name != '') {
      params = params.set('name', name);
    }

    return this._http.get<SetWithPagination>(this._url, { params });
  }

  getAllCardsOfSet(setId: string): Observable<Set> {
    return this._http.get<Set>(`${this._url}/${encodeURIComponent(setId)}`);
  }  
  
}
