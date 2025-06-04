import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  Ticket,
  TicketSummary,
  CreateTicketRequest,
  AddReplyRequest,
} from '../types/ticket.types';

@Injectable({
  providedIn: 'root',
})
export class TicketApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = 'https://localhost:7028/api';

  getTickets(): Observable<TicketSummary[]> {
    return this.http.get<TicketSummary[]>(`${this.baseUrl}/tickets`);
  }

  getTicket(ticketId: string): Observable<Ticket> {
    return this.http.get<Ticket>(`${this.baseUrl}/tickets/${ticketId}`);
  }

  createTicket(request: CreateTicketRequest): Observable<Ticket> {
    return this.http.post<Ticket>(`${this.baseUrl}/tickets`, request);
  }

  addReply(ticketId: string, request: AddReplyRequest): Observable<void> {
    return this.http.post<void>(
      `${this.baseUrl}/tickets/${ticketId}/replies`,
      request
    );
  }

  resolveTicket(ticketId: string): Observable<void> {
    return this.http.patch<void>(
      `${this.baseUrl}/tickets/${ticketId}/resolve`,
      {}
    );
  }
}
