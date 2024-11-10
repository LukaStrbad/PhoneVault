import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { Product } from "../../model/product";
import { ProductService } from "../../services/product.service";
import { DatePipe, NgClass } from "@angular/common";
import { Review } from "../../model/review";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { StarRatingComponent } from "../components/star-rating/star-rating.component";

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [
    NgClass,
    ReactiveFormsModule,
    StarRatingComponent,
    DatePipe
  ],
  templateUrl: './product.component.html',
  styleUrl: './product.component.scss'
})
export class ProductComponent {
  product?: Product;
  id: number;
  reviews: Review[] = [];

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
    private productService: ProductService
  ) {
    this.id = Number(route.snapshot.paramMap.get('id'));
    const state = router.getCurrentNavigation()?.extras.state;

    this.product = state?.["product"];

    if (!this.product) {
      productService.get(this.id).then(product => {
        this.product = product;
      });
    }

    productService.getReviews(this.id).then(reviews => {
      this.reviews = reviews;
    });

  }

  async onSubmit() {
    const value = this.commentForm.value;
    if (!value.comment || !value.rating || value.rating < 1 || value.rating > 5) {
      return;
    }

    await this.productService.addReview(this.id, value.rating, value.comment);
    this.commentForm.reset();
    this.reviews = await this.productService.getReviews(this.id);
  }
}
