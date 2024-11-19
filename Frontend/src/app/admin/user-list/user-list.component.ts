import { Component, ViewChild } from '@angular/core';
import { User } from "../../../model/user";
import { UserService } from "../../../services/user.service";
import { DatePipe } from "@angular/common";
import { RouterLink } from "@angular/router";
import { AuthService } from "../../../services/auth.service";
import { ToastComponent } from "../../components/toast/toast.component";

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [
    DatePipe,
    RouterLink,
    ToastComponent
  ],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss'
})
export class UserListComponent {
  users: User[] = [];

  @ViewChild('toast') private toastComponent!: ToastComponent;

  constructor(
    private userService: UserService,
    public auth: AuthService
  ) {
    userService.getAll().then(users => {
      this.users = users;
    });
  }

  async promoteAdmin(id: string) {
    try {
      await this.userService.update(id, true);
      this.toastComponent.show("Success", "User promoted to admin");
      this.users = await this.userService.getAll();
    } catch (e) {
      this.toastComponent.show("Error", "Failed to promote user to admin");
    }
  }
}
