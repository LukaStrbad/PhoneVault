import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  inject,
  Optional,
  PLATFORM_ID,
  ViewEncapsulation
} from '@angular/core';
import { isPlatformServer } from "@angular/common";
import { RESPONSE } from "../server.token";
import { Response } from 'express';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [],
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.html'
})
export class NotFoundComponent {
  platformId = inject(PLATFORM_ID);
  constructor(
    @Optional() @Inject(RESPONSE) private response: Response
  ) {
    // Only executes server-side
    // This will also only work in production builds as "ng serve" doesn't use server.ts
    if (isPlatformServer(this.platformId)) {
      this.response?.status(404);
    }
  }
}
