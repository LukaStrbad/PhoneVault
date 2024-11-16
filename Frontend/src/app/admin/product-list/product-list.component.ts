import { Component } from '@angular/core';
import { DatePipe } from "@angular/common";
import { Product } from "../../../model/product";
import { ProductService } from "../../../services/product.service";
import { RouterLink } from "@angular/router";
import { CategoryService } from "../../../services/category.service";
import { Category } from "../../../model/category";

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    DatePipe,
    RouterLink
  ],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent {
  products: Product[] = [];
  categories: Category[] = [];

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService
  ) {
    this.products = this.productService.productCache;
    this.categories = this.categoryService.categoryCache;

    Promise.all([productService.getAll(), categoryService.getAll()])
      .then(([products, categories]) => {
        this.products = products;
        this.categories = categories;
      });
  }

  categoryName(categoryId: number): string {
    const category = this.categories.find(c => c.id === categoryId);
    return category ? category.name : '';
  }
}
