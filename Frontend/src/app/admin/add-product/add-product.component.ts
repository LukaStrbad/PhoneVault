import { afterNextRender, Component, ViewChild } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { ProductService } from "../../../services/product.service";
import { NgClass } from "@angular/common";
import { ToastComponent } from "../../components/toast/toast.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Product, ProductRequest } from "../../../model/product";

@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass,
    ToastComponent
  ],
  templateUrl: './add-product.component.html',
  styleUrl: './add-product.component.scss'
})
export class AddProductComponent {
  productForm = new FormGroup({
    name: new FormControl('', Validators.required),
    brand: new FormControl('', Validators.required),
    description: new FormControl('', Validators.required),
    specification: new FormControl('', Validators.required),
    netPrice: new FormControl('', Validators.required),
    sellPrice: new FormControl('', Validators.required),
    quantityInStock: new FormControl('', Validators.required),
  })

  componentType = ComponentType.Add;
  productId: number | null = null;

  @ViewChild('toast') private toastComponent!: ToastComponent;

  constructor(
    private productService: ProductService,
    route: ActivatedRoute,
    router: Router
  ) {
    // Check edit mode
    if (router.url.includes('edit-product')) {
      this.componentType = ComponentType.Edit;
    }

    const idStr = route.snapshot.paramMap.get('id');
    if (!idStr) {
      return;
    }
    this.productId = Number(idStr);
    const state = router.getCurrentNavigation()?.extras.state;

    let product: Product = state?.["product"];

    if (product) {
      this.updateProductForm(product);
    } else {
      productService.get(this.productId).then(p => {
        this.updateProductForm(p);
      });
    }
  }

  updateProductForm(product: Product) {
    this.productForm.setValue({
      name: product.name,
      brand: product.brand,
      description: product.description,
      specification: product.specification,
      netPrice: product.netPrice.toString(),
      sellPrice: product.sellPrice.toString(),
      quantityInStock: product.quantityInStock.toString()
    });
  }

  async onSubmit() {
    const value = this.productForm.value;
    if (!value.name || !value.brand || !value.description || !value.specification || !value.netPrice
      || !value.sellPrice || !value.quantityInStock) {
      return;
    }

    let product: ProductRequest = {
      name: value.name,
      brand: value.brand,
      description: value.description,
      specification: value.specification,
      netPrice: Number(value.netPrice),
      sellPrice: Number(value.sellPrice),
      quantityInStock: Number(value.quantityInStock)
    };

    if (this.componentType === ComponentType.Add) {
      await this.createProduct(product);
    } else {
      await this.updateProduct(product);
    }
  }

  async createProduct(product: ProductRequest) {
    try {
      // Add product to database
      await this.productService.create(product);

      // Show toast
      this.toastComponent.show("Success", "Product added successfully");
      this.productForm.reset();
    } catch (e) {
      // Show toast
      this.toastComponent.show("Error", "Failed to add product");
      console.error(e);
    }
  }

  async updateProduct(product: ProductRequest) {
    if (!this.productId) {
      console.error("Product ID is null");
      return;
    }

    try {
      // Update product in database
      await this.productService.update(this.productId, product);

      // Show toast
      this.toastComponent.show("Success", "Product updated successfully");
    } catch (e) {
      // Show toast
      this.toastComponent.show("Error", "Failed to update product");
      console.error(e);
    }
  }

  protected readonly ComponentType = ComponentType;
}

enum ComponentType {
  Add,
  Edit
}
