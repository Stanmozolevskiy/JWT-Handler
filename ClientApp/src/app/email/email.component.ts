import { HttpEvent, HttpHandler, HttpInterceptor, HttpProgressEvent, HttpRequest,HttpEventType, HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { concat, Observable, of } from 'rxjs';
import { DataService } from '../data-service.service';

@Component({
  selector: 'app-email',
  templateUrl: './email.component.html',
  styleUrls: ['./email.component.css']
})

export class EmailComponent implements OnInit, HttpInterceptor  {
  intercept(req: HttpRequest<any>,next: HttpHandler): Observable<HttpEvent<any>> 
  {
    if (req.url === "email/saveAttachment") {
      const events: Observable<HttpEvent<any>>[] = [0, 30, 60, 100].map((x) =>
        of(<HttpProgressEvent>
          {
          type: HttpEventType.UploadProgress,
          loaded: x,
          total: 100,
          }));

      const success:any = of(new HttpResponse({ status: 200 }));
       events.push(success);
      
       return concat(...events);

    }
    if (req.url === 'email/rempveAttachment') {
      return of(new HttpResponse({ status: 200 }));
    }
    return next.handle(req);
  };
  
  emailSent!: boolean;
  emailForm!: FormGroup;
  uploadSaveUrl = "email/saveAttachment"; 
  uploadRemoveUrl = "email/rempveAttachment"; 

  constructor(private dataService: DataService, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.emailForm = this.fb.group({
      To: ["", [Validators.required, Validators.email]],
      Cc: ["", [ Validators.email]],
      Subject: [''],
      Message: ['',[Validators.required]]
  });
  }
  onSubmit(){
    this.dataService.sendEmail(this.emailForm.value).subscribe();
    this.emailForm.reset();
    this.emailForm.disable();
    this.emailSent = true;
  }

}


