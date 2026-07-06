import { Component, OnInit, inject, signal } from '@angular/core';
import { SetService } from '../../../services/set-service';
import { Set } from '../../../models/set/set';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-list-sets',
  imports: [RouterLink],
  templateUrl: './list-sets.html',
  styleUrl: './list-sets.scss',
})
export class ListSets implements OnInit {
  private _setService = inject(SetService);

  sets = signal<Set[]>([]);
  isLoading = signal(false);
  extension = signal('webp');

  ngOnInit(): void {
    this.loadSets();
  }

  loadSets(): void {
    this.isLoading.set(true);

    this._setService.getAllSets().subscribe({
      next: (sets) => {
        this.sets.set(sets);
        this.sets().forEach(set => {
          if (set.symbol) {
          set.symbol = `${set.symbol}.${this.extension()}`;
        }
        if (set.logo) {
          set.logo = `${set.logo}.${this.extension()}`;
        }  
        });
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error(err);
        this.isLoading.set(false);
      }
    });
  }
}
