import { Component } from '@angular/core';
import { DatePipe } from "@angular/common";
import { OrderService } from "../../services/order.service";
import { Order } from "../../model/order";
import { Product } from "../../model/product";
import { ProductService } from "../../services/product.service";
import { ExchangeRateService } from "../../services/exchange-rate.service";

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
  totalPricesStrings: string[] = [];
  currency = '€';

  constructor(
    private orderService: OrderService,
    private productService: ProductService,
    private exchangeRate: ExchangeRateService,
  ) {
    orderService.getOrders().then(orders => {
      this.totalPricesStrings = orders.map(o => `${this.totalPrice(o)} €`);
      this.orders = orders;

      exchangeRate.getExchangeRate().then(() => {
        this.totalPricesStrings = orders.map(o => {
          const price = this.totalPrice(o);
          const priceCurrency = exchangeRate.calculatePriceSync(price);
          return `${priceCurrency} ${exchangeRate.selectedCurrency}`;
        });
      });
    });

    productService.getAll().then(products => {
      this.products = products;
    });

    if (typeof window !== 'undefined' && exchangeRate.selectedCurrency !== 'EUR') {
      this.currency = exchangeRate.selectedCurrency;
    }
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
