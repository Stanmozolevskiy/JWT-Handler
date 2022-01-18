import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Token } from './models/token';

import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService{

  constructor(private http: HttpClient) { }


  getToken(token: Token): Observable<Token> {
    return this.http.post<Token>(`token/generate`, token, { headers: this.headers });
  }

  getDecodedToken(token: string): Observable<Token> {
    return this.http.post<Token>(`token/decode`, { token: token }, { headers: this.headers });
  }

  private headers: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

}
