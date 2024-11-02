import { Component } from '@angular/core';
import { ProductService } from "../../services/product.service";
import { Product } from "../../model/product";
import { ProductCardComponent } from "../components/product-card/product-card.component";

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
    private productService: ProductService
  ) {
    this.productService.getAll().then(products => {
      this.products = products;
    });
  }

}
