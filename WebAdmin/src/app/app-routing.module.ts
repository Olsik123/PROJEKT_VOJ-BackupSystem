import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { HomePageComponent} from "./pages/home-page/home-page.component";
import { ConfigPageComponent} from "./pages/config-page/config-page.component";
import { ClientPageComponent} from "./pages/client-page/client-page.component";
import { ReportPageComponent} from "./pages/report-page/report-page.component";
import { SettingsPageComponent} from "./pages/settings-page/settings-page.component";
import {ConfigEditPageComponent} from "./pages/config-edit-page/config-edit-page.component";
import {ConfigNewFormComponent} from "./components/config-new-form/config-new-form.component";
import {ConfigNewPageComponent} from "./pages/config-new-page/config-new-page.component";
import {VerifyClientPageComponent} from "./pages/verify-client-page/verify-client-page.component";
import {ClientEditFormComponent} from "./components/client-edit-form/client-edit-form.component";
import {ClientEditPageComponent} from "./pages/client-edit-page/client-edit-page.component";


const routes: Routes = [
  { path: 'login', component: LoginPageComponent},
  { path: 'homepage',component: HomePageComponent},
  { path: 'configs',component: ConfigPageComponent},
  { path: 'clients',component: ClientPageComponent},
  { path: 'reports',component: ReportPageComponent},
  { path: 'settings',component: SettingsPageComponent},
  { path: 'configs/:id',component: ConfigEditPageComponent},
  { path: 'clients/:id',component: ClientEditPageComponent},
  { path: 'newconfig',component: ConfigNewPageComponent},
  { path: 'verifyclient',component: VerifyClientPageComponent},
  { path: '' , component: LoginPageComponent},



];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
