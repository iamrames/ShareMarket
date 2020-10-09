import { TargetService } from './../../services/target.service';
import { CompanyService } from './../../services/company.service';
import { Target } from './../../models/target';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ResultTypeOption } from 'src/app/enums/result-type-option.enum';

@Component({
  selector: 'app-target-form',
  templateUrl: './target-form.component.html',
  styleUrls: ['./target-form.component.scss']
})
export class TargetFormComponent implements OnInit {

  companies$;
  target = {} as Target;
  id;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private companyService: CompanyService,
    private targetService: TargetService
  ) {
    this.companies$ = this.companyService.getAll();

    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id) {
      this.targetService.getTarget(this.id)
        .subscribe(x => x.resultType === ResultTypeOption.Success ? this.target = x.data : this.target = {} as Target );
    }
  }
  ngOnInit(): void {

  }

  save(target): void {
    if (this.id) {
      console.log("Update");
       this.targetService.updateTarget(this.id, target);
      } else {
        console.log("Create");
        this.targetService.create(target);
      }
    this.router.navigate(['']);
  }

  delete(): void {
    if (!confirm('Are you sure you want to delete target?')) { return; }
    this.targetService.deleteTarget(this.id);
    this.router.navigate(['']);
  }

}
