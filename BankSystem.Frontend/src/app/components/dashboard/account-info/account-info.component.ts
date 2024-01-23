import {Component, Input} from '@angular/core';
import ApiResponse from "../../../types/ApiResponse";
import BankAccount from "../../../types/BankAccount";
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-account-info',
  standalone: true,
  imports: [
    NgIf
  ],
  templateUrl: './account-info.component.html'
})
export class AccountInfoComponent {
  @Input() accountdata: ApiResponse<BankAccount> | null = null;
  @Input() accountLoading = false;
  @Input() userLoading = false;
  @Input() firstName = "";
  @Input() lastName = "";
}
