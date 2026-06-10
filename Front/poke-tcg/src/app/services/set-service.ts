import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Set } from '../models/set/set';

@Injectable({
  providedIn: 'root',
})
export class SetService {

  private readonly _url = 'https://localhost:7009/apiTcg/Set';

  constructor(private _http: HttpClient) {}

  getAllSets(): Observable<Set[]> {
    return this._http.get<Set[]>(this._url);
  }
}
