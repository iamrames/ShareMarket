import { Target } from './../models/target';
import { DataResult } from './../models/data-result';
import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TargetService {

  baseUrl: string = environment.api;
  constructor(private http: HttpClient) { }

  create(target): Observable<DataResult<string>> {
    return this.http.post<DataResult<string>>(this.baseUrl + '/targetsetup/', target);
  }

  getAll(): Observable<DataResult<Target[]>> {
    return this.http.get<DataResult<Target[]>>(this.baseUrl + '/targetsetup/');
  }

  getTarget(id): Observable<DataResult<Target>> {
    return this.http.get<DataResult<Target>>(this.baseUrl + '/targetsetup/' + id);
  }

  updateTarget(id, target): Observable<DataResult<string>> {
    return this.http.put<DataResult<string>>(this.baseUrl + '/targetsetup/' + id, target);
  }

  deleteTarget(id): Observable<DataResult<string>> {
    return this.http.delete<DataResult<string>>(this.baseUrl + '/targetsetup/' + id);
  }

}
