import { Routes } from '@angular/router';
import { LoginComponent } from './components/user/login/login';
import { Register } from './components/user/register/register';
import { ListSets } from './components/set/list-sets/list-sets';
import { ListCardsOfSet } from './components/set/list-cards-of-set/list-cards-of-set';
import { DetailCard } from './components/card/detail-card/detail-card';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: Register },
    { path : 'sets', component : ListSets},
    { path: 'set/:id', component: ListCardsOfSet},
    { path: 'card/:id', component: DetailCard}
];
