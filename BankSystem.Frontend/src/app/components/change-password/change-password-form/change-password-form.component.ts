import {Component, OnDestroy, OnInit} from '@angular/core';
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {ActivatedRoute, Route, Router} from "@angular/router";
import {Subscription} from "rxjs";
import {AlertService} from "../../../services/alert.service";
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {AuthService} from "../../../services/auth.service";
import {
  PasswordStrengthIndicatorComponent
} from "../../password-strength-indicator/password-strength-indicator.component";

@Component({
  selector: 'app-change-password-form',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NgClass,
    NgForOf,
    NgIf,
    PasswordStrengthIndicatorComponent
  ],
  templateUrl: './change-password-form.component.html'
})
export class ChangePasswordFormComponent implements OnInit, OnDestroy {
  mask = "000000000000000000000000".split('')
  controls = "000000000000000000000000".split('')
  key: string = "";
  email: string = "";
  passwordControlGroup = new FormGroup({
    passwordControl1: new FormControl('', [Validators.required, Validators.maxLength(1)]),
    passwordControl2: new FormControl('', [Validators.required, Validators.maxLength(1)]),
    passwordControl3: new FormControl('', [Validators.required, Validators.maxLength(1)]),
    passwordControl4: new FormControl('', [Validators.required, Validators.maxLength(1)]),
    passwordControl5: new FormControl('', [Validators.required, Validators.maxLength(1)]),
  })

  newPasswordControl = new FormControl('', [Validators.required, Validators.minLength(12), Validators.maxLength(24)]);
  confirmPasswordControl = new FormControl('', [Validators.required, Validators.minLength(12), Validators.maxLength(24)]);

  sub = new Subscription();

  isLoading = this.authService.isLoading;

  constructor(private route: ActivatedRoute, private alertService: AlertService, private router: Router, private authService: AuthService) {
  }

  checkStrength(password: string): number {
    let strength = 0;
    if (password.length < 12) {
      return strength;
    }
    if (/[a-z]/.test(password)) {
      strength += 20;
    }
    if (/[A-Z]/.test(password)) {
      strength += 20;
    }
    if (/[0-9]/.test(password)) {
      strength += 20;
    }
    if (/[^a-zA-Z0-9]/.test(password)) {
      strength += 20;
    }
    if (password.length > 20) {
      strength += 20;
    }
    return strength;
  }

  onFormSubmit(event: Event) {
    if (this.passwordControlGroup.invalid) {
      this.alertService.show("Please type the characters", "error");
      return;
    }
    if (this.newPasswordControl.invalid || !this.newPasswordControl.value || this.confirmPasswordControl.invalid || !this.confirmPasswordControl.value) {
      this.alertService.show('Password invalid', 'error');
      return;
    }
    if (this.newPasswordControl.value !== this.confirmPasswordControl.value) {
      this.alertService.show('Passwords do not match', 'error');
      return;
    }
    if (this.checkStrength(this.newPasswordControl.value) < 60) {
      this.alertService.show('Password too weak', 'error');
      return;
    }
    let password = "";
    password += this.passwordControlGroup.value.passwordControl1;
    password += this.passwordControlGroup.value.passwordControl2;
    password += this.passwordControlGroup.value.passwordControl3;
    password += this.passwordControlGroup.value.passwordControl4;
    password += this.passwordControlGroup.value.passwordControl5;

    this.sub = this.authService.changePassword(this.key, password, this.newPasswordControl.value).subscribe();
  }

  ngOnInit() {
    const key = this.route.snapshot.paramMap.get('key');
    if (key === null) {
      this.alertService.show("Invalid link", "error");
      this.router.navigate(['/sign-in']);
      return;
    }
    this.key = key;

    this.sub = this.route.queryParams.subscribe(params => {
      if (params['email'] === undefined){
        this.alertService.show("Invalid link", "error");
        this.router.navigate(['/sign-in']);
        return;
      }
      this.email = params['email'];
      if (params['mask'] === undefined) {
        this.alertService.show("Invalid link", "error");
        this.router.navigate(['/sign-in']);
        return;
      }
      this.mask = params['mask'].split('');
      let i = 1;
      for (let c = 0; c < this.mask.length; c++) {
        if (this.mask[c] === '1')
        {
          this.controls[c] = i.toString();
          i++;
        }
      }
    });
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
