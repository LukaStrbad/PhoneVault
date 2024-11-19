import { Component } from '@angular/core';
import { DatePipe } from "@angular/common";
import { OrderService } from "../../services/order.service";
import { Order } from "../../model/order";
import { Product } from "../../model/product";
import { ProductService } from "../../services/product.service";

@Component({
  selector: 'app-my-orders',
  standalone: true,
  imports: [
    DatePipe
  ],
  templateUrl: './my-orders.component.html',
  styleUrl: './my-orders.component.scss'
})
export class MyOrdersComponent {
  orders: Order[] = [];
  products: Product[] = [];

  constructor(
    private orderService: OrderService,
    private productService: ProductService
  ) {
    orderService.getOrders().then(orders => {
      this.orders = orders;
    });

    productService.getAll().then(products => {
      this.products = products;
    });
  }

  productName(productId: number): string {
    const product = this.products.find(p => p.id === productId);
    return product ? product.name : "Unknown product";
  }

  totalPrice(order: Order) {
    return order.orderItems.reduce((acc, item) => {
      return acc + item.priceAtPurchase * item.quantity;
    }, 0);
  }

}
