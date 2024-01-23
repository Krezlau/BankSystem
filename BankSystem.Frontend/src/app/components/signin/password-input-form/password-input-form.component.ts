import {Component, OnDestroy, OnInit} from '@angular/core';
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {AsapAction} from "rxjs/internal/scheduler/AsapAction";
import {ActivatedRoute, Route, Router} from "@angular/router";
import {Subscription} from "rxjs";
import {AlertService} from "../../../services/alert.service";
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {AuthService} from "../../../services/auth.service";

@Component({
  selector: 'app-password-input-form',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    NgClass,
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './password-input-form.component.html'
})
export class PasswordInputFormComponent implements OnInit, OnDestroy {
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

  sub = new Subscription();

  constructor(private route: ActivatedRoute, private alertService: AlertService, private router: Router, private authService: AuthService) {
  }

  onFormSubmit(event: Event) {
    if (this.passwordControlGroup.invalid) {
      this.alertService.show("Please type the characters", "error");
      return;
    }
    let password = "";
    password += this.passwordControlGroup.value.passwordControl1;
    password += this.passwordControlGroup.value.passwordControl2;
    password += this.passwordControlGroup.value.passwordControl3;
    password += this.passwordControlGroup.value.passwordControl4;
    password += this.passwordControlGroup.value.passwordControl5;

    this.authService.sendLoginRequest(this.email, this.key, password).subscribe((res) => {
      if (res.success) {
        this.alertService.show("Successfully logged in", "success");
        //this.router.navigate(['/dashboard']);
      }
    });
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
