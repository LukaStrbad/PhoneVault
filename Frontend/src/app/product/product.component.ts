import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { Product } from "../../model/product";
import { ProductService } from "../../services/product.service";

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [],
  templateUrl: './product.component.html',
  styleUrl: './product.component.scss'
})
export class ProductComponent {
  product?: Product;

  constructor(
    route: ActivatedRoute,
    router: Router,
    productService: ProductService
  ) {
    const id = Number(route.snapshot.paramMap.get('id'));
    const state = router.getCurrentNavigation()?.extras.state;

    this.product = state?.["product"];

    if (!this.product) {
      productService.get(id).then(product => {
        this.product = product;
      });
    }

  }

}
