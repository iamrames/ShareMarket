import { DataResult } from './../models/data-result';
import { environment } from './../../environments/environment';
import { Company } from './../models/company';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ResultTypeOption } from '../enums/result-type-option.enum';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  api: string = environment.api;
  constructor(private http: HttpClient) { }

  getAll(): Observable<Company[]> {
    return this.http.get<DataResult<Company[]>>(`${this.api}/Company`)
    .pipe(
      map(response => response.data)
    );
  }

}
