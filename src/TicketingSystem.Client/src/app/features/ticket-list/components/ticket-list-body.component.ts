import { Component, input, output } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { MatDividerModule } from '@angular/material/divider';
import { TicketListItemComponent } from './ticket-list-item.component';
import { TicketSummary } from '../../../../types/ticket.types';

@Component({
  selector: 'app-ticket-list-body',
  standalone: true,
  imports: [MatListModule, MatDividerModule, TicketListItemComponent],
  template: `
    <mat-nav-list>
      @for (ticket of tickets(); track ticket.id) {
        <app-ticket-list-item
          [ticket]="ticket"
          [selected]="selectedId() === ticket.id"
          (ticketSelected)="ticketSelected.emit($event)"
        />
        @if (!$last) {
          <mat-divider />
        }
      }
    </mat-nav-list>
  `,
  styles: [
    `
      mat-nav-list {
        flex: 1;
        overflow-y: auto;
      }
    `,
  ],
})
export class TicketListBodyComponent {
  tickets = input<TicketSummary[]>([]);
  selectedId = input<string | null>(null);
  ticketSelected = output<string>();
}
