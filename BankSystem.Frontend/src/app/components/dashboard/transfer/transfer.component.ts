import {Component, Input, OnInit} from '@angular/core';
import Transfer from "../../../types/Transfer";
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-transfer',
  standalone: true,
  imports: [
    NgIf
  ],
  templateUrl: './transfer.component.html'
})
export class TransferComponent {
  @Input() transfer: Transfer | null = null;
  @Input() accountId: string = '';
}
