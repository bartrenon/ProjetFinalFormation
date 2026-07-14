import { AsyncPipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Navbar } from "./components/layouts/navbar/navbar";
import { UserService } from './services/user/user-service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Navbar, AsyncPipe],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('poke-tcg');
  userService = inject(UserService);
}
