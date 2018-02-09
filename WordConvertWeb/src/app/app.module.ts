import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { WelcomeComponent } from './home/welcome.component';
import { ConverterComponent } from './converter/converter.component';
import { ProfileOutputComponent } from './profile-output/profile-output.component';
import { WordconverterService } from './wordconverter.service';
import { MessageService } from './message.service';
import { MessagesComponent } from './messages/messages.component';

@NgModule({
  declarations: [
    AppComponent,
    WelcomeComponent,
    ConverterComponent,
    ProfileOutputComponent,
    MessagesComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
        { path: 'converter', component: ConverterComponent },
        { path: 'welcome', component: WelcomeComponent },
        { path: '', redirectTo: 'welcome', pathMatch: 'full'},
        { path: '**', redirectTo: 'welcome', pathMatch: 'full'}
    ])
  ],
  providers: [
    WordconverterService,
    MessageService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
