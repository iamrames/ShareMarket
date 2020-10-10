import { Component, AfterViewInit, OnChanges, ViewChild, ElementRef, Input, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';

import pq from 'pqgrid';

//import few localization files for this demo.
import 'pqgrid/localize/pq-localize-en.js';
import 'pqgrid/localize/pq-localize-ja.js';
import 'pqgrid/localize/pq-localize-kr.js';
import 'pqgrid/localize/pq-localize-tr.js';


@Component({
  selector: 'app-pqgrid',
  template: '<div #pqgrid ></div>'
})

export class PqgridComponent implements AfterViewInit, OnChanges {
    @ViewChild('pqgrid') div: ElementRef;
    @Input() options: any;
    grid: pq.gridT.instance;

    private createGrid(): void{
        this.grid = pq.grid(this.div.nativeElement, this.options);
    }

    ngOnChanges(obj: SimpleChanges): void{
        if (!obj.options.firstChange ){// grid is destroyed and recreated only when whole options object is changed to new reference.
            this.grid.destroy();
            this.createGrid();
        }
    }
    ngAfterViewInit(): void {
        this.createGrid();
    }
  }
