import { Component, Input, output } from '@angular/core';
import { Review } from "../../../model/review";
import { StarRatingComponent } from "../star-rating/star-rating.component";
import { DatePipe } from "@angular/common";
import { FormsModule } from "@angular/forms";

@Component({
  selector: 'app-review-card',
  standalone: true,
  imports: [
    StarRatingComponent,
    DatePipe,
    FormsModule
  ],
  templateUrl: './review-card.component.html',
  styleUrl: './review-card.component.scss'
})
export class ReviewCardComponent {
  @Input() review!: Review;
  @Input() editable = false;
  onUpdate = output<[string, number]>();
  onDelete = output<void>();

  editing = false;
  originalComment = '';
  originalRating = 0;


  onEditClick() {
    this.originalComment = this.review.comment;
    this.originalRating = this.review.rating;
    this.editing = true;
  }

  onSaveClick() {
    this.onUpdate.emit([this.review.comment, this.review.rating]);
    this.editing = false;
  }

  onCancelClick() {
    this.review.comment = this.originalComment;
    this.review.rating = this.originalRating;
    this.editing = false;
  }

  onDeleteClick() {
    this.onDelete.emit();
  }
}
