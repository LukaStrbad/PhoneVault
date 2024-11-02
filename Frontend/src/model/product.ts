export interface Product {
  id: number;
  name: string;
  description: string;
  specification: string;
  sellPrice: number;
  quantityInStock: number;
  createdDate: Date;
  updatedDate: Date;
  // TODO: Implement interfaces for the following properties.
  category: any;
  reviews: any[];
}
