import { Component, input, output } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-ticket-error',
  standalone: true,
  imports: [MatCardModule, MatIconModule, MatButtonModule],
  template: `
    <mat-card class="error-card">
      <mat-card-content class="error-content">
        <mat-icon class="error-icon">error_outline</mat-icon>
        <h3>Failed to load tickets</h3>
        <p>{{ errorMessage() || 'An unexpected error occurred' }}</p>
        <button mat-raised-button color="primary" (click)="retry.emit()">
          <mat-icon>refresh</mat-icon>
          Try Again
        </button>
      </mat-card-content>
    </mat-card>
  `,
  styles: [
    `
      .error-card {
        max-width: 500px;
        margin: 24px auto;
        text-align: center;
      }
      .error-content {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 12px;
      }
      .error-icon {
        font-size: 48px;
        color: #f44336;
      }
    `,
  ],
})
export class TicketErrorComponent {
  errorMessage = input<string | undefined>();
  retry = output();
}
