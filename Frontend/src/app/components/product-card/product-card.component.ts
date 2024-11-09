import { Component, Input } from '@angular/core';
import { Product } from "../../../model/product";
import { CurrencyPipe, NgClass } from "@angular/common";
import { RouterLink } from "@angular/router";
import { ShoppingCartService } from "../../../services/shopping-cart.service";

const maxSpecificationLength = 3;

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [
    CurrencyPipe,
    RouterLink,
    NgClass
  ],
  templateUrl: './product-card.component.html',
  styleUrl: './product-card.component.scss'
})
export class ProductCardComponent {
  @Input({ required: true }) product!: Product;

  constructor(
    private cart: ShoppingCartService
  ) {

  }

  get specifications(): string[] {
    const specs = this.product.specification.split("\n").slice(0, maxSpecificationLength);
    // Fill the rest with empty strings
    return specs.concat(new Array(maxSpecificationLength - specs.length).fill(""));
  }

  addToCart() {
    this.cart.addToCart(this.product);
  }

  removeFromCart() {
    this.cart.removeFromCart(this.product.id);
  }

  showAddToCartButton() {
    return !this.cart.isProductInCart(this.product.id);
  }
}
