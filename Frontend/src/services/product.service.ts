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

  async getAll(categoryId?: number) {
    const requestUrl = categoryId ? `${url}?categoryId=${categoryId}` : url;
    this._productCache = await firstValueFrom(this.http.get<Product[]>(requestUrl));
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
    return firstValueFrom(this.http.post<Product>(url, product));
  }

  async update(id: number, product: ProductRequest) {
     return await firstValueFrom(this.http.put(`${url}/${id}`, product));
  }

  async delete(id: number) {
    return firstValueFrom(this.http.delete<void>(`${url}/${id}`));
  }

  async getImages(id: number) {
    return firstValueFrom(this.http.get<string[]>(`${url}/${id}/images`));
  }

  async updateImages(id: number, images: string[]) {
    return firstValueFrom(this.http.post<string[]>(`${url}/${id}/images`, images));
  }
}
