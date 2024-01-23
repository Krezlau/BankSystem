import { Component } from '@angular/core';
import {SignUpFormComponent} from "../sign-up-form/sign-up-form.component";
import {RouterLink} from "@angular/router";

@Component({
  selector: 'app-sign-up-page',
  standalone: true,
  imports: [
    SignUpFormComponent,
    RouterLink
  ],
  templateUrl: './sign-up-page.component.html'
})
export class SignUpPageComponent {

}
