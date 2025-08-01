import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import {APP_BASE_HREF} from '@angular/common';

import { AppComponent } from './app.component';
import { NotfoundComponent } from './notfound/notfound.component';

import { WelcomeComponent } from './welcome/welcome.component';

import { ConfigModule } from './modules/config/config.module';
import { PagesModule } from './modules/pages/pages.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DbModule } from './modules/db/db.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { environment } from '../environments/environment';
import { ContentModule } from './modules/content/content.module';


@NgModule({
  declarations: [
    AppComponent,
    NotfoundComponent,
    WelcomeComponent,
  ],
  imports: [
    BrowserModule,
    SharedModule,
    RouterModule.forRoot([
      { path: 'welcome', component: WelcomeComponent },
      { path: '', redirectTo: 'welcome', pathMatch: 'full' },
      { path: 'notfound', component: NotfoundComponent },
      { path: '**', component: NotfoundComponent },
    ]),
    ConfigModule,
    PagesModule,
    ContentModule,
    DbModule,
    NgbModule
  ],
  providers: [{provide: APP_BASE_HREF, useValue: environment.baseweburl}],
  bootstrap: [AppComponent]
})
export class AppModule { }
