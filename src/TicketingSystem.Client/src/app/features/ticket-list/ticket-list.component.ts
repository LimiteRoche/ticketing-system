import { Component, computed, inject, signal } from '@angular/core';

import { TicketStateService } from '../../../services/ticket-state.service.ts';

import { TicketStatsComponent } from './components/ticket-stats.component';
import { TicketFiltersComponent } from './components/ticket-filters.component';
import { TicketErrorComponent } from './components/ticket-error.component';
import { TicketIdleComponent } from './components/ticket-idle.component';
import { TicketLoadingComponent } from './components/ticket-loading';
import { TicketEmptyComponent } from './components/ticket-empty';
import { TicketListHeaderComponent } from './components/ticket-list-header.component';
import { TicketListBodyComponent } from './components/ticket-list-body.component';

type TicketFilter = 'all' | 'Open' | 'InResolution';

@Component({
  selector: 'app-ticket-list',
  imports: [
    TicketStatsComponent,
    TicketFiltersComponent,
    TicketErrorComponent,
    TicketIdleComponent,
    TicketLoadingComponent,
    TicketEmptyComponent,
    TicketListHeaderComponent,
    TicketListBodyComponent,
  ],
  template: `
    <div class="ticket-list-container">
      <app-ticket-stats [stats]="ticketStats()" />

      <app-ticket-filters
        [stats]="ticketStats()"
        [selectedFilter]="currentFilter()"
        (filterChange)="setFilter($event)"
      />

      @switch (ticketStatus()) {
        @case ('loading') {
          <app-ticket-loading [reloading]="isReloading()" />
        }

        @case ('error') {
          <app-ticket-error
            [errorMessage]="ticketState.ticketsError()?.message"
            (retry)="refreshTickets()"
          />
        }

        @case ('loaded') {
          <app-ticket-list-header
            [count]="filteredTickets().length"
            [filter]="currentFilter()"
            [reloading]="isReloading()"
            (refresh)="refreshTickets()"
          />

          @if (filteredTickets().length === 0) {
            <app-ticket-empty [filter]="currentFilter()" />
          } @else {
            <app-ticket-list-body
              [tickets]="filteredTickets()"
              [selectedId]="ticketState.selectedTicket()?.id ?? null"
              (ticketSelected)="selectTicket($event)"
            />
          }
        }

        @case ('idle') {
          <app-ticket-idle (load)="refreshTickets()" />
        }
      }
    </div>
  `,
  styles: [
    `
      .ticket-list-container {
        flex: 1;
      }
    `,
  ],
})
export class TicketListComponent {
  public readonly ticketState = inject(TicketStateService);
  private readonly currentFilterSignal = signal<TicketFilter>('all');

  readonly currentFilter = computed(() => this.currentFilterSignal());

  readonly ticketStatus = computed<'idle' | 'loading' | 'error' | 'loaded'>(
    () => {
      if (
        this.ticketState.isTicketsLoading() ||
        this.ticketState.isTicketsReloading()
      ) {
        return 'loading';
      }
      if (this.ticketState.isTicketsError()) {
        return 'error';
      }
      if (this.ticketState.ticketsHasValue()) {
        return 'loaded';
      }
      if (this.ticketState.isTicketsIdle()) {
        return 'idle';
      }
      return 'idle';
    }
  );

  readonly isReloading = computed(() => this.ticketState.isTicketsReloading());

  readonly ticketStats = computed(() => this.ticketState.ticketStats());

  readonly filteredTickets = computed(() => {
    const tickets = this.ticketState.tickets();
    const filter = this.currentFilterSignal();

    switch (filter) {
      case 'Open':
        return tickets.filter((t) => t.status === 'Open');
      case 'InResolution':
        return tickets.filter((t) => t.status === 'InResolution');
      default:
        return tickets;
    }
  });

  setFilter(filter: TicketFilter): void {
    this.currentFilterSignal.set(filter);
  }

  selectTicket(ticketId: string): void {
    this.ticketState.selectTicket(ticketId);
  }

  refreshTickets(): void {
    this.ticketState.refreshTickets();
  }
}
