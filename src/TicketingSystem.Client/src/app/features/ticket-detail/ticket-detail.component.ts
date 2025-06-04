import { Component, computed, inject, input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';

import { Ticket, AddReplyRequest } from '../../../types/ticket.types';
import { TicketStateService } from '../../../services/ticket-state.service.ts';
import { TicketContentComponent } from './components/ticket-content.component';
import { RepliesListComponent } from './components/replies-list.component';
import { ReplyFormComponent } from './components/reply-form.component';

@Component({
  selector: 'app-ticket-detail',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatSnackBarModule,
    TicketContentComponent,
    RepliesListComponent,
    ReplyFormComponent,
  ],
  template: `
    <div class="ticket-detail-container">
      @if (ticketState.isSelectedTicketLoading()) {
        <div class="loading-container">
          <mat-spinner diameter="60"></mat-spinner>
          <p>Loading ticket details...</p>
        </div>
      }

      @if (ticketState.isSelectedTicketReloading()) {
        <mat-progress-bar
          mode="indeterminate"
          class="refresh-progress"
        ></mat-progress-bar>
      } @else if (ticketState.isSelectedTicketError()) {
        <mat-card class="error-card">
          <mat-card-content class="error-content">
            <mat-icon class="error-icon">error_outline</mat-icon>
            <h2>Failed to load ticket</h2>
            <p>
              {{
                ticketState.selectedTicketError()?.message ||
                  'An unexpected error occurred'
              }}
            </p>
            <div class="error-actions">
              <button
                mat-raised-button
                color="primary"
                (click)="refreshTicket()"
              >
                <mat-icon>refresh</mat-icon>
                Try Again
              </button>
            </div>
          </mat-card-content>
        </mat-card>
      } @else if (ticketState.selectedTicketHasValue() && ticket()) {
        <div class="ticket-content">
          <app-ticket-content
            [ticket]="selectedTicket()!"
            (resolveTicket)="resolveTicket()"
          />

          <app-replies-list [replies]="ticket()!.replies || []" />

          <app-reply-form
            defaultAgentName="Support Agent"
            [isResolved]="ticket()!.status === 'Resolved'"
            (replySubmit)="submitReply($event)"
          />
        </div>
      } @else {
        <mat-card class="empty-state">
          <mat-card-content class="empty-content">
            <mat-icon class="empty-icon">description</mat-icon>
            <h2>No ticket selected</h2>
            <p>Select a ticket from the list to view its details</p>
          </mat-card-content>
        </mat-card>
      }
    </div>
  `,
  styles: [
    `
      .ticket-detail-container {
        height: 100%;
        display: flex;
        flex-direction: column;
        gap: 20px;
        position: relative;
      }

      .refresh-progress {
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        z-index: 10;
      }

      .loading-container,
      .error-content,
      .empty-content {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        text-align: center;
        gap: 20px;
        padding: 40px 20px;
        color: #666;
      }

      .error-card {
        border: 1px solid #f44336;
        background-color: #ffeaea;
      }

      .error-icon,
      .empty-icon {
        font-size: 3rem;
        width: 3rem;
        height: 3rem;
      }

      .error-icon {
        color: #f44336;
      }

      .empty-icon {
        color: #9e9e9e;
      }

      .error-content h2 {
        margin: 0;
        color: #f44336;
        font-weight: 500;
      }

      .error-content p {
        margin: 0;
        max-width: 400px;
      }

      .error-actions {
        display: flex;
        gap: 12px;
        flex-wrap: wrap;
        justify-content: center;
      }

      .ticket-content {
        display: flex;
        flex-direction: column;
        gap: 20px;
      }
    `,
  ],
})
export class TicketDetailComponent {
  ticket = input<Ticket>();

  protected readonly ticketState = inject(TicketStateService);
  private readonly snackBar = inject(MatSnackBar);

  protected readonly selectedTicket = computed(() =>
    this.ticketState.selectedTicket()
  );

  protected refreshTicket(): void {
    this.ticketState.refreshSelectedTicket();
  }

  protected async resolveTicket(): Promise<void> {
    const currentTicket = this.selectedTicket();
    if (!currentTicket) return;

    try {
      await this.ticketState.resolveTicket(currentTicket.id);
      this.snackBar.open('Ticket marked as resolved', 'Close', {
        duration: 3000,
        panelClass: ['success-snackbar'],
      });
    } catch {
      this.snackBar.open('Failed to resolve ticket', 'Close', {
        duration: 5000,
        panelClass: ['error-snackbar'],
      });
    }
  }

  protected async submitReply(replyData: AddReplyRequest): Promise<void> {
    const currentTicket = this.selectedTicket();
    if (!currentTicket) return;

    try {
      await this.ticketState.addReply(currentTicket.id, replyData);

      this.snackBar.open('Reply sent successfully', 'Close', {
        duration: 3000,
        panelClass: ['success-snackbar'],
      });
    } catch {
      this.snackBar.open('Failed to send reply', 'Close', {
        duration: 5000,
        panelClass: ['error-snackbar'],
      });
    }
  }

  protected getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'resolved':
        return 'resolved';
      case 'in-resolution':
        return 'in-resolution';
      default:
        return 'open';
    }
  }
}
