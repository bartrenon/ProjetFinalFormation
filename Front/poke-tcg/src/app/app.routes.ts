import { Routes } from '@angular/router';
import { LoginComponent } from './components/user/login/login';
import { Register } from './components/user/register/register';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: Register }
];
