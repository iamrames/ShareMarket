import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpResponseBase } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import pq from 'pqgrid';
import { interval, Subscription } from 'rxjs';
import { environment } from '../../../environments/environment';
import { DatePipe, formatDate } from '@angular/common';

interface LiveTradingData {
  symbol: string;
  ltp: number;
  ltv: number;
  changePercentage: number;
  high: number;
  low: number;
  open: number;
  qty: number;
  pq_rowstyle: any;
}

@Component({
  selector: 'app-market-depth',
  templateUrl: './market-depth.component.html',
  styleUrls: ['./market-depth.component.scss'],
  providers: [DatePipe]
})

export class MarketDepthComponent implements OnInit {
  myDate;
  liveTradingData: LiveTradingData[] = [];
  name = 'Angular';
  mySubscription: Subscription;
  api: string = environment.api;
  colors: any = {
    yellow : { background: 'yellow' },
    aqua : {background: 'aqua' },
    light_green : { background: 'lightgreen' },
    voilet : { background: 'violet' },
    orange : { background: 'orange' },
    red: { background: '#d2322d' },
    green: { background: '#47a447'}
  };

  columns1 = [
    { title: 'Symbol', width: 100, dataType: 'integer', dataIndx: 'symbol', editable: false },
    { title: 'LTP', width: 150, dataType: 'float', dataIndx: 'ltp', format: '#.00', editable: false },
    { title: 'LTV', width: 150, dataType: 'float', dataIndx: 'ltv', format: '#.00', editable: false },
    { title: '% Change', width: 150, dataType: 'float', dataIndx: 'changePercentage', format: '#.00', editable: false },
    { title: 'High', width: 150, dataType: 'float', dataIndx: 'high', format: '#.00', editable: false },
    { title: 'Low', width: 150, dataType: 'float', dataIndx: 'low', format: '#.00', editable: false },
    { title: 'Open', width: 150, dataType: 'float', dataIndx: 'open', format: '#.00', editable: false },
    { title: 'Qty', width: 150, dataType: 'float', dataIndx: 'qty', format: '#.00', editable: false },
  ];
  constructor(private http: HttpClient, private route: ActivatedRoute, private datePipe: DatePipe) {
    this.myDate = formatDate(new Date(), 'yyyy/MM/dd', 'en');
  }

  options: pq.gridT.options = {
    showTop: false,
    showHeader: true,
    showBottom: true,
    reactive: true,
    locale: 'en',
    height: 'flex',
    width: 'flex',
    numberCell: {
      show: true
    },
    pageModel: {
      type: 'local',
      rPP: 100,
      rPPOptions: [100, 200, 500],
      layout: ['strDisplay', '|', 'prev', 'next'],
    },
    colModel: this.columns1,
    animModel: {
      on: true
    },
    dataModel: {
      data: this.liveTradingData,
    },
    bootstrap: { on : true }
  };

  ngOnInit(): void {
    this.mySubscription = interval(600000).subscribe(x => {
      this.getLiveTradingData();
    });

    this.route.data.subscribe(res => this.getLiveTradingData());
  }

  getLiveTradingData(): void {
    this.http.get<LiveTradingData[]>(this.api + 'MeroLaganiScraper')
    .subscribe(response => {
      response.map(x => x.changePercentage < 0 ? x.pq_rowstyle = this.colors.red : x.pq_rowstyle = this.colors.green);
      this.liveTradingData = response;
      this.options.dataModel.data = this.liveTradingData;
    }, error => {
      console.log(error);
    });
  }

  ngOnDestroy() {
    this.mySubscription.unsubscribe();
  }

}
