import { Component, input } from '@angular/core';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-ticket-loading',
  imports: [MatProgressBarModule, MatProgressSpinnerModule],
  template: `
    <div class="loading-container">
      @if (reloading()) {
        <mat-progress-bar mode="indeterminate" class="refresh-progress">
        </mat-progress-bar>
      } @else {
        <div class="loading-content">
          <mat-spinner diameter="40"></mat-spinner>
          <p>Loading tickets...</p>
        </div>
      }
    </div>
  `,
  styles: [
    `
      .loading-container {
        display: flex;
        flex-direction: column;
        align-items: center;
        padding: 24px;
        text-align: center;
      }

      .refresh-progress {
        width: 100%;
        margin-bottom: 16px;
      }

      .loading-content {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 12px;
      }

      p {
        font-size: 14px;
        color: #555;
      }
    `,
  ],
})
export class TicketLoadingComponent {
  reloading = input(false);
}
