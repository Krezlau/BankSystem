import { Routes } from '@angular/router';
import {HomePageComponent} from "./components/home/home-page/home-page.component";
import {SignInPageComponent} from "./components/signin/sign-in-page/sign-in-page.component";
import {SignUpPageComponent} from "./components/signup/sign-up-page/sign-up-page.component";
import {DashBoardPageComponent} from "./components/dashboard/dash-board-page/dash-board-page.component";
import {NotFoundPageComponent} from "./components/not-found-page/not-found-page.component";
import {PasswordInputPageComponent} from "./components/signin/password-input-page/password-input-page.component";
import {
  ChangePasswordPageComponent
} from "./components/change-password/change-password-page/change-password-page.component";

export const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'sign-in', component: SignInPageComponent },
  { path: 'sign-in/:key', component: PasswordInputPageComponent },
  { path: 'change-password/:key', component: ChangePasswordPageComponent },
  { path: 'sign-up', component: SignUpPageComponent },
  { path: 'dashboard', component: DashBoardPageComponent },
  { path: '**', component: NotFoundPageComponent }
];
