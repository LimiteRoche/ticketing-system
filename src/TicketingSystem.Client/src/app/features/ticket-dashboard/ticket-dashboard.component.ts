import { Component, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';

import { TicketStateService } from '../../../services/ticket-state.service.ts.js';
import { TicketListComponent } from '../ticket-list/ticket-list.component.js';
import { TicketDetailComponent } from '../ticket-detail/ticket-detail.component.js';
import { CreateTicketComponent } from '../create-ticket/create-ticket.component.js';

@Component({
  selector: 'app-ticket-dashboard',
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatSidenavModule,
    MatToolbarModule,
    MatDialogModule,
    TicketListComponent,
    TicketDetailComponent,
  ],
  template: `
    <mat-sidenav-container class="container">
      <mat-sidenav mode="side" opened class="sidebar">
        <mat-toolbar color="primary" class="sidebar-toolbar">
          <div class="title-wrapper">
            <mat-icon>confirmation_number</mat-icon>
            <span>Tickets</span>
          </div>
          <span class="spacer"></span>
          <button
            mat-fab
            (click)="openNewTicketDialog()"
            matTooltip="Create New Ticket"
          >
            <mat-icon>add</mat-icon>
          </button>
        </mat-toolbar>

        <app-ticket-list class="flex-grow"></app-ticket-list>
      </mat-sidenav>

      <mat-sidenav-content class="content">
        <mat-toolbar color="primary">
          <span>Ticket Details</span>
          <span class="spacer"></span>
          @if (selectedTicket(); as ticket) {
            <span class="ticket-id">{{ ticket.id }}</span>
          }
        </mat-toolbar>

        <div class="main">
          @if (selectedTicket(); as ticket) {
            <app-ticket-detail [ticket]="ticket"></app-ticket-detail>
          } @else {
            <mat-card class="center-card">
              <mat-icon class="icon">inbox</mat-icon>
              <h2>No Ticket Selected</h2>
              <p>Select a ticket to view its details</p>
              <button
                mat-raised-button
                color="primary"
                (click)="openNewTicketDialog()"
              >
                <mat-icon>add</mat-icon>
                Create New Ticket
              </button>
            </mat-card>
          }
        </div>
      </mat-sidenav-content>
    </mat-sidenav-container>
  `,
  styles: [
    `
      :host {
        height: 100vh;

        mat-sidenav-container,
        .container {
          height: 100vh;
        }

        .spacer {
          flex: 1 1 auto;
        }

        .ticket-id {
          font-family: monospace;
          background: rgba(255, 255, 255, 0.15);
          padding: 4px 8px;
          border-radius: 4px;
        }

        .sidebar {
          width: 420px;
        }

        .sidebar-toolbar {
          display: flex;
          justify-content: space-between;
          align-items: center;

          .title-wrapper {
            display: flex;
            align-items: center;
            gap: 0.5rem;
          }

          button {
            margin-left: auto;
          }
        }

        .content {
          display: flex;
          flex-direction: column;
          height: 100%;
          overflow: hidden;

          mat-toolbar {
            flex-shrink: 0;
          }

          .main {
            flex: 1;
            overflow-y: auto;
            min-height: 0;
            padding: 1.5rem;
            background: #f5f5f5;

            .center-card {
              margin: auto;
              text-align: center;
              padding: 2rem;
              max-width: 400px;

              .icon {
                font-size: 4rem;
                color: #ccc;
              }
            }
          }
        }
      }
    `,
  ],
})
export class TicketDashboardComponent {
  private ticketState = inject(TicketStateService);
  private dialog = inject(MatDialog);

  readonly selectedTicket = computed(() => this.ticketState.selectedTicket());

  openNewTicketDialog() {
    this.dialog
      .open(CreateTicketComponent, {
        width: '600px',
        maxWidth: '90vw',
        maxHeight: '90vh',
        panelClass: 'new-ticket-dialog',
        disableClose: false,
        autoFocus: true,
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          console.log('New ticket created:', result);
        }
      });
  }
}
