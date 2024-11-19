import { Component, effect, ViewChild } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from "@angular/forms";
import { UserService } from "../../services/user.service";
import { EmailSetting } from "../../model/email-settings";
import { AuthService } from "../../services/auth.service";
import { ToastComponent } from "../components/toast/toast.component";
import { ExchangeRateService } from "../../services/exchange-rate.service";

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    ToastComponent,
    FormsModule
  ],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss'
})
export class SettingsComponent {
  emailSettingsForm = new FormGroup({
    order: new FormControl(false),
    newProduct: new FormControl(false),
  });
  currencies: string[] = ['EUR'];

  @ViewChild('toast') private toastComponent!: ToastComponent;

  constructor(
    private userService: UserService,
    public authService: AuthService,
    public exchangeRate: ExchangeRateService
  ) {

    exchangeRate.getExchangeRate().then(rate => {
      this.currencies.push(...rate.map(r => r.valuta));
    })

    effect(() => {
      if (authService.isUserLoggedIn()) {
        userService.getEmailSettings().then(emailSettings => {
          this.emailSettingsForm.setValue({
            order: emailSettings.includes("Order"),
            newProduct: emailSettings.includes("NewProduct")
          });
        });
      }
    });

  }

  async onSubmit() {
    const emailSettings: EmailSetting[] = [];
    if (this.emailSettingsForm.get('order')?.value) {
      emailSettings.push('Order');
    }
    if (this.emailSettingsForm.get('newProduct')?.value) {
      emailSettings.push('NewProduct');
    }
    try {
      await this.userService.updateEmailSettings(emailSettings);
      this.toastComponent.show('Succes', 'Email settings updated successfully');
    } catch (e) {
      this.toastComponent.show('Error', 'Failed to update email settings');
    }
  }

  get colorTheme(): string {
    return localStorage.getItem('theme') ?? 'auto';
  }

  set colorTheme(value: string) {
    console.log('Setting theme to', value);
    if (value != 'auto' && value != 'dark' && value != 'light') {
      value = 'light';
    }
    localStorage.setItem('theme', value);

    // Set data-bs-theme
    if (value == 'auto') {
      const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
      document.documentElement.setAttribute('data-bs-theme', prefersDark ? 'dark' : 'light');
    } else {
      document.documentElement.setAttribute('data-bs-theme', value);
    }
  }

  onCurrencyChange($event: Event) {
    const target = $event.target as HTMLSelectElement;
    this.exchangeRate.selectedCurrency = target.value;
    console.log('Currency changed to', target.value);
  }
}
