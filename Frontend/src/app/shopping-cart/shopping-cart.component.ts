import { Component, Signal } from '@angular/core';
import { ShoppingCartService } from "../../services/shopping-cart.service";
import { ShoppingCartItem } from "../../model/ShoppingCartItem";
import { Product } from "../../model/product";
import { ProductService } from "../../services/product.service";
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-shopping-cart',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './shopping-cart.component.html',
  styleUrl: './shopping-cart.component.scss'
})
export class ShoppingCartComponent {
  cart: ShoppingCartItemWithIndex[] = [];
  products: (Product | null)[] = [];

  constructor(
    private shoppingCart: ShoppingCartService,
    private productService: ProductService
  ) {
    this.refreshProducts();
  }

  refreshProducts() {
    this.cart = this.shoppingCart.cart().map((item, i) => ({ ...item, index: i }));

    const productCache = this.productService.productCache;
    this.cart.forEach(item => {
      const product = productCache.find((product: Product) => product.id === item.productId);
      if (product) {
        this.products[item.index] = product;
      }
      this.productService.get(item.productId).then(p => {
        this.products[item.index] = p;
      });
    });
  }

  totalPrice() {
    return this.cart.reduce((total, item) => {
      const product = this.products[item.index];
      return total + (product ? product.sellPrice * item.quantity : 0);
    }, 0);
  }

  async decreaseQuantity(cartItem: ShoppingCartItemWithIndex) {
    const newQuantity = cartItem.quantity - 1;
    if (newQuantity > 0) {
      await this.shoppingCart.updateQuantity(cartItem.productId, newQuantity);
      cartItem.quantity = newQuantity;
    }
  }

  async increaseQuantity(cartItem: ShoppingCartItemWithIndex) {
    const newQuantity = cartItem.quantity + 1;
    if (newQuantity <= (this.products[cartItem.index]?.quantityInStock ?? 0)) {
      await this.shoppingCart.updateQuantity(cartItem.productId, newQuantity);
      cartItem.quantity = newQuantity;
    }
  }

  async removeItem(cartItem: ShoppingCartItemWithIndex) {
    await this.shoppingCart.removeFromCart(cartItem.productId);
    this.refreshProducts();
  }

}

export interface ShoppingCartItemWithIndex extends ShoppingCartItem {
  index: number;
}
