import { AfterViewInit, ChangeDetectorRef, Component, Input } from '@angular/core';
import { Product } from "../../../model/product";
import { CurrencyPipe, NgClass } from "@angular/common";
import { RouterLink } from "@angular/router";
import { ShoppingCartService } from "../../../services/shopping-cart.service";
import { ExchangeRateService } from "../../../services/exchange-rate.service";

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
export class ProductCardComponent implements AfterViewInit{
  @Input({ required: true }) product!: Product;
  priceString = "";

  constructor(
    private shoppingCart: ShoppingCartService,
    private exchangeRate: ExchangeRateService,
    private cdr: ChangeDetectorRef
  ) {
  }

  ngAfterViewInit() {
    this.priceString = `${this.product.sellPrice} €`;
    this.cdr.detectChanges();
    this.exchangeRate.calculatePrice(this.product.sellPrice).then(price => {
      this.priceString = `${price} ${this.exchangeRate.selectedCurrency}`;
    });
  }

  get specifications(): string[] {
    const specs = this.product.specification.split("\n").slice(0, maxSpecificationLength);
    // Fill the rest with empty strings
    return specs.concat(new Array(maxSpecificationLength - specs.length).fill(""));
  }

  async addToCart() {
    await this.shoppingCart.addToCart(this.product);
  }

  async removeFromCart() {
    await this.shoppingCart.removeFromCart(this.product.id);
  }

  showAddToCartButton() {
    return !this.shoppingCart.isProductInCart(this.product.id);
  }
}
