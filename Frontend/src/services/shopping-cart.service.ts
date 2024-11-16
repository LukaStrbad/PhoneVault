import { effect, Inject, Injectable, PLATFORM_ID, signal, untracked } from '@angular/core';
import { Product } from "../model/product";
import { isPlatformBrowser } from "@angular/common";
import { ShoppingCartItem } from "../model/ShoppingCartItem";
import { HttpClient } from "@angular/common/http";
import { environment } from "../environments/environment";
import { firstValueFrom } from "rxjs";
import { AuthService } from "./auth.service";

const url = `${environment.apiUrl}/api/ShoppingCart`;

@Injectable({
  providedIn: 'root'
})
export class ShoppingCartService {
  cart = signal<ShoppingCartItem[]>([]);

  constructor(
    @Inject(PLATFORM_ID) platformId: Object,
    private http: HttpClient,
    private authService: AuthService
  ) {
    if (isPlatformBrowser(platformId)) {
      const localCart = this.getLocalCart();
      this.cart.set(localCart);

      effect(() => {
        localStorage.setItem('cartItems', JSON.stringify(this.cart()));
      });
    }

    let wasLoggedIn = false;
    effect(() => {
      const isLoggedIn = authService.isUserLoggedIn();

      untracked(() => {
        if (isLoggedIn) {
          wasLoggedIn = true;
          const localCart = this.getLocalCart();
          this.getCartItems().then(async items => {
            this.cart.set(items);

            const localIds = localCart.map(item => item.productId);
            const remoteIds = items.map(item => item.productId);
            const missingIds = localIds.filter(id => !remoteIds.includes(id));
            const missingItems = localCart.filter(item => missingIds.includes(item.productId));
            this.cart.set(items.concat(missingItems));
            await this.updateCartItems(this.cart());
          });
        }
        if (wasLoggedIn && !isLoggedIn) {
          this.cart.set([]);
        }
      });
    });
  }

  getLocalCart(): ShoppingCartItem[] {
    const items = localStorage.getItem('cartItems');
    return items ? JSON.parse(items) : [];
  }

  async getCartItems() {
    return await firstValueFrom(this.http.get<ShoppingCartItem[]>(url));
  }

  async updateCartItems(items: ShoppingCartItem[]) {
    return await firstValueFrom(this.http.put(url, items));
  }

  isProductInCart(productId: number): boolean {
    return this.cart().some(item => item.productId === productId);
  }

  async addToCart(product: Product) {
    if (this.isProductInCart(product.id)) {
      return;
    }

    this.cart.update(items => items.concat({ productId: product.id, quantity: 1 }));

    if (this.authService.isUserLoggedIn()) {
      await this.updateCartItems(this.cart());
    }
  }

  async removeFromCart(productId: number) {
    this.cart.update(items => items.filter(item => item.productId !== productId));
    if (this.authService.isUserLoggedIn()) {
      await this.updateCartItems(this.cart());
    }
  }

  async updateQuantity(productId: number, quantity: number) {
    this.cart.update(items => items.map(item => item.productId === productId ? { ...item, quantity } : item));
    if (this.authService.isUserLoggedIn()) {
      await this.updateCartItems(this.cart());
    }
  }
}
