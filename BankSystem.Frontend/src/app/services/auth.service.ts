import {Injectable, signal} from '@angular/core';
import {finalize, Subscription, tap} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import moment from "moment";
import {AlertService} from "./alert.service";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _authState = {
    username: '',
    authToken: '',
    isLoggedIn: false,
  };
  private xd: Subscription = new Subscription();

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

  login(username: string, password: string) {
    this.xd = this.sendLoginRequest(username, password).subscribe();
  }

  sendLoginRequest(username: string, password: string) {
    this.isLoading.set(true);
    return this.http
      .post<IAuthResponse>('http://localhost:5077/api/auth/login', {
        username,
        password,
      })
      .pipe(
        finalize(() => {
          this.isLoading.set(false);
          this.xd.unsubscribe();
        }),
        tap(
          (res) => this.handleAuthResponse(res),
          (error) => this.handleAuthError(error),
        ),
      );
  }

  logout() {
    this._authState = {
      username: '',
      authToken: '',
      isLoggedIn: false,
    };
    this._authStateSignal.set(this._authState);
    localStorage.removeItem('authState');
    this.alertService.show('You have been logged out.', 'info');
    this.router.navigate(['/login']);
  }

  register(username: string, password: string, email: string) {
    this.xd = this.sendRegisterRequest(username, password, email).subscribe();
  }

  sendRegisterRequest(username: string, password: string, email: string) {
    this.isLoading.set(true);
    return this.http
      .post<IAuthResponse>('http://localhost:5077/api/auth/register', {
        username,
        password,
        email,
      })
      .pipe(
        finalize(() => {
          this.isLoading.set(false);
          this.xd.unsubscribe();
        }),
        tap(
          (res) => this.handleAuthResponse(res, true),
          (error) => this.handleAuthError(error),
        ),
      );
  }

  private handleAuthError(err: any) {
    console.log(err);
    if (err.error.errorMessage) {
      this.alertService.show(err.error.errorMessage, 'error');
    }
    else if (err.error.errors && err.error.errors.length > 0) {
      this.alertService.show(err.error.errors[0].defaultMessage, 'error');
    }
    else {
      this.alertService.show('An unknown error has occured. Please try again.', 'error');
    }
  }

  private handleAuthResponse(res: IAuthResponse, isRegister = false) {
    console.log('Response: ', res);
    this._authState = {
      username: res.username,
      authToken: res.token,
      isLoggedIn: true,
    };
    this._authStateSignal.set(this._authState);
    localStorage.setItem('authState', JSON.stringify(this._authState));
    const expiresAt = moment().add(1, 'hour');
    localStorage.setItem('expires_at', expiresAt.toISOString());
    this.router.navigate(['/book-catalog']);
    if (isRegister) {
      this.alertService.show('Registered successfully!', 'success');
    } else {
      this.alertService.show('Logged in successfully!', 'success');
    }
  }
}

export interface IAuthState {
  username: string;
  authToken: string;
  isLoggedIn: boolean;
}

export interface IAuthResponse {
  username: string;
  token: string;
}
