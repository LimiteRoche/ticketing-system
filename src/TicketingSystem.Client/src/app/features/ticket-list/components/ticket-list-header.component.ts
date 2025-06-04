import { Component, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-ticket-list-header',
  imports: [MatButtonModule, MatIconModule, MatTooltipModule],
  template: `
    <div class="list-header">
      <span class="list-title">
        {{ count() }}
        {{ filter() === 'all' ? 'tickets' : filter() + ' tickets' }}
      </span>
      <button
        mat-icon-button
        (click)="refresh.emit()"
        matTooltip="Refresh tickets"
        [disabled]="reloading()"
      >
        <mat-icon [class.spinning]="reloading()">refresh</mat-icon>
      </button>
    </div>
  `,
  styles: [
    `
      .list-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 8px 16px;
        background-color: #f5f5f5;
        border-radius: 8px;
      }

      .list-title {
        font-weight: 500;
        color: #333;
      }

      .spinning {
        animation: spin 1s linear infinite;
      }

      @keyframes spin {
        from {
          transform: rotate(0deg);
        }
        to {
          transform: rotate(360deg);
        }
      }
    `,
  ],
})
export class TicketListHeaderComponent {
  count = input(0);
  filter = input('all');
  reloading = input(false);
  refresh = output<void>();
}
