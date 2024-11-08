import { Component, Input } from '@angular/core';
import { Product } from "../../../model/product";
import { CurrencyPipe, NgClass } from "@angular/common";
import { RouterLink } from "@angular/router";

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

  get specifications(): string[] {
    const specs = this.product.specification.split("\n").slice(0, maxSpecificationLength);
    // Fill the rest with empty strings
    return specs.concat(new Array(maxSpecificationLength - specs.length).fill(""));
  }
}
