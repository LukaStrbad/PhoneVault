import { Injectable } from '@angular/core';
import { environment } from "../environments/environment";
import { HttpClient } from "@angular/common/http";
import { ShoppingCartItem } from "../model/ShoppingCartItem";
import { firstValueFrom } from "rxjs";
import { Order } from "../model/order";

const url = `${environment.apiUrl}/api/order`;

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(
    private http: HttpClient
  ) { }

  async createOrder(shippingAddress: string, items: ShoppingCartItem[]): Promise<void> {
    await firstValueFrom(this.http.post(url, { shippingAddress, orderItems: items }));
  }

  async getOrders(): Promise<Order[]> {
    const orders = await firstValueFrom(this.http.get<Order[]>(url));
    return orders.map(order => {
      order.orderDate = new Date(order.orderDate);
      return order;
    });
  }
}
