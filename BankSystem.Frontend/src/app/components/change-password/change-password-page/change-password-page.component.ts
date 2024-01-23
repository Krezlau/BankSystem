import { Component } from '@angular/core';
import {ChangePasswordFormComponent} from "../change-password-form/change-password-form.component";

@Component({
  selector: 'app-change-password-page',
  standalone: true,
  imports: [
    ChangePasswordFormComponent
  ],
  templateUrl: './change-password-page.component.html'
})
export class ChangePasswordPageComponent {

}
