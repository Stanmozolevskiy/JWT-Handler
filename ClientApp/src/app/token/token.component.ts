import { Component, OnInit } from '@angular/core';
import { Token} from '../models/token';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DataService } from '../data-service.service';

@Component({
  selector: 'app-token',
  templateUrl: './token.component.html',
  styleUrls: ['./token.component.css']
})
export class TokenComponent implements OnInit {
  errorMessage!: string;
  tokenForm!: FormGroup;
  decodedTokenForm!: FormGroup;
  token!: Token;
  decodedToken!: Token;
  decodedIntent!: string;

  constructor(private dataService: DataService, private fb: FormBuilder) { }

  
  ngOnInit(): void {
    console.log("Loaded");
    this.tokenForm = this.fb.group({
        Intent: [this.intent],
        Role: ['', [Validators.required]],
        UserId: ['', [Validators.required]],
        Email: ['', [Validators.email]],
        FirstName: [''],
        LastName: [''],
        Lifetime: ['']
    });

    this.decodedTokenForm = this.fb.group(({
        Token: []
    }));
  }

   public intent: Array<{ text: string; value: number }> =[
            {text: "Utilities", value: 1},
            {text: "Login", value: 2},
            {text: "ReportingServices", value: 3},
            {text: "AlertsAndNotifications", value: 4}
            ];

  public onGenerateLoginToken() {
    this.tokenForm.value.Intent = this.tokenForm.value.Intent.value;
      this.dataService.getToken({ ...this.token, ...this.tokenForm.value }).subscribe(res => {
      this.token = res;
    });
}  
  public onDecodeLoginToken() {
    this.dataService.getDecodedToken(this.decodedTokenForm.value.Token).subscribe(res => {
      this.decodedToken = res;
      this.decodedIntent = this.intent.filter(x => x.value === res.Intent)[0].text;
    });
}

}
