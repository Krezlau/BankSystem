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

@Component({
  selector: 'app-dash-board-page',
  standalone: true,
  imports: [
    TransferHistoryComponent,
    AccountInfoComponent,
    UserSensitiveDataComponent,
    AsyncPipe
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

  constructor(private userService: UserService, private authService: AuthService) {
    this.userLoading = userService.userLoading;
    this.accountLoading = userService.accountLoading;
    this.transfersLoading = userService.transfersLoading;
  }

  logout(): void {
    this.authService.logout();
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
