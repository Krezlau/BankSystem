import {Component, Input, OnInit} from '@angular/core';
import {NgClass} from "@angular/common";

@Component({
  selector: 'app-password-strength-indicator',
  standalone: true,
  imports: [
    NgClass
  ],
  templateUrl: './password-strength-indicator.component.html'
})
export class PasswordStrengthIndicatorComponent{
  @Input() strength = 0;

  constructor() {
  }
}
