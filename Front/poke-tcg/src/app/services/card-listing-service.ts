import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
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
 
    return this._http
      .get<unknown>(this._url, { params })
      .pipe(map((response) => this.normalizePagination(response)));
  }
 
  getById(listingId: number): Observable<CardListing> {
    return this._http.get<CardListing>(`${this._url}/${listingId}`);
  }
 
  getBySeller(sellerId: number, page: number = 1): Observable<CardListingWithPagination> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', this.pageSize.toString());
 
    return this._http
      .get<unknown>(`${this._url}/seller/${sellerId}`, { params })
      .pipe(map((response) => this.normalizePagination(response)));
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

  private normalizePagination(response: unknown): CardListingWithPagination {
    if (!response || typeof response !== 'object') {
      return { listings: [], totalListings: 0 };
    }

    const payload = response as Record<string, unknown>;
    const listings = payload['listings'] ?? payload['Listings'] ?? payload['cardListings']
      ?? payload['CardListings'] ?? payload['items'] ?? payload['Items'] ?? payload['data'];
    const safeListings = Array.isArray(listings) ? listings as CardListing[] : [];
    const total = payload['totalListings'] ?? payload['TotalListings'] ?? payload['totalCount']
      ?? payload['TotalCount'] ?? payload['count'] ?? payload['Count'];
    const totalListings = Number(total);

    return {
      listings: safeListings,
      totalListings: Number.isFinite(totalListings) ? totalListings : safeListings.length,
    };
  }

}
