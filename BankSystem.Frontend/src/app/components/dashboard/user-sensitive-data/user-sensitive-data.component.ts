import {Component, Input} from '@angular/core';
import ApiResponse from "../../../types/ApiResponse";
import UserSensitiveData from "../../../types/UserSensitiveData";
import BankAccount from "../../../types/BankAccount";
import {NgClass, NgIf} from "@angular/common";

@Component({
  selector: 'app-user-sensitive-data',
  standalone: true,
  imports: [
    NgClass,
    NgIf
  ],
  templateUrl: './user-sensitive-data.component.html'
})
export class UserSensitiveDataComponent {
  @Input() userSensitiveData: ApiResponse<UserSensitiveData> | null = null;
  @Input() userLoading = false;
  @Input() accountdata: ApiResponse<BankAccount> | null = null;
  @Input() accountLoading = false;

  cvvVisible = false;

  toggleCvvVisible() {
    this.cvvVisible = !this.cvvVisible;
  }

  getCardNumber(): string {
    if (this.accountdata?.data) {
      return this.accountdata.data.cardNumber.match(/.{1,4}/g)?.join(' ') ?? '';
    }
    return '';
  }

  getPhoneNumber(): string {
    if (this.userSensitiveData?.data) {
      return this.userSensitiveData.data.phoneNumber.match(/.{1,3}/g)?.join(' ') ?? '';
    }
    return '';
  }

  protected readonly SVGFETurbulenceElement = SVGFETurbulenceElement;
}
