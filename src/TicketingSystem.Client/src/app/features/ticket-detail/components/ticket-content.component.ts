import { Component, input, output } from '@angular/core';
import { DatePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { Ticket } from '../../../../types/ticket.types';
import { AvatarComponent } from '../../../shared/avatar.component';

@Component({
  selector: 'app-ticket-content',
  standalone: true,
  imports: [
    DatePipe,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    AvatarComponent,
  ],
  template: `
    <div class="wrapper">
      <mat-card>
        <mat-card-header>
          <div class="header">
            <app-avatar [avatarUrl]="ticket().avatarUrl" />
            <div class="details">
              <h2 class="subject">{{ ticket().subject }}</h2>
              <div class="meta">
                <span>#{{ ticket().id }}</span>
                <span class="sep">•</span>
                <span>{{ ticket().username }}</span>
                <span class="sep">•</span>
                <span>Customer</span>
                <span class="sep">•</span>
                <span>{{ ticket().createdAt | date: 'medium' }}</span>
                <mat-chip class="chip">{{ ticket().status }}</mat-chip>
              </div>
            </div>
            @if (ticket().status !== 'Resolved') {
              <button mat-raised-button color="accent" (click)="resolve()">
                <mat-icon>check_circle</mat-icon>
                Mark as resolved
              </button>
            }
          </div>
        </mat-card-header>
        <mat-card-content>
          <p class="description">{{ ticket().description }}</p>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [
    `
      .wrapper {
        padding: 20px;
        max-width: 960px;
        margin: auto;
      }

      .header {
        display: flex;
        flex-wrap: wrap;
        gap: 16px;
      }

      .details {
        flex: 1;
        min-width: 200px;
      }

      .subject {
        margin: 0;
        font-size: 1.5rem;
        font-weight: 500;
        color: #333;
      }

      .meta {
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
        font-size: 0.85rem;
        color: #666;
      }

      .chip {
        background-color: #4caf50;
        color: white;
      }

      .sep {
        color: #bbb;
      }

      .description {
        margin-top: 16px;
        line-height: 1.6;
        font-size: 1rem;
        white-space: pre-wrap;
      }

      @media (max-width: 768px) {
        .header {
          flex-direction: column;
        }
      }
    `,
  ],
})
export class TicketContentComponent {
  ticket = input.required<Ticket>();
  resolveTicket = output<string>();

  resolve() {
    this.resolveTicket.emit(this.ticket().id);
  }
}
