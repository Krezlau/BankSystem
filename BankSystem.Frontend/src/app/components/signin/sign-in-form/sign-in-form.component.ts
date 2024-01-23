import {Component, OnDestroy} from '@angular/core';
import {FormControl, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {AuthService} from "../../../services/auth.service";
import {AlertService} from "../../../services/alert.service";
import ApiResponse from "../../../types/ApiResponse";
import LoginCheckResponse from "../../../types/LoginCheckResponse";
import {Router} from "@angular/router";
import {Subscription} from "rxjs";
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-sign-in-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule,
    NgIf
  ],
  templateUrl: './sign-in-form.component.html'
})
export class SignInFormComponent implements OnDestroy {
  emailControl: FormControl = new FormControl<string>('', [Validators.required, Validators.email]);
  sub = new Subscription();
  isLoading = this.authService.isLoading;

  constructor(private authService: AuthService, private alertService: AlertService, private router: Router) {
  }

  onFormSubmit(event: FormDataEvent) {
    if (this.emailControl.invalid || !this.emailControl.value) {
      this.alertService.show('Please enter a valid email address.', 'error');
      return;
    }

    this.authService.loginCheck(this.emailControl.value).subscribe((res: ApiResponse<LoginCheckResponse>) => {
      if (res.success && res.data) this.router.navigate(['/sign-in', res.data.key], {queryParams: {email: this.emailControl.value, mask: res.data.mask}});
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
