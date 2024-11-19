import { Component } from '@angular/core';
import { CategoryService } from "../../services/category.service";
import { Category } from "../../model/category";
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './categories.component.html',
  styleUrl: './categories.component.scss'
})
export class CategoriesComponent {
  categories: Category[];

  constructor(
    private categoryService: CategoryService
  ) {
    this.categories = this.categoryService.categoryCache;
    this.categoryService.getAll().then(categories => {
      this.categories = categories;
    });
  }
}
