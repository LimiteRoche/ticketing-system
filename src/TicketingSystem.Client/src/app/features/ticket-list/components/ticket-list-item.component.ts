import { Component, input, output } from '@angular/core';
import { DatePipe } from '@angular/common';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { AvatarComponent } from '../../../shared/avatar.component';
import { TicketSummary } from '../../../../types/ticket.types';

@Component({
  selector: 'app-ticket-list-item',
  imports: [DatePipe, MatListModule, MatIconModule, AvatarComponent],
  template: `
    <mat-list-item
      class="ticket-item"
      [class.selected]="selected()"
      (click)="onSelect()"
    >
      <div class="ticket-wrapper" matListItemLine>
        <div class="avatar">
          <app-avatar [avatarUrl]="ticket().avatarUrl" />
        </div>

        <div class="ticket-details">
          <div class="subject">{{ ticket().subject }}</div>

          <div class="ticket-meta">
            <div class="meta-item">
              <mat-icon class="meta-icon">person</mat-icon>
              <span>{{ ticket().username }}</span>
            </div>
            <div class="meta-item">
              <mat-icon class="meta-icon">schedule</mat-icon>
              <span>{{ ticket().createdAt | date: 'short' }}</span>
            </div>
          </div>
        </div>
      </div>
    </mat-list-item>
  `,
  styles: [
    `
      .ticket-item {
        cursor: pointer;
        padding: 8px 12px;
        border-bottom: 1px solid #e0e0e0;
        min-height: 100px;
      }

      .ticket-item.selected {
        background-color: rgba(0, 0, 0, 0.04);
      }

      .ticket-wrapper {
        display: flex;
        align-items: center;
        gap: 12px;
        width: 100%;
      }

      .avatar {
        flex-shrink: 0;
        display: flex;
        align-items: center;
      }

      .ticket-details {
        flex: 1;
        display: flex;
        flex-direction: column;
        gap: 4px;
        font-size: 13px;
      }

      .subject {
        font-weight: 600;
        font-size: 14px;
        line-height: 1.4;
        color: #333;
      }

      .ticket-meta {
        display: flex;
        flex-wrap: wrap;
        gap: 8px 12px;
        align-items: center;
        color: #555;
      }

      .meta-item {
        display: flex;
        align-items: center;
        gap: 4px;
      }
    `,
  ],
})
export class TicketListItemComponent {
  ticket = input.required<TicketSummary>();
  selected = input(false);
  ticketSelected = output<string>();

  onSelect() {
    this.ticketSelected.emit(this.ticket()!.id);
  }
}
