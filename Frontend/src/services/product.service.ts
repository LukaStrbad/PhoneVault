import { Injectable } from '@angular/core';
import { environment } from "../environments/environment";
import { HttpClient } from "@angular/common/http";
import { firstValueFrom } from "rxjs";
import { Product, ProductRequest } from "../model/product";
import { Review } from "../model/review";

const url = `${environment.apiUrl}/api/products`;

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private _productCache: Product[] = [];

  constructor(private http: HttpClient) {
  }

  async getAll() {
    this._productCache = await firstValueFrom(this.http.get<Product[]>(url));
    return this._productCache;
  }

  get productCache() {
    return this._productCache;
  }

  async get(id: number) {
    const value = await firstValueFrom(this.http.get<Product>(`${url}/${id}`));
    // Update cache
    const index = this._productCache.findIndex(p => p.id === id);
    if (index !== -1) {
      this._productCache[index] = value;
    } else {
      this._productCache.push(value);
    }
    return value;
  }

  async create(product: ProductRequest) {
    return firstValueFrom(this.http.post<ProductRequest>(url, product));
  }

  async update(id: number, product: ProductRequest) {
     return await firstValueFrom(this.http.put(`${url}/${id}`, product));
  }

  async delete(id: number) {
    return firstValueFrom(this.http.delete<void>(`${url}/${id}`));
  }

  async getReviews(productId: number) {
    const reviews = await firstValueFrom(this.http.get<Review[]>(`${url}/${productId}/reviews`));
    return reviews.map(review => {
      return {
        ...review,
        createdAt: new Date(review.createdAt)
      };
    })
  }

  async addReview(productId: number, rating: number, comment: string) {
    return firstValueFrom(this.http.post<Review>(`${url}/${productId}/reviews`, { rating, comment }));
  }

  async create(product: ProductRequest) {
    return firstValueFrom(this.http.post<ProductRequest>(url, product));
  }

  async update(id: number, product: ProductRequest) {
    return firstValueFrom(this.http.put<ProductRequest>(`${url}/${id}`, product));
  }

  async delete(id: number) {
    return firstValueFrom(this.http.delete<void>(`${url}/${id}`));
  }

  async getReviews(productId: number) {
    const reviews = await firstValueFrom(this.http.get<Review[]>(`${url}/${productId}/reviews`));
    return reviews.map(review => {
      return {
        ...review,
        createdAt: new Date(review.createdAt)
      };
    })
  }

  async addReview(productId: number, rating: number, comment: string) {
    return firstValueFrom(this.http.post<Review>(`${url}/${productId}/reviews`, { rating, comment }));
  }
}
