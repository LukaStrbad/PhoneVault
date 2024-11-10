import { Routes } from '@angular/router';
import { LoginComponent } from "./login/login.component";
import { RegisterComponent } from "./register/register.component";
import { NotFoundComponent } from "./not-found/not-found.component";
import { HomeComponent } from "./home/home.component";
import { ProductComponent } from "./product/product.component";
import { AdminComponent } from "./admin/admin.component";
import { AddProductComponent } from "./admin/add-product/add-product.component";
import { ProductListComponent } from "./admin/product-list/product-list.component";
import { ShoppingCartComponent } from "./shopping-cart/shopping-cart.component";

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'register',
    component: RegisterComponent,
  },
  {
    path: 'shopping-cart',
    component: ShoppingCartComponent,
  },
  {
    path: 'product/:id',
    component: ProductComponent,
  },
  {
    path: 'admin',
    component: AdminComponent,
    children: [
      {
        path: 'product-list',
        component: ProductListComponent,
      },
      {
        path: 'add-product',
        component: AddProductComponent,
      },
      {
        path: 'edit-product/:id',
        component: AddProductComponent,
      }
    ]
  },
  {
    path: '**',
    component: NotFoundComponent,
  }
];
