import { Component, input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { AvatarComponent } from '../../../shared/avatar.component';
import { Reply } from '../../../../types/ticket.types';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-reply-item',
  standalone: true,
  imports: [MatCardModule, MatIconModule, AvatarComponent, DatePipe],
  template: `
    <mat-card class="reply">
      <mat-card-content>
        <div class="header">
          <app-avatar [avatarUrl]="reply().avatarUrl" />
          <div class="info">
            <span class="author">{{ reply().agentName }}</span>
            <span class="role">Support Agent</span>
            <span class="date">{{ reply().createdAt | date: 'medium' }}</span>
          </div>
        </div>
        <div class="content">
          <p>{{ reply().message }}</p>
        </div>
      </mat-card-content>
    </mat-card>
  `,
  styles: [
    `
      .reply {
        border-left: 4px solid #4caf50;
        margin-left: 20px;
        border-radius: 12px;
      }

      .header {
        display: flex;
        align-items: center;
        gap: 12px;
        margin-bottom: 12px;
      }

      .info {
        flex: 1;
      }

      .author {
        font-weight: 500;
        color: #333;
        font-size: 0.95rem;
        display: block;
      }

      .role {
        font-size: 0.75rem;
        background: #e8f5e8;
        color: #2e7d32;
        padding: 2px 8px;
        border-radius: 12px;
        display: inline-block;
        margin-top: 2px;
        font-weight: 500;
      }

      .date {
        display: block;
        font-size: 0.8rem;
        color: #666;
      }

      .content {
        font-size: 1rem;
        line-height: 1.6;
        white-space: pre-wrap;
        word-wrap: break-word;
      }

      @media (max-width: 768px) {
        .reply {
          margin-left: 0;
        }
      }
    `,
  ],
})
export class ReplyItemComponent {
  reply = input.required<Reply>();
}
