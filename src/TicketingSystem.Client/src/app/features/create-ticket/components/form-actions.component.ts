import { Component, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-form-actions',
  standalone: true,
  imports: [MatButtonModule, MatProgressSpinnerModule],
  template: `
    <div class="actions">
      <button
        mat-raised-button
        color="primary"
        type="submit"
        [disabled]="!formValid() || submitting()"
      >
        @if (submitting()) {
          <mat-spinner diameter="20" class="spinner" />
          Generating...
        } @else {
          Create Ticket
        }
      </button>
    </div>
  `,
  styles: [
    `
      .actions {
        display: flex;
        justify-content: flex-end;
        margin-top: 24px;
      }

      .spinner {
        margin-right: 8px;
      }
    `,
  ],
})
export class FormActionsComponent {
  submitting = input(false);
  formValid = input(false);
}
