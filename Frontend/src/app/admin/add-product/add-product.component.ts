import { afterNextRender, AfterViewInit, Component, ElementRef, OnInit, ViewChild, ViewRef } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { ProductService } from "../../../services/product.service";
import { NgClass } from "@angular/common";
import { ToastComponent } from "../../components/toast/toast.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Product, ProductRequest } from "../../../model/product";
import { CategoryService } from "../../../services/category.service";
import { Category } from "../../../model/category";
import { HttpClient } from "@angular/common/http";
import { firstValueFrom } from "rxjs";
import { ImageBlobService } from "../../../services/image-blob.service";

@Component({
  selector: 'app-add-product',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass,
    ToastComponent,
    FormsModule
  ],
  templateUrl: './add-product.component.html',
  styleUrl: './add-product.component.scss'
})
export class AddProductComponent implements AfterViewInit {
  productForm = new FormGroup({
    name: new FormControl('', Validators.required),
    brand: new FormControl('', Validators.required),
    description: new FormControl('', Validators.required),
    specification: new FormControl('', Validators.required),
    category: new FormControl<number>(0, Validators.min(1)),
    netPrice: new FormControl('', Validators.required),
    sellPrice: new FormControl('', Validators.required),
    quantityInStock: new FormControl('', Validators.required),
  });

  componentType = ComponentType.Add;
  productId: number | null = null;
  categories: Category[] = [];
  imagesUrls: string[] = [];
  newImageUrl = "";
  @ViewChild('fileInput') private fileInput!: ElementRef<HTMLInputElement>;

  @ViewChild('toast') private toastComponent!: ToastComponent;
  private uploadFiles: FileList | null = null;

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    route: ActivatedRoute,
    private router: Router,
    private imageBlobService: ImageBlobService
  ) {
    // Check edit mode
    if (router.url.includes('edit-product')) {
      this.componentType = ComponentType.Edit;
    }

    const categoryPromise = this.categoryService.getAll().then(categories => {
      this.categories = categories;
    });

    const idStr = route.snapshot.paramMap.get('id');
    if (!idStr) {
      return;
    }

    categoryPromise.then(() => {
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

      productService.getImages(this.productId).then(imagesUrl => {
        this.imagesUrls = imagesUrl;
      });
    });
  }

  ngAfterViewInit() {
    this.fileInput.nativeElement.value = "";
  }

  updateProductForm(product: Product) {
    const category = this.categories.find(c => c.id === product.categoryId);
    if (!category) {
      console.error("Category not found");
      return;
    }
    this.productForm.setValue({
      name: product.name,
      brand: product.brand,
      description: product.description,
      specification: product.specification,
      category: category.id,
      netPrice: product.netPrice.toString(),
      sellPrice: product.sellPrice.toString(),
      quantityInStock: product.quantityInStock.toString()
    });
  }

  async onSubmit() {
    const value = this.productForm.value;
    if (!value.name || !value.brand || !value.description || !value.specification || !value.category ||
      !value.netPrice || !value.sellPrice || !value.quantityInStock) {
      return;
    }

    let product: ProductRequest = {
      name: value.name,
      brand: value.brand,
      description: value.description,
      specification: value.specification,
      categoryId: value.category,
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
      const addedProduct = await this.productService.create(product);
      await this.productService.updateImages(addedProduct.id, this.imagesUrls);

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
      await this.productService.updateImages(this.productId, this.imagesUrls);

      // Show toast
      this.toastComponent.show("Success", "Product updated successfully");
    } catch (e) {
      // Show toast
      this.toastComponent.show("Error", "Failed to update product");
      console.error(e);
    }
  }

  async onDelete(event: MouseEvent) {
    event.preventDefault();

    if (!this.productId) {
      this.toastComponent.show("Error", "Invalid product id");
      return;
    }

    try {
      // Delete product from database
      await this.productService.delete(this.productId);

      // Show toast
      this.toastComponent.show("Success", "Product deleted successfully");
      await this.router.navigate(['/admin/product-list']);
    } catch (e) {
      // Show toast
      this.toastComponent.show("Error", "Failed to delete product");
      console.error(e);
    }
  }

  protected readonly ComponentType = ComponentType;

  async onAddImageUrl() {
    this.imagesUrls.push(this.newImageUrl);
    this.newImageUrl = "";
  }

  onRemoveImage(url: string) {
    this.imagesUrls = this.imagesUrls.filter(u => u !== url);
  }

  async onFilesUpload() {
    const files = this.uploadFiles;
    if (files && files.length > 0) {
      const urls = await this.imageBlobService.addImages(files);
      this.uploadFiles = null;
      this.fileInput.nativeElement.value = "";
      this.imagesUrls.push(...urls);
    }
  }

  onSelectImages() {
    this.uploadFiles = this.fileInput.nativeElement.files;
    console.log(this.uploadFiles);
  }
}

enum ComponentType {
  Add,
  Edit
}
