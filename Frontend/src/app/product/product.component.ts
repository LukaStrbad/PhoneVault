import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { Product } from "../../model/product";
import { ProductService } from "../../services/product.service";
import { DatePipe, NgClass } from "@angular/common";
import { Review } from "../../model/review";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { StarRatingComponent } from "../components/star-rating/star-rating.component";
import { ShoppingCartService } from "../../services/shopping-cart.service";
import { ReviewCardComponent } from "../components/review-card/review-card.component";
import { AuthService } from "../../services/auth.service";
import { ReviewsService } from "../../services/reviews.service";

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [
    NgClass,
    ReactiveFormsModule,
    StarRatingComponent,
    DatePipe,
    ReviewCardComponent
  ],
  templateUrl: './product.component.html',
  styleUrl: './product.component.scss'
})
export class ProductComponent {
  product?: Product;
  notFound = false;
  id: number;
  reviews: Review[] = [];
  imagesUrls: string[] | null = null;

  commentForm = new FormGroup({
    comment: new FormControl('', Validators.required),
    rating: new FormControl(0, [Validators.min(1), Validators.max(5)])
  });

  get specifications(): string[] {
    if (!this.product) {
      return [];
    }

    return this.product.specification.split("\n");
  }

  constructor(
    route: ActivatedRoute,
    router: Router,
    productService: ProductService,
    private reviewsService: ReviewsService,
    private shoppingCart: ShoppingCartService,
    public auth: AuthService
  ) {
    this.id = Number(route.snapshot.paramMap.get('id'));
    const state = router.getCurrentNavigation()?.extras.state;

    this.product = state?.["product"];

    if (!this.product) {
      productService.get(this.id).then(product => {
        this.product = product;
      }).catch(() => {
        this.notFound = true;
      });
    }

    reviewsService.getReviews(this.id).then(reviews => {
      this.reviews = reviews;
    });

    productService.getImages(this.id).then(imagesUrls => {
      this.imagesUrls = imagesUrls;
    });

  }

  async onSubmit() {
    const value = this.commentForm.value;
    if (!value.comment || !value.rating || value.rating < 1 || value.rating > 5) {
      return;
    }

    await this.reviewsService.addReview(this.id, value.rating, value.comment);
    this.commentForm.reset();
    this.reviews = await this.reviewsService.getReviews(this.id);
  }

  async addToCart() {
    if (!this.product) {
      return
    }
    await this.shoppingCart.addToCart(this.product);
  }

  async removeFromCart() {
    if (!this.product) {
      return
    }
    await this.shoppingCart.removeFromCart(this.product.id);
  }

  showAddToCartButton() {
    if (!this.product) {
      return false;
    }
    return !this.shoppingCart.isProductInCart(this.product.id);
  }

  async onReviewUpdate(review: Review, [comment, rating]: [string, number]) {
    await this.reviewsService.updateReview(review.id, rating, comment);
  }

  async onReviewDelete(review: Review) {
    await this.reviewsService.deleteReview(review.id);
    this.reviews = await this.reviewsService.getReviews(this.id);
  }
}
