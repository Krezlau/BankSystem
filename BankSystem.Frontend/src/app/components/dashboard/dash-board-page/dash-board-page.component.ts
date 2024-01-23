import {Component, OnDestroy, OnInit, signal} from '@angular/core';
import {TransferHistoryComponent} from "../transfer-history/transfer-history.component";
import {AccountInfoComponent} from "../account-info/account-info.component";
import {UserSensitiveDataComponent} from "../user-sensitive-data/user-sensitive-data.component";
import {UserService} from "../../../services/user.service";
import {Subscription} from "rxjs";
import ApiResponse from "../../../types/ApiResponse";
import Transfer from "../../../types/Transfer";
import BankAccount from "../../../types/BankAccount";
import UserSensitiveData from "../../../types/UserSensitiveData";
import {AsyncPipe} from "@angular/common";
import {AuthService} from "../../../services/auth.service";
import {FormControl, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {AlertService} from "../../../services/alert.service";

@Component({
  selector: 'app-dash-board-page',
  standalone: true,
  imports: [
    TransferHistoryComponent,
    AccountInfoComponent,
    UserSensitiveDataComponent,
    AsyncPipe,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './dash-board-page.component.html'
})
export class DashBoardPageComponent implements OnInit, OnDestroy{
  accountdata: ApiResponse<BankAccount> | null = null;
  transferHistory: ApiResponse<Transfer[]> | null = null;
  userSensitiveData: ApiResponse<UserSensitiveData> | null = null;

  userLoading = signal(false);
  accountLoading = signal(false);
  transfersLoading = signal(false);

  sub = new Subscription();

  AccountNumberControl = new FormControl('',
    [
    Validators.minLength(32),
    Validators.maxLength(32),
    Validators.required,
    Validators.pattern('[A-Z0-9]{32}')
    ]);

  AmountControl = new FormControl(20.00, [Validators.required, Validators.min(0.01), Validators.max(1_000_000)]);

  TitleControl = new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength(25)]);

  RecipientControl = new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength(50)]);

  constructor(private userService: UserService, private authService: AuthService, private alertService: AlertService) {
    this.userLoading = userService.userLoading;
    this.accountLoading = userService.accountLoading;
    this.transfersLoading = userService.transfersLoading;
  }

  logout(): void {
    this.authService.logout();
  }

  resetForm(): void {
    this.AccountNumberControl.reset();
    this.AmountControl.reset();
  }

  sumbitTransfer(): void {
    if (this.AmountControl.invalid || !this.AmountControl.value) {
      this.alertService.show("Invalid amount", "error");
      return;
    }
    if (this.AccountNumberControl.invalid || !this.AccountNumberControl.value) {
      this.alertService.show("Invalid account number", "error");
      return;
    }
    if (this.RecipientControl.invalid || !this.RecipientControl.value) {
      this.alertService.show("Invalid recipient", "error");
      return;
    }
    if (this.TitleControl.invalid || !this.TitleControl.value) {
      this.alertService.show("Invalid title", "error");
      return;
    }

    this.sub.add(this.userService.sendTransfer(this.AccountNumberControl.value, this.AmountControl.value, this.RecipientControl.value, this.TitleControl.value).subscribe(
      (data) => {
        this.alertService.show("Transfer sent", "success");
        this.accountdata!.data!.accountBalance! -= this.AmountControl.value!;
        this.sub.add(this.userService.getTransfers().subscribe((data) => this.transferHistory = data));
        this.resetForm();
      }
    ));

  }

  ngOnInit(): void {
    this.sub.add(this.userService.getTransfers().subscribe(
      (data) => { this.transferHistory = data; }
    ));
    this.sub.add(this.userService.getAccount().subscribe(
      (data) => { this.accountdata = data; }
    ));
    this.sub.add(this.userService.getSensitiveData().subscribe(
      (data) => { this.userSensitiveData = data; }
    ));
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
