import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { AuthService } from "../../services/auth.service";
import { log } from "node:util";
import { NgClass } from "@angular/common";
import { Router } from "@angular/router";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm = new FormGroup({
    email: new FormControl('', Validators.email),
    password: new FormControl('', Validators.required)
  });

  constructor(
    private auth: AuthService,
    private router: Router
  ) {
  }

  async onSubmit() {
    console.log("login");
    const value = this.loginForm.value;
    if (!value.email || !value.password) {
      return;
    }

    await this.auth.login(value.email, value.password);
    await this.router.navigate(['/']);
  }

  async onGoogleLogin() {
    try {
      await this.auth.onGoogleLogin();
      await this.router.navigate(['/']);
    } catch (e) {
      console.error(e);
    }
  }
}
