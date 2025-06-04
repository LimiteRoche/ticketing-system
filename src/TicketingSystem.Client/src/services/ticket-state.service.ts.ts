import { Injectable, inject, resource, signal, computed } from '@angular/core';
import { TicketApiService } from './ticket.service';
import { CreateTicketRequest, AddReplyRequest } from '../types/ticket.types';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TicketStateService {
  private api = inject(TicketApiService);

  private refreshTrigger = signal(0);
  private selectedTicketId = signal<string | null>(null);

  readonly ticketsResource = resource({
    params: () => ({ trigger: this.refreshTrigger() }),
    loader: () => firstValueFrom(this.api.getTickets()),
  });

  readonly selectedTicketResource = resource({
    params: () => ({ ticketId: this.selectedTicketId() }),
    loader: ({ params }) =>
      params.ticketId
        ? firstValueFrom(this.api.getTicket(params.ticketId))
        : Promise.resolve(undefined),
  });

  readonly tickets = computed(() => this.ticketsResource.value() ?? []);
  readonly selectedTicket = computed(() => this.selectedTicketResource.value());

  readonly ticketsStatus = computed(() => this.ticketsResource.status());
  readonly selectedTicketStatus = computed(() =>
    this.selectedTicketResource.status()
  );

  readonly ticketsError = computed(() => this.ticketsResource.error());
  readonly selectedTicketError = computed(() =>
    this.selectedTicketResource.error()
  );

  readonly ticketsHasValue = computed(() => this.ticketsResource.hasValue());
  readonly selectedTicketHasValue = computed(() =>
    this.selectedTicketResource.hasValue()
  );

  readonly openTickets = computed(() =>
    this.ticketsHasValue()
      ? this.tickets().filter((t) => t.status === 'Open')
      : []
  );
  readonly inResolutionTickets = computed(() =>
    this.ticketsHasValue()
      ? this.tickets().filter((t) => t.status === 'InResolution')
      : []
  );

  readonly ticketStats = computed(() => ({
    total: this.tickets().length,
    open: this.openTickets().length,
    inResolution: this.inResolutionTickets().length,
  }));

  selectTicket(id: string | null): void {
    this.selectedTicketId.set(id);
  }

  refreshTickets(): void {
    this.ticketsResource.reload();
  }

  forceRefreshTickets(): void {
    this.refreshTrigger.update((v) => v + 1);
  }

  refreshSelectedTicket(): void {
    this.selectedTicketResource.reload();
  }

  createTicket(request: CreateTicketRequest): Promise<void> {
    return firstValueFrom(this.api.createTicket(request))
      .then(() => {
        this.refreshTickets();
      })
      .catch((err) => {
        console.error('Error creating ticket:', err);
        throw err;
      });
  }

  addReply(ticketId: string, request: AddReplyRequest): Promise<void> {
    return firstValueFrom(this.api.addReply(ticketId, request))
      .then(() => {
        if (this.selectedTicketId() === ticketId) {
          this.refreshSelectedTicket();
          this.refreshTickets();
        }
      })
      .catch((err) => {
        console.error('Error replying: ', err);
        return Promise.reject(err);
      });
  }

  resolveTicket(ticketId: string): void {
    this.api.resolveTicket(ticketId).subscribe({
      next: () => {
        this.refreshTickets();
        if (this.selectedTicketId() === ticketId) {
          this.refreshSelectedTicket();
        }
      },
      error: (err) => {
        console.error('Error resolving ticket:', err);
      },
    });
  }

  private isStatus(resourceStatus: () => string, status: string): boolean {
    return resourceStatus() === status;
  }

  isTicketsIdle() {
    return this.isStatus(this.ticketsStatus, 'idle');
  }
  isTicketsLoading() {
    return this.isStatus(this.ticketsStatus, 'loading');
  }
  isTicketsReloading() {
    return this.isStatus(this.ticketsStatus, 'reloading');
  }
  isTicketsResolved() {
    return this.isStatus(this.ticketsStatus, 'resolved');
  }
  isTicketsError() {
    return this.isStatus(this.ticketsStatus, 'error');
  }

  isSelectedTicketIdle() {
    return this.isStatus(this.selectedTicketStatus, 'idle');
  }
  isSelectedTicketLoading() {
    return this.isStatus(this.selectedTicketStatus, 'loading');
  }
  isSelectedTicketReloading() {
    return this.isStatus(this.selectedTicketStatus, 'reloading');
  }
  isSelectedTicketResolved() {
    return this.isStatus(this.selectedTicketStatus, 'resolved');
  }
  isSelectedTicketError() {
    return this.isStatus(this.selectedTicketStatus, 'error');
  }
}
