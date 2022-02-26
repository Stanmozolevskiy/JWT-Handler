import { SMS, TwilioAccount } from './models/sms';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Token } from './models/token';
import { EmailMessage } from './models/email';

import { Observable } from 'rxjs';

@Injectable({providedIn: 'root'})
export class DataService{

  constructor(private http: HttpClient) { }


  getToken(token: Token): Observable<Token> {
    return this.http.post<Token>(`token/generate`, token, { headers: this.headers });
  }

  getDecodedToken(token: string): Observable<Token> {
    return this.http.post<Token>(`token/decode`, { token: token }, { headers: this.headers });
  }

  sendEmail(email: EmailMessage): Observable<EmailMessage>{
    return this.http.post<EmailMessage>('email/send', email, {headers: this.headers});
  }

  sendSms(sms: SMS): Observable<SMS>{
    return this.http.post<SMS>('sms/send', sms, {headers: this.headers});
  }

  getSummary(): Observable<TwilioAccount[]> {
     return this.http.get<TwilioAccount[]>(`sms`, { headers: this.headers });
  }

  private headers: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

}
