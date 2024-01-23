import { Component } from '@angular/core';
import {SignInFormComponent} from "../sign-in-form/sign-in-form.component";
import {RouterLink} from "@angular/router";

@Component({
  selector: 'app-sign-in-page',
  standalone: true,
  imports: [
    SignInFormComponent,
    RouterLink
  ],
  templateUrl: './sign-in-page.component.html'
})
export class SignInPageComponent {

}
