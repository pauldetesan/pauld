import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { RequestService } from '../request-service.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  submitted: boolean;
  hasError = false;
  isSuccess = false;
  errMessage = '';
  successMessage = '';

  constructor(
    private formBuilder: FormBuilder,
    private requestService: RequestService
  ) {
    this.loginForm = this.formBuilder.group({
      email: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });
  }

  ngOnInit() {
  }



  tryLogin() {
    debugger;
    this.submitted = true;

    if (this.loginForm.invalid) {
      return;
    }

    this.requestService.tryLogin(this.loginForm.controls.email.value, this.loginForm.controls.password.value).then(response => {
      if (response.error) {
        this.hasError = true;
        this.errMessage = response.error.error_description;
        return;
      }

      this.isSuccess = true;
      this.successMessage = response;

    });
  }

  popModal() {

  }

  signUp()
  {
    
  }

}
