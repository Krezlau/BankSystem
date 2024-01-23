import {Component, OnDestroy, OnInit, signal} from '@angular/core';
import {TransferHistoryComponent} from "../transfer-history/transfer-history.component";
import {AccountInfoComponent} from "../account-info/account-info.component";
import {UserSensitiveDataComponent} from "../user-sensitive-data/user-sensitive-data.component";
import {UserService} from "../../../services/user.service";
import {Observable} from "rxjs";
import ApiResponse from "../../../types/ApiResponse";
import Transfer from "../../../types/Transfer";
import BankAccount from "../../../types/BankAccount";
import UserSensitiveData from "../../../types/UserSensitiveData";
import {AsyncPipe} from "@angular/common";

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
  accountdata$: Observable<ApiResponse<BankAccount>> = new Observable<ApiResponse<BankAccount>>();
  transferHistory$: Observable<ApiResponse<Transfer[]>> = new Observable<ApiResponse<Transfer[]>>();
  userSensitiveData$: Observable<ApiResponse<UserSensitiveData>> = new Observable<ApiResponse<UserSensitiveData>>();

  userLoading = signal(false);
  accountLoading = signal(false);
  transfersLoading = signal(false);

  constructor(private userService: UserService) {
    this.userLoading = userService.userLoading;
    this.accountLoading = userService.accountLoading;
    this.transfersLoading = userService.transfersLoading;
  }

  ngOnInit(): void {
    this.transferHistory$ = this.userService.getTransfers();
    this.accountdata$ = this.userService.getAccount();
    this.userSensitiveData$ = this.userService.getSensitiveData();
  }

  ngOnDestroy(): void {
  }
}
