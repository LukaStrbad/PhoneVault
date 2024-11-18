export interface Order {
  id: number;
  shippingAddress: string;
  orderDate: Date;
  orderItems: {
    productId: number;
    quantity: number;
    priceAtPurchase: number;
  }[];
}
