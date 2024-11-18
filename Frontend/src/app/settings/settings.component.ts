import { Component, effect, ViewChild } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from "@angular/forms";
import { UserService } from "../../services/user.service";
import { EmailSetting } from "../../model/email-settings";
import { AuthService } from "../../services/auth.service";
import { ToastComponent } from "../components/toast/toast.component";

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    ToastComponent
  ],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss'
})
export class SettingsComponent {
  emailSettingsForm = new FormGroup({
    order: new FormControl(false),
    newProduct: new FormControl(false),
  });

  @ViewChild('toast') private toastComponent!: ToastComponent;

  constructor(
    private userService: UserService,
    public authService: AuthService
  ) {

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
}
