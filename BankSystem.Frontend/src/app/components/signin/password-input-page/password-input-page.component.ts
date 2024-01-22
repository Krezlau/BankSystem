import { Component } from '@angular/core';
import {PasswordInputFormComponent} from "../password-input-form/password-input-form.component";

@Component({
  selector: 'app-password-input-page',
  standalone: true,
  imports: [
    PasswordInputFormComponent
  ],
  templateUrl: './password-input-page.component.html'
})
export class PasswordInputPageComponent {

}
