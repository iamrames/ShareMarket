import { CompanyService } from './../services/company.service';
import { TargetService } from './../services/target.service';
import { MarketDepthComponent } from './market-depth/market-depth.component';
import { TargetListComponent } from './target/target-list/target-list.component';
import { TargetFormComponent } from './target/target-form/target-form.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { RouterModule } from '@angular/router';
import { AuthGuard } from '../guards/auth.guard';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PqgridComponent } from '../pqgrid.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      {
          path: 'dashboard',
          runGuardsAndResolvers: 'always',
          canActivate: [AuthGuard],
          children: [
            { path: '', component: DashboardComponent},
            { path: 'marketdepth', component: MarketDepthComponent },
            { path: 'target',
              children: [
                { path: '', component: TargetListComponent},
                { path: 'target/create', component: TargetFormComponent}
              ]
            },
          ]
      }
    ]),
  ],
  declarations: [
    DashboardComponent,
    TargetFormComponent,
    TargetListComponent,
    MarketDepthComponent,
    PqgridComponent,
  ],
  providers: [
    TargetService,
    CompanyService,
    AuthGuard
  ]
})
export class DashboardModule { }
