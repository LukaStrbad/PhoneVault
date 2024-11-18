import {
  afterNextRender,
  Inject,
  Injectable,
  OnInit,
  PLATFORM_ID,
  Signal,
  signal,
  WritableSignal
} from '@angular/core';
import { environment } from "../environments/environment";
import { HttpClient } from "@angular/common/http";
import { User } from "../model/user";
import { jwtDecode, JwtPayload } from "jwt-decode";
import { firstValueFrom } from "rxjs";
import {
  GoogleAuthProvider,
  getAuth,
  signInWithPopup,
  onAuthStateChanged,
  User as FirebaseUser,
  signOut
} from "firebase/auth";
import { isPlatformBrowser } from "@angular/common";

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
  private token: DecodedTokenBase | null = null;
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

  get isGoogleUser() {
    return this.token?.isGoogle === true;
  }

  constructor(
    @Inject(PLATFORM_ID) private platformId: Object,
    private http: HttpClient
  ) {
    afterNextRender(() => {
      const accessToken = this.accessToken;
      if (accessToken) {
        try {
          const decodedToken = this.setUserAndToken(accessToken);

          const expired = decodedToken.exp * 1000 < Date.now();
          if (expired) {
            this.refreshTokens().then(() => {
              this._isUserLoggedIn.set(true);
            }, () => {
              this.logout();
            });
          } else {
            this._isUserLoggedIn.set(true);
          }
        } catch (e) {
          // Ignored
        }
      }

      const auth = getAuth();
      onAuthStateChanged(auth, async user => {
        if (user) {
          console.log(user);
          await this.setGoogleUserAndToken(user);
          this._isUserLoggedIn.set(true);
        }
      });
    });
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

  async setGoogleUserAndToken(user: FirebaseUser) {
    const accessToken = await user.getIdToken(true);
    const decodedToken = jwtDecode(accessToken);
    if ("name" in decodedToken && "user_id" in decodedToken && "email" in decodedToken) {
      this.token = decodedToken as DecodedGoogleToken;
      this.token.isGoogle = true;
      this.setUser(this.token);
      localStorage.setItem('accessToken', accessToken);

      return this.token;
    } else {
      throw new Error(`Invalid access token: ${accessToken}`);
    }
  }

  private setUser(token: DecodedTokenBase) {
    if (token.isGoogle) {
      let decodedToken = token as DecodedGoogleToken;
      this.user = {
        id: decodedToken.user_id,
        name: decodedToken.name,
        email: decodedToken.email,
        userType: "Customer"
      };
    } else {
      let decodedToken = token as DecodedToken;
      const userType = decodedToken.role === "Admin" ? "Admin" : "Customer";
      this.user = {
        id: decodedToken.id,
        name: decodedToken.given_name,
        email: decodedToken.email,
        userType
      };
    }
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

  logout() {
    if (this.isGoogleUser) {
      const auth = getAuth();
      signOut(auth).catch(console.error);
    }
    this.accessToken = null;
  }

  async onGoogleLogin() {
    const provider = new GoogleAuthProvider();
    const auth = getAuth();
    try {
      const result = await signInWithPopup(auth, provider);
    } catch (e) {
      console.error(e);
    }
  }

}

interface DecodedTokenBase extends JwtPayload {
  isGoogle?: boolean;
  exp: number;
}

interface DecodedToken extends DecodedTokenBase {
  id: string;
  unique_name: string;
  given_name: string;
  email: string;
  role?: string;
}

interface DecodedGoogleToken extends DecodedTokenBase {
  name: string;
  user_id: string;
  email: string;
}
