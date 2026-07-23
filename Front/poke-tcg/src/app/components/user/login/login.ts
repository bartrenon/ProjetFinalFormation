import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserLogin } from '../../../models/user/userLogin';
import { UserService } from '../../../services/user/user-service';
import { UserJwt } from '../../../models/user/user-jwt';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid)
      return;

    const credentials: UserLogin = {
      email: this.loginForm.value.email!,
      password: this.loginForm.value.password!
    };

    this.userService.login(credentials)
      .subscribe({
        next: (response: UserJwt) => {
          this.router.navigate(['/sets']);
        },
        error: (err) => {
          console.error(err);
        }
      });
  }
}
