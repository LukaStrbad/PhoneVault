import { Injectable } from '@angular/core';
import { environment } from "../environments/environment";
import { HttpClient } from "@angular/common/http";
import { firstValueFrom } from "rxjs";
import { Category } from "../model/category";

const url = `${environment.apiUrl}/api/category`;

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private _categoryCache: Category[] = [];

  get categoryCache() {
    return this._categoryCache;
  }

  constructor(
    private http: HttpClient
  ) {
  }

  async getAll() {
    this._categoryCache = await firstValueFrom(this.http.get<Category[]>(url));
    return this.categoryCache;
  }

  async get(id: number) {
    const value = await firstValueFrom(this.http.get<Category>(`${url}/${id}`));
    // Update cache
    const index = this.categoryCache.findIndex(p => p.id === id);
    if (index !== -1) {
      this.categoryCache[index] = value;
    } else {
      this.categoryCache.push(value);
    }
    return value;
  }

  async create(name: string, description: string) {
    return await firstValueFrom(this.http.post<Category>(url, { name, description }));
  }

  async update(id: number, name: string, description: string) {
    return await firstValueFrom(this.http.put(`${url}/${id}`, { name, description }));
  }

  async delete(id: number) {
    return await firstValueFrom(this.http.delete(`${url}/${id}`, { responseType: 'text' }));
  }
}
