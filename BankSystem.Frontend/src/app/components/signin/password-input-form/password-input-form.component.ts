import {Component, OnInit} from '@angular/core';
import {NgForOf, NgIf} from "@angular/common";
import {AsapAction} from "rxjs/internal/scheduler/AsapAction";

@Component({
  selector: 'app-password-input-form',
  standalone: true,
  imports: [
    NgForOf,
    NgIf
  ],
  templateUrl: './password-input-form.component.html'
})
export class PasswordInputFormComponent {
  mask = "010101010100000000000010".split('')
  protected readonly AsapAction = AsapAction;
}
