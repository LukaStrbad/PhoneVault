import { Injectable } from '@angular/core';
import { environment } from "../environments/environment";
import { HttpClient } from "@angular/common/http";
import { firstValueFrom } from "rxjs";
import { Review } from "../model/review";

const url = `${environment.apiUrl}/api/reviews`;

@Injectable({
  providedIn: 'root'
})
export class ReviewsService {

  constructor(private http: HttpClient) { }

  async getForProduct(productId: number) {
    return await firstValueFrom(this.http.get<Review[]>(`${url}?productId=${productId}`));
  }
}
