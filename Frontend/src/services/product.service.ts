import { Injectable } from '@angular/core';
import { environment } from "../environments/environment";
import { HttpClient } from "@angular/common/http";
import { firstValueFrom } from "rxjs";
import { Product } from "../model/product";

const url = `${environment.apiUrl}/api/products`;

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private http: HttpClient) {

  }

  async getAll() {
    return firstValueFrom(this.http.get<Product[]>(url));
  }

  async get(id: number) {
    return firstValueFrom(this.http.get<Product>(`${url}/${id}`));
  }
}
