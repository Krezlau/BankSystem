import { Component } from '@angular/core';
import {FormControl, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {AlertService} from "../../../services/alert.service";
import {AuthService} from "../../../services/auth.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-sign-up-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './sign-up-form.component.html'
})
export class SignUpFormComponent {
  sub = new Subscription();

  emailControl = new FormControl('', [Validators.required, Validators.email, Validators.maxLength(50)]);
  passwordControl = new FormControl('', [Validators.required, Validators.minLength(12)]);
  confirmPasswordControl = new FormControl('', [Validators.required, Validators.minLength(12)]);
  firstNameControl = new FormControl('', [Validators.required, Validators.pattern('^[a-zA-ZĘęÓóĄąŚśŁłŻżŹźĆćŃń ,.\'-]+$')]);
  lastNameControl = new FormControl('', [Validators.required, Validators.pattern('^[a-zA-ZĘęÓóĄąŚśŁłŻżŹźĆćŃń ,.\'-]+$')]);
  phoneControl = new FormControl('', [Validators.required, Validators.pattern('^[0-9]{9}$')]);
  idControl = new FormControl('', [Validators.required, Validators.pattern(/^[A-Z]{3}\s[\d]{6}$/)]);

  constructor(private alertService: AlertService, private authService: AuthService) {
  }


  onSubmit() {
    if (this.emailControl.invalid || !this.emailControl.value){
      this.alertService.show('Email invalid', 'error');
      return;
    }
    if (this.passwordControl.invalid || !this.passwordControl.value || this.confirmPasswordControl.invalid || !this.confirmPasswordControl.value) {
      this.alertService.show('Password invalid', 'error');
      return;
    }
    if (this.passwordControl.value !== this.confirmPasswordControl.value) {
      this.alertService.show('Passwords do not match', 'error');
      return;
    }
    if (this.firstNameControl.invalid || !this.firstNameControl.value){
      this.alertService.show('First name invalid', 'error');
      return;
    }
    if (this.lastNameControl.invalid || !this.lastNameControl.value){
      this.alertService.show('Last name invalid', 'error');
      return;
    }
    if (this.phoneControl.invalid || !this.phoneControl.value){
      this.alertService.show('Phone number invalid', 'error');
      return;
    }
    if (this.idControl.invalid || !this.idControl.value){
      this.alertService.show('ID invalid', 'error');
      console.log(this.idControl.value)
      return;
    }

    this.sub = this.authService.sendRegisterRequest(
      this.emailControl.value,
      this.firstNameControl.value,
      this.lastNameControl.value,
      this.passwordControl.value,
      this.idControl.value,
      this.phoneControl.value,
    ).subscribe()
  }
}
