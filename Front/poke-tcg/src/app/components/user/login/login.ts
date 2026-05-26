import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { userService } from '../../../services/userService';
import { UserCreate } from '../../../models/user/userCreate';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {

  loginForm: FormGroup;
  
   constructor(
    private fb: FormBuilder,
    private userService: userService
  ) {
     this.loginForm = this.fb.group({
     username: ['', Validators.required],
     email: ['', [Validators.required, Validators.email]],
     password: ['', Validators.required]
  });
  }

  onSubmit(): void {

    if (this.loginForm.invalid)
      return;

    const user : UserCreate  = {
      username: this.loginForm.value.username!,
      email: this.loginForm.value.email!,
      passwordHash: this.loginForm.value.password!
    };

    this.userService.createUser(user)
      .subscribe({
        next: (response) => {
          console.log('Utilisateur créé', response);
        },
        error: (err) => {
          console.error(err);
        }
      });
  }
}
