import { Component, input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { ReplyItemComponent } from './reply-item.component';
import { Reply } from '../../../../types/ticket.types';

@Component({
  selector: 'app-replies-list',
  standalone: true,
  imports: [MatIconModule, ReplyItemComponent],
  template: `
    @if (replies() && replies().length > 0) {
      <div class="replies-section">
        <h3 class="section-title">
          <mat-icon>forum</mat-icon>
          Replies ({{ replies().length }})
        </h3>

        @for (reply of replies(); track reply.id) {
          <app-reply-item [reply]="reply" />
        }
      </div>
    }
  `,
  styles: [
    `
      .replies-section {
        display: flex;
        flex-direction: column;
        gap: 16px;
      }

      .section-title {
        display: flex;
        align-items: center;
        gap: 8px;
        margin: 0 0 8px 0;
        font-size: 1.1rem;
        font-weight: 500;
      }
    `,
  ],
})
export class RepliesListComponent {
  replies = input.required<Reply[]>();
}
