import { Routes } from '@angular/router';
import { LoginComponent } from './components/user/login/login';
import { Register } from './components/user/register/register';
import { ListSets } from './components/set/list-sets/list-sets';
import { ListCardsOfSet } from './components/set/list-cards-of-set/list-cards-of-set';
import { DetailCard } from './components/card/detail-card/detail-card';
import { UserGuard } from './services/user/user-guard';
import { ListCards } from './components/card/list-cards/list-cards';
import { Profil } from './components/user/profil/profil';
import { Rules } from './components/page/rules/rules';
import { Anatomy } from './components/page/anatomy/anatomy';
import { CardBuy } from './components/card/card-buy/card-buy';
import { CreateListing } from './components/Listing/create-listing/create-listing';
import { EditListing } from './components/Listing/edit-listing/edit-listing';
import { MyListing } from './components/Listing/my-listing/my-listing';
import { DetailListing } from './components/Listing/detail-listing/detail-listing';
import { ListListing } from './components/Listing/list-listing/list-listing';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: Register },
    { path : 'sets', component : ListSets, canActivate: [UserGuard]},
    { path: 'set/:id', component: ListCardsOfSet, canActivate: [UserGuard] },
    { path: 'card/:id', component: DetailCard, canActivate: [UserGuard] },
    { path: 'cards', component: ListCards, canActivate: [UserGuard] },
    { path: 'profil', component: Profil, canActivate: [UserGuard] },
    { path: 'rules', component: Rules, canActivate: [UserGuard] },
    { path: 'anatomy', component: Anatomy, canActivate: [UserGuard] },
    { path: 'cardBuy', component: CardBuy, canActivate: [UserGuard] },
    { path: 'listings', component: ListListing, canActivate: [UserGuard] },
    { path: 'listings/new', component: CreateListing, canActivate: [UserGuard] },
    { path: 'listings/:id/edit', component: EditListing, canActivate: [UserGuard] },
    { path: 'listings/:id', component: DetailListing, canActivate: [UserGuard] },
    { path: 'my-listings', component: MyListing, canActivate: [UserGuard] },
];
