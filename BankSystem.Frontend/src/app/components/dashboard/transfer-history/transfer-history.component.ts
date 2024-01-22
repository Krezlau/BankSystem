import { Component } from '@angular/core';
import {TransferComponent} from "../transfer/transfer.component";
import {NgForOf} from "@angular/common";

@Component({
  selector: 'app-transfer-history',
  standalone: true,
  imports: [
    TransferComponent,
    NgForOf
  ],
  templateUrl: './transfer-history.component.html'
})
export class TransferHistoryComponent {
  transfers: number[] = [1, 2, 3];

}
