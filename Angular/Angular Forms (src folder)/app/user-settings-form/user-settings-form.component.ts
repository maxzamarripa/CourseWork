import { Component, OnInit } from '@angular/core';
import { UserSettings } from '../data/user-settings';
import { DataService } from '../data/data.service';
import { NgForm, NgModel } from '@angular/forms';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-user-settings-form',
  templateUrl: './user-settings-form.component.html',
  styleUrls: ['./user-settings-form.component.css']
})
export class UserSettingsFormComponent implements OnInit {
  
  originalUserSettings: UserSettings = {
    name: null,
    emailOffers: false,
    interfaceStyle: null,
    subscriptionType: null,
    notes: null
  };

  singleModel = "On";
  startDate: Date;
  startTime: Date;
  userRating = 0;
  maxRating = 10;
  userSettings : UserSettings = {...this.originalUserSettings};
  postError = false;
  postErrorMessage = '';
  subscriptionTypes: Observable<string[]>;

  constructor(private dataService: DataService) { }
 
  ngOnInit(): void {
    this.subscriptionTypes = this.dataService.getSubscriptionTypes();
    this.startDate = new Date();
    this.startTime = new Date(); 
  }

  onHttpError(errorResponse: any){
    console.log('error: ', errorResponse);
    this.postError = true;
    this.postErrorMessage = errorResponse.error.errorMessage;
  }

  onSubmit(form: NgForm){
    console.log('in onSubmit: ', form.valid);
    if(form.valid){
      this.dataService.postUserSettingsForm(this.userSettings)
      .subscribe(
        result => console.log('sucess: ', result),
        error => this.onHttpError(error)
      );
    }
    else{
      this.postError = true;
      this.postErrorMessage = 'Please fix all form errors';
    }
  }

  onBlur(field : NgModel){
    console.log('in onBlur: ', field.valid);
  }
}
