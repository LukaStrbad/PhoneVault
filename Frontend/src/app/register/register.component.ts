import { Component } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from "@angular/forms";
import { NgClass } from "@angular/common";
import { AuthService } from "../../services/auth.service";
import { Router } from "@angular/router";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  registrationForm = new FormGroup({
    name: new FormControl('', Validators.required),
    email: new FormControl('', Validators.email),
    password: new FormControl('', Validators.minLength(8)),
    confirmPassword: new FormControl('', Validators.required)
  }, { validators: this.passwordMatchValidator });

  passwordMatchValidator(formGroup: AbstractControl): ValidationErrors | null {
    const password = formGroup.get('password');
    const confirmPassword = formGroup.get('confirmPassword');
    return password && confirmPassword && password.value !== confirmPassword.value ? { passwordMismatch: true } : null;
  }

  constructor(
    private auth: AuthService,
    private router: Router
  ) {
  }

  async onSubmit() {
    const value = this.registrationForm.value;
    if (!value.name || !value.email || !value.password || !value.confirmPassword) {
      return;
    }

    await this.auth.register(value.name, value.email, value.password);
    await this.router.navigate(['/']);
  }
}
