import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { firstValueFrom } from "rxjs";
import { ExchangeRate } from "../model/exchange-rate";
import { environment } from "../environments/environment";

const url = `${environment.apiUrl}/api/ExchangeRate`;

@Injectable({
  providedIn: 'root'
})
export class ExchangeRateService {
  private _rate: ExchangeRate[] = [];

  constructor(
    private http: HttpClient
  ) {
  }

  get selectedCurrency() {
    return localStorage.getItem('selectedCurrency') || 'EUR';
  }

  set selectedCurrency(currency: string) {
    localStorage.setItem('selectedCurrency', currency);
  }

  async getExchangeRate() {
    if (this._rate.length > 0) {
      return this._rate;
    }
    this._rate = await firstValueFrom(this.http.get<ExchangeRate[]>(url));
    return this._rate;
  }

  async calculatePrice(priceEur: number) {
    if (this.selectedCurrency === 'EUR' || typeof window === 'undefined') {
      return priceEur;
    }

    const rate = (await this.getExchangeRate()).find(r => r.valuta === this.selectedCurrency);
    if (!rate) {
      throw new Error('Currency not found');
    }

    const rateNum = Number(rate.srednji_tecaj.replace(',', '.'));
    return Math.round(priceEur * rateNum * 100) / 100;
  }

  calculatePriceSync(priceEur: number) {
    if (this.selectedCurrency === 'EUR' || typeof window === 'undefined') {
      return priceEur;
    }

    const rate = this._rate.find(r => r.valuta === this.selectedCurrency);
    if (!rate) {
      throw new Error('Currency not found');
    }

    const rateNum = Number(rate.srednji_tecaj.replace(',', '.'));
    return Math.round(priceEur * rateNum * 100) / 100;
  }
}
