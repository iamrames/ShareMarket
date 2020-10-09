import { TargetListComponent } from './target/target-list/target-list.component';
import { TargetFormComponent } from './Target/target-form/target-form.component';
import { CompanyService } from './services/company.service';
import { TargetService } from './services/target.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { MarketDepthComponent } from './market-depth/market-depth.component';

import { PqgridComponent } from './pqgrid.component';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [
    AppComponent,
    PqgridComponent,
    MarketDepthComponent,
    TargetFormComponent,
    TargetListComponent
   ],
  imports: [
    CommonModule,
    FormsModule,
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: '', component: MarketDepthComponent },
      { path: 'target', component: TargetListComponent },
      { path: 'target/create', component: TargetFormComponent}
    ]),
  ],
  exports: [
    PqgridComponent,
    FormsModule
  ],
  providers: [
    TargetService,
    CompanyService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
