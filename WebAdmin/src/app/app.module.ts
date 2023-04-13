import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { ConfigPageComponent } from './pages/config-page/config-page.component';
import { ClientPageComponent } from './pages/client-page/client-page.component';
import { ReportPageComponent } from './pages/report-page/report-page.component';
import { SettingsPageComponent } from './pages/settings-page/settings-page.component';
import { MatGridListModule} from '@angular/material/grid-list';
import { MatIconModule} from "@angular/material/icon";
import { CommonModule } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatSliderModule } from '@angular/material/slider';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { FormsModule } from '@angular/forms';
import {MatNativeDateModule} from "@angular/material/core";
import {JwtModule} from "@auth0/angular-jwt";
import { MomentDateModule } from '@angular/material-moment-adapter';
import { MAT_DATE_FORMATS } from '@angular/material/core';
import { MY_DATE_FORMATS} from "./modules/my_date_format";
// Material Navigation
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
// Material Layout
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatListModule } from '@angular/material/list';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTreeModule } from '@angular/material/tree';
// Material Buttons & Indicators
import { MatButtonModule } from '@angular/material/button';
import { MatBadgeModule } from '@angular/material/badge';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatRippleModule } from '@angular/material/core';
// Material Popups & Modals
import { MatBottomSheetModule } from '@angular/material/bottom-sheet';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
// Material Data tables
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { ConfigEditPageComponent } from './pages/config-edit-page/config-edit-page.component';
import { ConfigTableComponent } from './components/config-table/config-table.component';
import { ReportTableComponent} from "./components/report-table/report-table.component";
import { ClientTableComponent } from './components/client-table/client-table.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ConfigEditFormComponent } from './components/config-edit-form/config-edit-form.component';
import {ConfigNewFormComponent} from "./components/config-new-form/config-new-form.component";
import {ConfigNewPageComponent } from './pages/config-new-page/config-new-page.component';
import {HttpClientModule} from "@angular/common/http";
import { SettingsFormComponent } from './components/settings-form/settings-form.component';
import {DatePipe} from "@angular/common";
import { VerifyClientPageComponent } from './pages/verify-client-page/verify-client-page.component';
import { VerifyClientTableComponent } from './components/verify-client-table/verify-client-table.component';
import { ClientEditFormComponent } from './components/client-edit-form/client-edit-form.component';
import { ClientEditPageComponent } from './pages/client-edit-page/client-edit-page.component';
import { DialogComponent } from './components/dialog/dialog.component';
import {IMPORT_PREFIX} from "@angular/compiler-cli/ngcc/src/constants";
import { AdminSettingsComponent } from './components/admin-settings/admin-settings.component';

function tokenGetter() {
  console.log('xxx');
  return sessionStorage.getItem('token');
}
// @ts-ignore
@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    LoginPageComponent,
    HomePageComponent,
    ConfigPageComponent,
    ClientPageComponent,
    ReportPageComponent,
    SettingsPageComponent,
    ConfigEditPageComponent,
    ConfigTableComponent,
    ReportTableComponent,
    ClientTableComponent,
    ConfigEditFormComponent,
    ConfigNewFormComponent,
    ConfigNewPageComponent,
    SettingsFormComponent,
    VerifyClientPageComponent,
    VerifyClientTableComponent,
    ClientEditFormComponent,
    ClientEditPageComponent,
    DialogComponent,
    AdminSettingsComponent
  ],
  imports: [
    ReactiveFormsModule,
    MatNativeDateModule,
    BrowserModule,
    FormsModule,
    HttpClientModule,
    MatButtonToggleModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatGridListModule,
    MatIconModule,
    CommonModule,
    MatAutocompleteModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    MatRadioModule,
    MatSelectModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatMenuModule,
    MatSidenavModule,
    MatToolbarModule,
    MatCardModule,
    MatDividerModule,
    MatExpansionModule,
    MatGridListModule,
    MatListModule,
    MatStepperModule,
    MatTabsModule,
    MatTreeModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatBadgeModule,
    MatChipsModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatRippleModule,
    MatBottomSheetModule,
    MatDialogModule,
    MatSnackBarModule,
    MatTooltipModule,
    MatPaginatorModule,
    MatSortModule,
    MatTableModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter
      },
    }),
  ],

  providers: [DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
