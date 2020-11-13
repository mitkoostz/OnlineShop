import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { AuthGuard } from './core/guards/auth.guard';
import { CantactUsComponent } from './home/cantact-us/cantact-us.component';

const routes: Routes = [

  {
    path: '', loadChildren: () => import('./home/home.module').then(mod => mod.HomeModule),
    data: { breadcrumb: {skip: true} }
  },
  { path: 'Contact', component: CantactUsComponent, data: {  } },
  { path: 'test-error', component: TestErrorComponent, data: { breadcrumb: 'Test Errors' } },
  { path: 'server-error', component: ServerErrorComponent, data: { breadcrumb: 'ServerError' } },
  { path: 'not-found', component: NotFoundComponent, data: { breadcrumb: 'Not Found' } },

  {
    path: 'shop', loadChildren: () => import('./shop/shop.module').then(mod => mod.ShopModule),
    data: { breadcrumb: 'Shop' }
  },
  {
    path: 'basket',
    loadChildren: () => import('./basket/basket.module').then(mod => mod.BasketModule),
    data: { breadcrumb: 'Basket' }
  },
  {
    path: 'checkout',
    canActivate: [AuthGuard],
    loadChildren: () => import('./checkout/checkout.module').then(mod => mod.CheckoutModule),
    data: { breadcrumb: 'Checkout' }
  },
  {
    path: 'account', loadChildren: () => import('./account/account.module').then(mod => mod.AccountModule),
    data: { breadcrumb: {skip: true} }
  },
  {
    path: 'order',
    canActivate: [AuthGuard],
    loadChildren: () => import('./user-orders/user-orders.module').then(mod => mod.UserOrdersModule),
    data: { breadcrumb: 'Orders' }
  },
  {
    path: 'admin',
    canActivate: [AuthGuard],
    loadChildren: () => import('./admin/admin-panel.module').then(mod => mod.AdminPanelModule),
    data: { breadcrumb: 'Admin' , role:"Admin" }
  },
  {
    path: 'myaccount',
    canActivate: [AuthGuard],
    loadChildren: () => import('./account-overview/account-overview.module').then(mod => mod.AccountOverviewModule),
    data: { breadcrumb: '' }
  },
  { path: '**', redirectTo: 'not-found', pathMatch: 'full' }

   ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
