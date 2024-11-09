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

  async getReviews(productId: number) {
    const reviews = await firstValueFrom(this.http.get<Review[]>(`${url}?productId=${productId}`));
    return reviews.map(review => {
      return {
        ...review,
        createdAt: new Date(review.createdAt)
      };
    })
  }

  async addReview(productId: number, rating: number, comment: string) {
    return firstValueFrom(this.http.post(`${url}?productId=${productId}`, { rating, comment }));
  }

  async updateReview(reviewId: number, rating: number, comment: string) {
    return firstValueFrom(this.http.put(`${url}/${reviewId}`, { rating, comment }));
  }

  async deleteReview(reviewId: number) {
    return firstValueFrom(this.http.delete(`${url}/${reviewId}`));
  }
}
