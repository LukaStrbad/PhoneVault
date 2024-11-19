import { Component, Inject, makeStateKey, OnInit, PLATFORM_ID, TransferState } from '@angular/core';
import { ReviewsService } from "../../../services/reviews.service";
import { ActivatedRoute } from "@angular/router";
import { Review } from "../../../model/review";
import { ReviewCardComponent } from "../../components/review-card/review-card.component";
import { ProductService } from "../../../services/product.service";
import { Product } from "../../../model/product";
import { isPlatformBrowser, isPlatformServer } from "@angular/common";

const stateKey = makeStateKey<[Product | null, Review[]]>('product');

@Component({
  selector: 'app-reviews',
  standalone: true,
  imports: [
    ReviewCardComponent
  ],
  templateUrl: './reviews.component.html',
  styleUrl: './reviews.component.scss'
})
export class ReviewsComponent implements OnInit {
  productId: number = 0;
  product: Product | null = null;
  reviews: Review[] = [];

  constructor(
    @Inject(PLATFORM_ID) private platformId: Object,
    private transferState: TransferState,
    private route: ActivatedRoute,
    private productService: ProductService,
    private reviewsService: ReviewsService
  ) {

  }

  ngOnInit(): void {
    this.productId = Number(this.route.snapshot.paramMap.get('productId'));

    if (isPlatformBrowser(this.platformId)) {
      if (this.transferState.hasKey(stateKey)) {
        [this.product, this.reviews] = this.transferState.get(stateKey, [null, []]);
      } else {
        this.fetchReviews().then();
      }
    } else {
      this.fetchReviews(true).then();
    }
  }

  async fetchReviews(setState = false) {
    [this.product, this.reviews] = await Promise.all([
      this.productService.get(this.productId),
      this.reviewsService.getReviews(this.productId)
    ]);
    if (setState) {
      this.transferState.set(stateKey, [this.product, this.reviews]);
    }
  }

  onDelete(review: Review) {
    this.reviewsService.deleteReview(review.id).then(() => {
      this.reviews = this.reviews.filter(r => r !== review);
    });
  }

  async onUpdate(review: Review, [comment, rating]: [string, number]) {
    await this.reviewsService.updateReview(review.id, rating, comment);
  }
}
