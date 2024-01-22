import { Component } from '@angular/core';
import {TransferHistoryComponent} from "../transfer-history/transfer-history.component";
import {AccountInfoComponent} from "../account-info/account-info.component";
import {UserSensitiveDataComponent} from "../user-sensitive-data/user-sensitive-data.component";

@Component({
  selector: 'app-dash-board-page',
  standalone: true,
  imports: [
    TransferHistoryComponent,
    AccountInfoComponent,
    UserSensitiveDataComponent
  ],
  templateUrl: './dash-board-page.component.html'
})
export class DashBoardPageComponent {

}
