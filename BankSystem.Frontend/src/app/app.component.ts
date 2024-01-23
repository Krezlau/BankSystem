import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import {AlertService} from "./services/alert.service";
import {AuthService} from "./services/auth.service";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './app.component.html',
})
export class AppComponent {
  title = 'BankSystem.Frontend';
  alert$ = this.alertService.getAlert();
  authState = this.authService.getAuthState();

  constructor(private alertService: AlertService, private authService: AuthService) {
  }

  ngOnInit() {
    this.authService.retrieveAuthState();
  }

  alertHide() {
    this.alertService.hide();
  }
}
