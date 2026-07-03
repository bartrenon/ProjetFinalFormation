import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Set } from '../models/set/set';

@Injectable({
  providedIn: 'root',
})
export class SetService {

  private readonly _url = 'https://localhost:7009/apiTcg/Set';
  private readonly offset = 5;
  private readonly pageSize = 10;

  constructor(private _http: HttpClient) {}

 getAllSets(name?: string): Observable<Set[]> {
  let params = new HttpParams()
    .set('pageNumber', this.offset)   
    .set('pageSize', this.pageSize);

  if (name) {
    params = params.set('name', name);
  }

  return this._http.get<Set[]>(this._url, { params });
}
  
}
