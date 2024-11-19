import { Component, ViewChild } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { ToastComponent } from "../../components/toast/toast.component";
import { ActivatedRoute, Router } from "@angular/router";
import { CategoryService } from "../../../services/category.service";
import { Category } from "../../../model/category";
import { NgClass } from "@angular/common";

@Component({
  selector: 'app-add-category',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass,
    ToastComponent
  ],
  templateUrl: './add-category.component.html',
  styleUrl: './add-category.component.scss'
})
export class AddCategoryComponent {
  categoryForm = new FormGroup({
    name: new FormControl('', Validators.required),
    description: new FormControl('', Validators.required),
  });

  componentType = ComponentType.Add;
  categoryId: number | null = null;

  @ViewChild('toast') private toastComponent!: ToastComponent;

  constructor(
    private categoryService: CategoryService,
    route: ActivatedRoute,
    private router: Router
  ) {
    // Check edit mode
    if (router.url.includes('edit-category')) {
      this.componentType = ComponentType.Edit;
    }

    const idStr = route.snapshot.paramMap.get('id');
    if (!idStr) {
      return;
    }
    this.categoryId = Number(idStr);
    const state = router.getCurrentNavigation()?.extras.state;

    let category: Category = state?.["category"];

    if (category) {
      this.updateCategoryForm(category);
    } else {
      categoryService.get(this.categoryId).then(p => {
        this.updateCategoryForm(p);
      });
    }
  }

  updateCategoryForm(category: Category) {
    this.categoryForm.setValue({
      name: category.name,
      description: category.description,
    });
  }

  async onSubmit() {
    const value = this.categoryForm.value;
    if (!value.name || !value.description) {
      return;
    }

    if (this.componentType == ComponentType.Add) {
      await this.createCategory(value.name, value.description);
    } else {
      if (!this.categoryId) {
        this.toastComponent.show("Error", "Invalid category id");
        return;
      }
      await this.updateCategory(this.categoryId, value.name, value.description);
    }

  }

  async createCategory(name: string, description: string) {
    try {
      await this.categoryService.create(name, description);
      this.toastComponent.show("Success", "Category created successfully");
      this.categoryForm.reset();
    } catch (e) {
      this.toastComponent.show("Error", "Failed to create category");
      console.error(e);
    }
  }

  async updateCategory(id: number, name: string, description: string) {
    try {
      await this.categoryService.update(id, name, description);
      this.toastComponent.show("Success", "Category updated successfully");
    } catch (e) {
      this.toastComponent.show("Error", "Failed to update category");
      console.error(e);
    }
  }

  protected readonly ComponentType = ComponentType;

  async onDelete(event: MouseEvent) {
    event.preventDefault();

    if (!this.categoryId) {
      this.toastComponent.show("Error", "Invalid category id");
      return;
    }

    try {
      await this.categoryService.delete(this.categoryId);
      this.toastComponent.show("Success", "Category deleted successfully");
      await this.router.navigate(['/admin/category-list']);
    } catch (e) {
      this.toastComponent.show("Error", "Failed to delete category");
      console.error(e);
    }
  }
}

enum ComponentType {
  Add,
  Edit
}
