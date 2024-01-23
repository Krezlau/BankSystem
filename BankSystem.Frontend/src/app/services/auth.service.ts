import {Injectable, signal} from '@angular/core';
import {finalize, Subscription, tap} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import moment from "moment";
import {AlertService} from "./alert.service";
import ApiResponse from "../types/ApiResponse";
import LoginCheckResponse from "../types/LoginCheckResponse";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _authState = {
    authToken: '',
    isLoggedIn: false,
    userId: '',
  };
  private _authStateSignal = signal(this._authState);

  isLoading = signal(false);

  constructor(
    private http: HttpClient,
    private router: Router,
    private alertService: AlertService,
  ) {}

  retrieveAuthState(): void {
    const expirationDate = localStorage.getItem('expires_at');
    if (
      expirationDate &&
      moment()
        .add(5 * 60, 'second')
        .isBefore(moment(expirationDate))
    ) {
      const state = localStorage.getItem('authState');
      if (state === null) {
        return;
      }
      this._authState = JSON.parse(state);
      this._authStateSignal.set(this._authState);
      console.log('authState', this._authState);
    }
  }

  getAuthState() {
    return this._authStateSignal;
  }

  loginCheck(email: string) {
    this.isLoading.set(true);
    return this.http.post<ApiResponse<LoginCheckResponse>>("http://localhost:5077/api/auth/login-check", {email: email}).pipe(
      finalize(() => {
        this.isLoading.set(false);
      }),
      tap(
        (res) => {
          console.log(res);
        },
        (error) => {
          console.log(error);
          this.alertService.show('An unknown error has occured. Please try again.', 'error');
        },
      ),
    );
  }

  sendLoginRequest(email: string, key: string, passwordCharacters: string) {
    this.isLoading.set(true);
    return this.http
      .post<ApiResponse<IAuthResponse>>('http://localhost:5077/api/auth/login', {
        email, key, passwordCharacters
      })
      .pipe(
        finalize(() => {
          this.isLoading.set(false);
        }),
        tap(
          (res) => this.handleAuthResponse(res),
          (error: IAuthResponse) => this.handleAuthError(error),
        ),
      );
  }

  logout() {
    this._authState = {
      authToken: '',
      isLoggedIn: false,
      userId: '',
    };
    this._authStateSignal.set(this._authState);
    localStorage.removeItem('authState');
    this.alertService.show('You have been logged out.', 'info');
    this.router.navigate(['/']);
  }

  sendRegisterRequest(email: string, firstName: string,  lastName: string, password: string, idNumber: string, phoneNumber: string) {
    this.isLoading.set(true);
    return this.http
      .post<ApiResponse<IAuthResponse>>('http://localhost:5077/api/auth/register', {
        email, firstName, lastName, password, idNumber, phoneNumber
      })
      .pipe(
        finalize(() => {
          this.isLoading.set(false);
        }),
        tap(
          (res) => this.handleAuthResponse(res, true),
          (error) => this.handleAuthError(error),
        ),
      );
  }

  private handleAuthError(err: any) {
    const response = err.error as ApiResponse<IAuthResponse>;
    if (!response.data) {
      this.alertService.show(response.message, 'error');
    }
    else {
      this.alertService.show(response.data.tryCountMessage!, 'error');
    }
    this.router.navigate(["/sign-in"]);
  }

  private handleAuthResponse(res: ApiResponse<IAuthResponse>, isRegister = false) {
    if (!isRegister && (!res.data || !res.data.success)) {
      console.log("xd")
      const data = res.data as IAuthResponse;
      if (data.tryCountMessage)
        this.alertService.show(data.tryCountMessage, 'error');
      else
        this.alertService.show(data.message!, 'error');
      this.router.navigate(["/sign-in"]);
      throw res;
    }

    const resp = res.data as IAuthResponse;
    this._authState = {
      authToken: resp.token,
      isLoggedIn: true,
      userId: resp.userId,
    };
    this._authStateSignal.set(this._authState);
    localStorage.setItem('authState', JSON.stringify(this._authState));
    const expiresAt = moment().add(1, 'hour');
    localStorage.setItem('expires_at', expiresAt.toISOString());
    if (isRegister) {
      this.alertService.show('Registered successfully!', 'success');
      this.router.navigate(['/sign-in']);
    } else {
      this.alertService.show('Logged in successfully!', 'success');
      this.router.navigate(['/dashboard']);
    }
  }
}

export interface IAuthState {
  authToken: string;
  isLoggedIn: boolean;
  userId: string;
}

export interface IAuthResponse {
  success: boolean;
  token: string;
  userId: string;
  message?: string;
  tryCountMessage?: string;
}
