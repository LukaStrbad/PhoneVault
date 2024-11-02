import { Injectable, Signal, signal, WritableSignal } from '@angular/core';
import { environment } from "../environments/environment";
import { HttpClient } from "@angular/common/http";
import { User } from "../model/user";
import { jwtDecode, JwtPayload } from "jwt-decode";
import { firstValueFrom } from "rxjs";

const url = `${environment.apiUrl}/auth`;

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  /**
   * The currently logged-in user, or null if no user is logged in.
   */
  user: User | null = null;
  /**
   * The authentication token, or null if no user is logged in.
   */
  private token: DecodedToken | null = null;
  private _isUserLoggedIn: WritableSignal<boolean> = signal(false);
  public isUserLoggedIn: Signal<boolean> = this._isUserLoggedIn;

  get accessToken() {
    if (typeof localStorage === 'undefined') {
      return null;
    }
    return localStorage.getItem('accessToken');
  }

  set accessToken(accessToken: string | null) {
    if (accessToken) {
      this.setUserAndToken(accessToken);
      this._isUserLoggedIn.set(true);
    } else {
      this.user = null;
      this.token = null;
      localStorage.removeItem('accessToken');
      this._isUserLoggedIn.set(false);
    }
  }

  constructor(
    private http: HttpClient
  ) {
    const accessToken = this.accessToken;
    if (accessToken) {
      const decodedToken = this.setUserAndToken(accessToken);

      const expired = decodedToken.exp * 1000 < Date.now();

      if (expired) {
        this.refreshTokens().then(() => {
          this._isUserLoggedIn.set(true);
          console.log("tokens refreshed");
        }, () => {
          this.logout().then();
        });
      } else {
        this._isUserLoggedIn.set(true);
      }
    }
  }

  private setUserAndToken(accessToken: string) {
    const decodedToken = jwtDecode(accessToken);
    // Check if token has the required fields (role is optional)
    if ("id" in decodedToken && "unique_name" in decodedToken && "given_name" in decodedToken && "email" in decodedToken) {
      this.token = decodedToken as DecodedToken;
      this.setUser(this.token);
      localStorage.setItem('accessToken', accessToken);

      return this.token;
    } else {
      throw new Error(`Invalid access token: ${accessToken}`);
    }
  }

  private setUser(token: DecodedToken) {
    this.user = {
      id: parseInt(token.id),
      name: token.given_name,
      email: token.email
    };
  }

  async login(email: string, password: string) {
    this.accessToken = await firstValueFrom(this.http.post(`${url}/login`, {
      email,
      password
    }, { withCredentials: true, responseType: 'text' }));
  }

  async register(name: string, email: string, password: string) {
    this.accessToken = await firstValueFrom(this.http.post(`${url}/register`, {
      name,
      email,
      password
    }, { withCredentials: true, responseType: 'text' }));
  }

  async refreshTokens() {
    this.accessToken = await firstValueFrom(
      this.http.post(`${url}/refreshAccessToken`, this.accessToken,
        { withCredentials: true, responseType: 'text' })
    );
  }

  async logout() {
    this.accessToken = null;
  }

}


interface DecodedToken extends JwtPayload {
  id: string;
  unique_name: string;
  given_name: string;
  email: string;
  role?: string | string[];
  exp: number;
}
