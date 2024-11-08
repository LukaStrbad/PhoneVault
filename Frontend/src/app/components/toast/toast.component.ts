import { Component, ElementRef, signal, ViewChild } from '@angular/core';
import { NgClass } from "@angular/common";

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [
    NgClass
  ],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.scss'
})
export class ToastComponent {
  title = "";
  message = "";
  toastState = signal(ToastState.Hidden);

  show(title: string, message: string) {
    console.log(title, message);
    this.title = title;
    this.message = message;
    this.toastState.set(ToastState.Shown);

    setTimeout(() => {
      this.close();
    }, 5000);
  }

  close() {
    this.toastState.set(ToastState.Hidden);
  }

  protected readonly ToastState = ToastState;
}

enum ToastState {
  Hidden,
  Shown
}
