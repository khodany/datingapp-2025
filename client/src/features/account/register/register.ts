import { Component, Inject, inject, input, OnInit, output, signal } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { RegisterCreds } from '../../../types/user';
import { AccountService } from '../../../core/services/account-service';
import { JsonPipe } from '@angular/common';
import { TextInput } from "../../../shared/text-input/text-input";
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, TextInput],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {


  private accountService = inject(AccountService);
  private router = inject(Router);
  private fb = inject(FormBuilder);
  cancelRegister = output<boolean>(); // sendng from child to parent
  protected creds = {} as RegisterCreds;
  protected CredentialForm: FormGroup;
  protected ProfileForm: FormGroup;
  protected currentStep = signal(1);
  protected validationError = signal<string[]>([]);



  constructor() {

    this.CredentialForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      displayName: ['', Validators.required],
      password: ['', [Validators.required,
      Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]

    });
    this.CredentialForm.controls['password'].valueChanges.subscribe(() => {
      this.CredentialForm.controls['confirmPassword'].updateValueAndValidity();
    });

    this.ProfileForm = this.fb.group({
      gender: ['male', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required]
    });

  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const parent = control.parent;
      if (!parent) return null;

      const matchValue = parent.get(matchTo)?.value;
      return control.value === matchValue ? null : { passwordMismatch: true }
    }
  }

  nextStep() {
    if (this.CredentialForm.valid) {
      this.currentStep.update(prevStep => prevStep + 1)
    }
  }

  prevStep() {
    this.currentStep.update(prevStep => prevStep - 1);
  }

  getMaxDate() {
    const today = new Date();
    today.setFullYear(today.getFullYear() - 18);

    return today.toISOString().split('T')[0];
  }


  register() {

    if (this.ProfileForm.valid && this.CredentialForm.valid) {
      const formData = { ...this.CredentialForm.value, ...this.ProfileForm.value };

      this.accountService.register(formData).subscribe({
        next: () => {
         this.router.navigateByUrl('/members');

        },
        error: error => {
          console.log(error);
          this.validationError.set(error);
        }
      })
    }


    // console.log(this.creds);
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
