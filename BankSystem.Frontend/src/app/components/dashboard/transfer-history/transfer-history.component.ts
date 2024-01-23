import {Component, Input} from '@angular/core';
import {TransferComponent} from "../transfer/transfer.component";
import {NgForOf, NgIf} from "@angular/common";
import ApiResponse from "../../../types/ApiResponse";
import Transfer from "../../../types/Transfer";

@Component({
  selector: 'app-transfer-history',
  standalone: true,
  imports: [
    TransferComponent,
    NgForOf,
    NgIf
  ],
  templateUrl: './transfer-history.component.html'
})
export class TransferHistoryComponent {
  @Input() transfersLoading: boolean = false;
  @Input() transferHistory: ApiResponse<Transfer[]> | null = null;
  @Input() accountId: string = '';
}
