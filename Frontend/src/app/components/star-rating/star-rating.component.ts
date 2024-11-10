import { Component, Input, output, Output } from '@angular/core';
import { NgClass, NgStyle } from "@angular/common";

@Component({
  selector: 'app-star-rating',
  standalone: true,
  imports: [
    NgClass,
    NgStyle
  ],
  templateUrl: './star-rating.component.html',
  styleUrl: './star-rating.component.scss'
})
export class StarRatingComponent {
  @Input() interactive = false;
  @Input() rating = 0;
  @Input() size = "1em";
  ratingChange = output<number>();

  dragRating = 0;
  readonly productStars = [1, 2, 3, 4, 5];
  timeout: NodeJS.Timeout | null = null;

  onMouseEnter(i: number) {
    if (!this.interactive) {
      return;
    }

    if (this.timeout) {
      clearTimeout(this.timeout);
    }
    this.dragRating = i;
  }

  onMouseLeave() {
    if (!this.interactive) {
      return;
    }

    this.timeout = setTimeout(() => {
      this.dragRating = 0;
    }, 150);
  }

  onRated(i: number) {
    if (!this.interactive) {
      return;
    }

    this.rating = i;
    this.ratingChange.emit(i);
  }
}
