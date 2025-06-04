import { Component, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatBadgeModule } from '@angular/material/badge';

@Component({
  selector: 'app-ticket-filters',
  standalone: true,
  imports: [MatButtonModule, MatBadgeModule],
  template: `
    <div class="filter-buttons">
      <button
        mat-button
        [class.active]="selectedFilter() === 'all'"
        (click)="selectFilter('all')"
      >
        All Tickets
      </button>

      <button
        mat-button
        [class.active]="selectedFilter() === 'Open'"
        (click)="selectFilter('Open')"
        [matBadge]="stats().open"
        [matBadgeHidden]="stats().open === 0"
        matBadgeColor="warn"
        matBadgeSize="small"
      >
        Open
      </button>

      <button
        mat-button
        [class.active]="selectedFilter() === 'InResolution'"
        (click)="selectFilter('InResolution')"
        [matBadge]="stats().inResolution"
        [matBadgeHidden]="stats().inResolution === 0"
        matBadgeColor="primary"
        matBadgeSize="small"
      >
        In Resolution
      </button>
    </div>
  `,
  styles: [
    `
      .filter-buttons {
        display: flex;
        gap: 8px;
        margin-bottom: 16px;
        flex-wrap: wrap;
      }
      .filter-buttons button.active {
        background-color: #e0e0e0;
        font-weight: bold;
      }
    `,
  ],
})
export class TicketFiltersComponent {
  stats = input<{ open: number; inResolution: number }>({
    open: 0,
    inResolution: 0,
  });
  selectedFilter = input<'all' | 'Open' | 'InResolution'>('all');
  filterChange = output<'Open' | 'InResolution' | 'all'>();

  selectFilter(filter: 'Open' | 'InResolution' | 'all') {
    this.filterChange.emit(filter);
  }
}
