import { Component } from '@angular/core';
import { DatePipe } from "@angular/common";
import { Product } from "../../../model/product";
import { ProductService } from "../../../services/product.service";
import { RouterLink } from "@angular/router";
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

  constructor(private productService: ProductService) {
    this.products = this.productService.productCache;
    productService.getAll().then(products => {
      this.products = products;
    });
  }
}
