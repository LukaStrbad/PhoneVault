export interface ProductRequest {
  name: string;
  brand: string;
  description: string;
  specification: string;
  categoryId: number;
  netPrice: number;
  sellPrice: number;
  quantityInStock: number;
}

export type ProductResponse = ProductRequest;

export interface Product extends ProductRequest {
  id: number;
  createdDate: Date;
  updatedDate: Date;
}
