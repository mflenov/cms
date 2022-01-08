import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NotfoundComponent } from './notfound/notfound.component';

import { WelcomeComponent } from './welcome/welcome.component';

import { ConfigModule } from './modules/config/config.module';
import { PagesModule } from './modules/pages/pages.module';
import { ContentDefinitionComponent } from './modules/pages/content-definition/content-definition.component';


@NgModule({
  declarations: [
    AppComponent,
    NotfoundComponent,
    WelcomeComponent,
    ContentDefinitionComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot([
      { path: 'welcome', component: WelcomeComponent },
      { path: '', redirectTo: 'welcome', pathMatch: 'full' },
      { path: 'notfound', component: NotfoundComponent },
      { path: '**', component: NotfoundComponent },
    ]),
    ConfigModule,
    PagesModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
