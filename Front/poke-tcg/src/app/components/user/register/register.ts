import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { UserCreate } from '../../../models/user/userCreate';
import { UserService } from '../../../services/userService';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserService
  ) {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.registerForm.invalid)
      return;

    const user: UserCreate = {
      username: this.registerForm.value.username!,
      email: this.registerForm.value.email!,
      password: this.registerForm.value.password!
    };

    // Inscription : le hash du mot de passe doit etre fait cote backend.
    this.userService.createUser(user)
      .subscribe({
        next: (response) => {
          console.log('Utilisateur cree', response);
        },
        error: (err) => {
          console.error(err);
        }
      });
  }
}
