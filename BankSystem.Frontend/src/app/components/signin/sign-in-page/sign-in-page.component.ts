import { Component } from '@angular/core';
import {SignInFormComponent} from "../sign-in-form/sign-in-form.component";

@Component({
  selector: 'app-sign-in-page',
  standalone: true,
  imports: [
    SignInFormComponent
  ],
  templateUrl: './sign-in-page.component.html'
})
export class SignInPageComponent {

}
