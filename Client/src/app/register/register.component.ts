import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  
  registerForm: FormGroup = new FormGroup({});
  maxDate:Date = new Date();
  validationError: string [] | undefined;

  constructor(private accountService: AccountService,private router: Router, private toastr:ToastrService,
        private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeFrom();
    this.maxDate.setFullYear(this.maxDate.getFullYear() -18);
  }
  

  initializeFrom() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['',
              [
                Validators.required,
                Validators.minLength(4),
                Validators.maxLength(16)
              ]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    });
  }


  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : {notMatching: true}
    }
  }

  register(){
    const dob = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value);
    const values = {...this.registerForm.value, dateOfBirth: dob};

    //console.log(values);
    this.accountService.register(values).subscribe({
      next: () => this.router.navigateByUrl('/members'), //this makes the user goes to members page after login success
      error: error => {
        this.validationError = error
      }     
    })
  }

  //this function is used to convert date that comes from Datepicker into Date only as UTC format 
  private getDateOnly(dob: string | undefined){
    if (!dob) return;
    let theDob = new Date(dob);
    return new Date(theDob.setMinutes(theDob.getMinutes()-theDob.getTimezoneOffset())).toISOString().slice(0,10)
  }

  cancel(){
    console.log('Cancelled...!')
  }
}
