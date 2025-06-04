import { Component, output } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-ticket-idle',
  standalone: true,
  imports: [MatCardModule, MatIconModule, MatButtonModule],
  template: `
    <mat-card class="idle">
      <mat-card-content class="content">
        <mat-icon class="icon">hourglass_empty</mat-icon>
        <h3>Ready to load tickets</h3>
        <button mat-raised-button color="primary" (click)="loadTickets.emit()">
          <mat-icon>download</mat-icon>
          Load Tickets
        </button>
      </mat-card-content>
    </mat-card>
  `,
  styles: [
    `
      .idle {
        margin: 24px auto;
        max-width: 500px;
        text-align: center;
      }
      .content {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 12px;
      }
      .icon {
        font-size: 48px;
        color: #1976d2;
      }
    `,
  ],
})
export class TicketIdleComponent {
  loadTickets = output<void>();
}
