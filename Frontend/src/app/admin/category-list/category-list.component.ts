import { Component } from '@angular/core';
import { Category } from "../../../model/category";
import { CategoryService } from "../../../services/category.service";
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.scss'
})
export class CategoryListComponent {
  categories: Category[] = [];

  constructor(private categoryService: CategoryService) {
    this.categories = this.categoryService.categoryCache;
    categoryService.getAll().then(categories => {
      this.categories = categories;
    });
  }
}
