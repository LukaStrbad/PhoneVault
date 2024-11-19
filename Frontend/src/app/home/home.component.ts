import { Component } from '@angular/core';
import { ProductService } from "../../services/product.service";
import { Product } from "../../model/product";
import { ProductCardComponent } from "../components/product-card/product-card.component";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    ProductCardComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  products: Product[] = [];

  constructor(
    private productService: ProductService,
    route: ActivatedRoute
  ) {
    const categoryId = route.snapshot.paramMap.get('categoryId');

    let products = this.productService.productCache;
    if (categoryId) {
      this.products = this.products.filter(product => product.categoryId === Number(categoryId));
      this.productService.getAll(Number(categoryId)).then(products => {
        this.products = products;
      });
    } else {
      this.products = products;
      this.productService.getAll().then(products => {
        this.products = products;
      });
    }
  }

}
