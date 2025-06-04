import { Component, input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-ticket-stats',
  imports: [MatCardModule, MatDividerModule],
  template: `
    <mat-card class="stats-card">
      <mat-card-content class="stats-content">
        <div class="stat-item">
          <span class="stat-number">{{ stats().total }}</span>
          <span class="stat-label">Total</span>
        </div>
        <mat-divider [vertical]="true"></mat-divider>
        <div class="stat-item">
          <span class="stat-number unresolved">{{ stats().open }}</span>
          <span class="stat-label">Open</span>
        </div>
        <mat-divider [vertical]="true"></mat-divider>
        <div class="stat-item">
          <span class="stat-number resolved">{{ stats().inResolution }}</span>
          <span class="stat-label">In Resolution</span>
        </div>
      </mat-card-content>
    </mat-card>
  `,
  styles: [
    `
      .stats-card {
        margin-bottom: 12px;
      }

      .stats-content {
        display: flex;
        align-items: center;
        justify-content: space-around;
        gap: 16px;
      }

      .stat-item {
        display: flex;
        flex-direction: column;
        align-items: center;
      }

      .stat-number {
        font-size: 20px;
        font-weight: bold;
        color: #333;
      }

      .stat-number.unresolved {
        color: #d9534f;
      }

      .stat-number.resolved {
        color: #5cb85c;
      }

      .stat-label {
        font-size: 12px;
        color: #666;
      }

      mat-divider[vertical] {
        height: 32px;
      }
    `,
  ],
})
export class TicketStatsComponent {
  stats = input({ total: 0, open: 0, inResolution: 0 });
}
