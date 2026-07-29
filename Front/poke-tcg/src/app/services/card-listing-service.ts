import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CardListingWithPagination } from '../models/Listing/card-listing-with-pagination';
import { CardListing } from '../models/Listing/card-listing';
import { CreateCardListing } from '../models/Listing/create-card-listing';
import { UpdateCardListing } from '../models/Listing/update-card-listing';

@Injectable({
  providedIn: 'root',
})
export class CardListingService {

  private readonly _url = 'https://localhost:7009/apiTcg/CardListing';

  pageSize = 12;

  constructor(private _http: HttpClient) {}

  getActiveListings(page: number, query: string = ''): Observable<CardListingWithPagination> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', this.pageSize.toString());
 
    if (query) {
      params = params.set('q', query);
    }
 
    return this._http.get<CardListingWithPagination>(this._url, { params });
  }
 
  getById(listingId: number): Observable<CardListing> {
    return this._http.get<CardListing>(`${this._url}/${listingId}`);
  }
 
  getBySeller(sellerId: number, page: number = 1): Observable<CardListingWithPagination> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', this.pageSize.toString());
 
    return this._http.get<CardListingWithPagination>(
      `${this._url}/seller/${sellerId}`,
      { params }
    );
  }
 
  create(dto: CreateCardListing): Observable<CardListing> {
    return this._http.post<CardListing>(this._url, dto);
  }
 
  update(listingId: number, dto: UpdateCardListing): Observable<void> {
    return this._http.put<void>(`${this._url}/${listingId}`, dto);
  }
 
  delete(listingId: number): Observable<void> {
    return this._http.delete<void>(`${this._url}/${listingId}`);
  }

  buy(listingId: number): Observable<void> {
    return this._http.post<void>(`${this._url}/${listingId}/buy`, {});
  }

}
