import { Component } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { ShoppingCartItem } from "../../model/ShoppingCartItem";
import { ShoppingCartService } from "../../services/shopping-cart.service";
import { Product } from "../../model/product";
import { ProductService } from "../../services/product.service";
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { AuthService } from "../../services/auth.service";
import { NgClass } from "@angular/common";
import { OrderService } from "../../services/order.service";

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent {
  items: ShoppingCartItem[] = [];
  products: Product[] = [];
  checkoutForm = new FormGroup({
    name: new FormControl('', Validators.required),
    address: new FormControl('', Validators.required),
    email: new FormControl('', Validators.required),
    cardNumber: new FormControl('', Validators.required),
    expiry: new FormControl('', Validators.required),
    cvv: new FormControl('', Validators.required)
  });

  constructor(
    route: ActivatedRoute,
    private shoppingCart: ShoppingCartService,
    private productService: ProductService,
    private authService: AuthService,
    private orderService: OrderService
  ) {
    this.products = this.productService.productCache;
    this.productService.getAll().then(products => {
      this.products = products;
    });

    if (authService.isUserLoggedIn()) {
      this.checkoutForm.controls.name.setValue(authService.user?.name ?? "");
      this.checkoutForm.controls.email.setValue(authService.user?.email ?? "");
    }

    const id = route.snapshot.queryParamMap.get('productId');
    console.log("Product id: ", id);

    if (id) {
      const idNum = Number(id);
      this.items.push({ productId: idNum, quantity: 1 });
    } else {
      this.items = shoppingCart.cart();
    }
  }

  productName(id: number) {
    return this.products.find(p => p.id === id)?.name;
  }

  price(id: number) {
    const productPrice = this.products.find(p => p.id === id)?.sellPrice;
    const quantity = this.items.find(i => i.productId === id)?.quantity;
    if (productPrice && quantity) {
      return productPrice * quantity;
    }
    return 0;
  }

  totalPrice() {
    return this.items.reduce((acc, item) => {
      return acc + this.price(item.productId);
    }, 0);
  }

  async onSubmit() {
    const value = this.checkoutForm.value;
    if (!value.name || !value.address || !value.email || !value.cardNumber || !value.expiry || !value.cvv) {
      return;
    }

    await this.orderService.createOrder(value.address, this.items);
    this.checkoutForm.reset();
    this.items = [];
    await this.shoppingCart.clearCart();
  }
}
