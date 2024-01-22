import { Component } from '@angular/core';
import {SignUpFormComponent} from "../sign-up-form/sign-up-form.component";

@Component({
  selector: 'app-sign-up-page',
  standalone: true,
  imports: [
    SignUpFormComponent
  ],
  templateUrl: './sign-up-page.component.html'
})
export class SignUpPageComponent {

}
