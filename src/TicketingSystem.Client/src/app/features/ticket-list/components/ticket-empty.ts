import { Component, input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-ticket-empty',
  standalone: true,
  imports: [MatCardModule],
  template: `
    <mat-card class="empty-state">
      <mat-card-content>
        <h3>No {{ filter() === 'all' ? '' : filter() }} tickets</h3>
        <p>
          {{
            filter() === 'all'
              ? 'Create your first ticket to get started'
              : 'No ' + filter() + ' tickets found'
          }}
        </p>
      </mat-card-content>
    </mat-card>
  `,
  styles: [
    `
      .empty-state {
        text-align: center;
        padding: 32px;
      }

      h3 {
        margin: 8px 0 4px;
        font-size: 18px;
        font-weight: 600;
      }

      p {
        font-size: 14px;
        color: #666;
      }
    `,
  ],
})
export class TicketEmptyComponent {
  filter = input<'Open' | 'InResolution' | 'all'>('all');
}
