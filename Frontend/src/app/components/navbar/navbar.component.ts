import { Component } from '@angular/core';
import { RouterLink } from "@angular/router";
import { AuthService } from "../../../services/auth.service";

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  constructor(public auth: AuthService) {
  }

}
