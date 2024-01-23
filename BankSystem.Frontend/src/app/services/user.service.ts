import {Injectable, signal} from '@angular/core';
import {AlertService} from "./alert.service";
import {HttpClient} from "@angular/common/http";
import ApiResponse from "../types/ApiResponse";
import Transfer from "../types/Transfer";
import {finalize, tap} from "rxjs";
import {AuthService} from "./auth.service";
import BankAccount from "../types/BankAccount";
import UserSensitiveData from "../types/UserSensitiveData";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  transfersLoading = signal(false);
  accountLoading = signal(false);
  userLoading = signal(false);
  sendTransferLoading = signal(false);

  constructor(private alertService: AlertService, private http: HttpClient, private authService: AuthService) { }

  sendTransfer(accountNumber: string, amount: number, recipient: string, title: string) {
    this.sendTransferLoading.set(true);
    return this.http.post<ApiResponse<Transfer>>('http://localhost:5077/api/transfers/send', {
      recipientBankAccountNumber: accountNumber,
      amount: amount,
      title: title,
      RecipientFullName: recipient,
    }, {headers: {
        Authorization: `Bearer ${this.authService.getAuthState()().authToken}`
      }}).pipe(
      finalize(() => this.sendTransferLoading.set(false)),
      tap((response) => {},
        (error) => {
        if (error.statusCode===401) this.authService.logout();
          this.alertService.show(error.error.message, 'error')
        }
    ))
  }

  getTransfers() {
    this.transfersLoading.set(true);
    return this.http.get<ApiResponse<Transfer[]>>('http://localhost:5077/api/transfers/history', {headers: {
        Authorization: `Bearer ${this.authService.getAuthState()().authToken}`
      }}).pipe(
      finalize(() => this.transfersLoading.set(false)),
      tap((response) => {},
        (error) => {
          if (error.statusCode===401) this.authService.logout();
          this.alertService.show(error.error.message, 'error')
        }
    ))
  }

  getAccount() {
    this.accountLoading.set(true);
    return this.http.get<ApiResponse<BankAccount>>('http://localhost:5077/api/users/myaccount', {headers: {
        Authorization: `Bearer ${this.authService.getAuthState()().authToken}`
      }}).pipe(
      finalize(() => this.accountLoading.set(false)),
      tap((response) => {},
        (error) => {
          if (error.statusCode===401) this.authService.logout();
          this.alertService.show(error.error.message, 'error')
        }
    ))
  }

  getSensitiveData() {
    this.userLoading.set(true);
    return this.http.get<ApiResponse<UserSensitiveData>>('http://localhost:5077/api/users/mysensitivedata', {headers: {
        Authorization: `Bearer ${this.authService.getAuthState()().authToken}`
      }}).pipe(
      finalize(() => this.userLoading.set(false)),
      tap((response) => {},
        (error) => {
          if (error.statusCode===401) this.authService.logout();
          this.alertService.show(error.error.message, 'error')
        }
    ))
  }
}
