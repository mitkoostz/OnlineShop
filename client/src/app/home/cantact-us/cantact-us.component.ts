import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';
import { IContactUsMessage } from 'src/app/shared/models/contactusMessage';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-cantact-us',
  templateUrl: './cantact-us.component.html',
  styleUrls: ['./cantact-us.component.scss'],
})
export class CantactUsComponent implements OnInit {
  baseUrl = environment.apiUrl;
  contactUsMessage: IContactUsMessage[] = [];
  messageForm: FormGroup;
  messageLength: number = 0;
  formSubmited: boolean = false;

  constructor(private http: HttpClient, private router: Router, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.createMessageForm();
  }

  createMessageForm() {
    this.messageForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
      email: new FormControl('', [
        Validators.required,
        Validators.pattern(
          "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*"
        ),
      ]),
      phoneNumber: new FormControl('', [
        Validators.required,
        Validators.pattern(
          '^\\s*(?:\\+?(\\d{1,3}))?[-. (]*(\\d{3})[-. )]*(\\d{3})[-. ]*(\\d{4})(?: *x(\\d+))?\\s*$'
        ),
      ]),
      messageText: new FormControl('', [
        Validators.required,
        Validators.minLength(30),
        Validators.maxLength(600),
      ]),
    });
  }

  onSubmit()
  {
    if(this.messageForm.invalid)
    {
      this.messageForm.markAllAsTouched();
      return;
    }
       this.sentMessage();
  }

  sentMessage() {
      this.contactUsMessage = {...this.contactUsMessage,...this.messageForm.value};
    return this.http.post<IContactUsMessage>(this.baseUrl + "contactus/contactusmessage" , this.contactUsMessage).subscribe((response) =>
   {
     this.formSubmited = true;
   }, error =>
   {
     error.errors.forEach(er => {
      this.toastr.error(er);
     });
   });

  }

  CalculateMessageCharacters(event) {
    this.messageLength = event;
  }
}
