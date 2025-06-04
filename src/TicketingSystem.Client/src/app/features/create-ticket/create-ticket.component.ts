import { FormsModule, NgForm } from '@angular/forms';
import { CreateTicketRequest } from '../../../types/ticket.types';
import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { TicketStateService } from '../../../services/ticket-state.service.ts';
import { FormActionsComponent } from './components/form-actions.component';
import { AvatarComponent } from '../../shared/avatar.component';

@Component({
  selector: 'app-create-ticket',
  template: `
    <form #ticketForm="ngForm" (ngSubmit)="createTicket(ticketForm)" novalidate>
      <app-avatar [avatarUrl]="ticketData.avatarUrl" />
      <button
        mat-button
        type="button"
        (click)="setRandomAvatar()"
        [disabled]="isSubmitting()"
      >
        Random avatar
      </button>

      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Username</mat-label>
        <input
          matInput
          name="username"
          placeholder="e.g. John Doe"
          required
          [(ngModel)]="ticketData.username"
          [disabled]="isSubmitting()"
          #username="ngModel"
        />
        <mat-icon matSuffix>person</mat-icon>
      </mat-form-field>

      @if (username.invalid && (username.dirty || username.touched)) {
        <mat-error>Username is required.</mat-error>
      }

      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Subject</mat-label>
        <input
          matInput
          name="subject"
          placeholder="e.g. Application issue"
          required
          [(ngModel)]="ticketData.subject"
          [disabled]="isSubmitting()"
          #subject="ngModel"
        />
        <mat-icon matSuffix>subject</mat-icon>
      </mat-form-field>

      @if (subject.invalid && (subject.dirty || subject.touched)) {
        <mat-error>Subject is required.</mat-error>
      }

      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Description</mat-label>
        <textarea
          matInput
          name="description"
          rows="6"
          maxlength="2000"
          placeholder="Describe the problem or request in detail..."
          [(ngModel)]="ticketData.description"
          [disabled]="isSubmitting()"
          #description="ngModel"
          required
        ></textarea>
        <mat-icon matSuffix>description</mat-icon>
      </mat-form-field>
      @if (description.invalid && (subject.dirty || subject.touched)) {
        <mat-error>Description is required.</mat-error>
      }

      <ng-template #noDescriptionError></ng-template>

      <app-form-actions
        [submitting]="isSubmitting()"
        [formValid]="ticketForm.valid!"
        (reset)="resetForm(ticketForm)"
      ></app-form-actions>
    </form>
  `,
  imports: [
    FormsModule,
    AvatarComponent,
    FormActionsComponent,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
  ],
  standalone: true,
  styles: [
    `
      form {
        padding: 2rem;
        border-radius: 8px;
        box-shadow: 0 4px 12px rgb(0 0 0 / 0.1);
        background-color: #fff;
        display: flex;
        flex-direction: column;
        gap: 1.5rem;
      }

      app-avatar {
        align-self: center;
        margin-bottom: 1rem;
      }

      mat-form-field {
        width: 100%;
      }

      mat-error {
        color: #d32f2f;
        font-size: 0.875rem;
        margin-top: 0.25rem;
        display: block;
      }

      textarea {
        font-family: inherit;
        font-size: 1rem;
        resize: vertical;
      }

      mat-hint {
        font-size: 0.75rem;
        color: #666;
      }

      @media (max-width: 640px) {
        form {
          margin: 1rem;
          padding: 1rem;
        }
      }
    `,
  ],
})
export class CreateTicketComponent {
  protected readonly ticketState = inject(TicketStateService);

  defaultAvatar = 'https://api.dicebear.com/7.x/open-peeps/svg?seed=default';

  ticketData: CreateTicketRequest = {
    userId: 'e01bafa2-f148-4391-b273-035c03b3fee6',
    username: '',
    subject: '',
    description: '',
    avatarUrl: this.defaultAvatar,
  };

  private submitting = signal(false);

  isSubmitting() {
    return this.submitting();
  }

  createTicket(form: NgForm) {
    if (!form.valid || this.isSubmitting()) {
      return;
    }

    this.submitting.set(true);

    this.ticketState.createTicket(this.ticketData).then(() => {
      this.resetForm(form);
      this.submitting.set(false);
    });
  }

  resetForm(form: NgForm) {
    this.ticketData = {
      userId: 'https://api.dicebear.com/7.x/open-peeps/svg?seed=default',
      username: '',
      subject: '',
      description: '',
      avatarUrl: this.defaultAvatar,
    };
    form.resetForm();
  }

  setRandomAvatar() {
    const seed = Math.random().toString(36).substring(2, 10);
    this.ticketData.avatarUrl = `https://api.dicebear.com/7.x/open-peeps/svg?seed=${seed}`;
  }

  onAvatarError(event: Event) {
    (event.target as HTMLImageElement).src = this.defaultAvatar;
  }
}
