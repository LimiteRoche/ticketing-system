import { Component, input, output, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AddReplyRequest } from '../../../../types/ticket.types';

@Component({
  selector: 'app-reply-form',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  template: `
    <mat-card>
      <mat-card-content>
        <h3>
          <mat-icon>reply</mat-icon>
          Add Reply
        </h3>

        @if (isResolved()) {
          <div class="resolved-notice">
            <mat-icon>check_circle</mat-icon>
            <p>
              This ticket has been resolved. No additional replies can be added.
            </p>
          </div>
        } @else {
          <form [formGroup]="replyForm" (ngSubmit)="onSubmit()">
            <mat-form-field appearance="outline">
              <mat-label>Agent Name</mat-label>
              <input
                matInput
                formControlName="agentName"
                placeholder="Enter your name"
              />
              <mat-icon matSuffix>person</mat-icon>
              @if (
                agentNameControl().hasError('required') &&
                agentNameControl().touched
              ) {
                <mat-error>Agent name is required</mat-error>
              }
            </mat-form-field>

            <mat-form-field appearance="outline">
              <mat-label>Reply Message</mat-label>
              <textarea
                matInput
                formControlName="message"
                placeholder="Type your reply here..."
                rows="4"
              ></textarea>
              <mat-icon matSuffix>message</mat-icon>
              @if (
                messageControl().hasError('required') &&
                messageControl().touched
              ) {
                <mat-error>Message is required</mat-error>
              }
              @if (messageControl().hasError('minlength')) {
                <mat-error>Message must be at least 10 characters</mat-error>
              }
            </mat-form-field>

            <div class="form-actions">
              <button mat-raised-button color="primary" type="submit">
                Send Reply
              </button>
              <button mat-button type="button" (click)="onReset()">
                <mat-icon>clear</mat-icon>
                Clear
              </button>
            </div>
          </form>
        }
      </mat-card-content>
    </mat-card>
  `,
  styles: [
    `
      mat-card {
        background-color: #f8f9fa;
        border: 2px dashed #dee2e6;
      }

      h3 {
        display: flex;
        align-items: center;
        gap: 8px;
        margin: 0 0 16px 0;
      }

      form {
        display: flex;
        flex-direction: column;
        gap: 16px;
      }

      mat-form-field {
        width: 100%;
      }

      .form-actions {
        display: flex;
        gap: 12px;
      }

      .resolved-notice {
        display: flex;
        align-items: center;
        gap: 12px;
        padding: 16px;
        background-color: #e8f5e8;
        border: 1px solid #4caf50;
        border-radius: 8px;
        color: #2e7d32;
      }

      .resolved-notice mat-icon {
        color: #4caf50;
      }

      .resolved-notice p {
        margin: 0;
      }
    `,
  ],
})
export class ReplyFormComponent {
  defaultAgentName = input<string>('Support Agent');
  isResolved = input<boolean>(false);
  replySubmit = output<AddReplyRequest>();
  formReset = output<void>();

  private fb = inject(FormBuilder);

  replyForm = this.fb.group({
    agentName: [this.defaultAgentName(), [Validators.required]],
    message: ['', [Validators.required, Validators.minLength(10)]],
  });

  agentNameControl = computed(() => this.replyForm.get('agentName')!);
  messageControl = computed(() => this.replyForm.get('message')!);

  private stringToNumberHash(str: string): number {
    let hash = 0;
    for (let i = 0; i < str.length; i++) {
      hash = (hash * 31 + str.charCodeAt(i)) >>> 0;
    }
    return hash;
  }

  onSubmit(): void {
    if (this.replyForm.invalid) return;

    const formData: AddReplyRequest = {
      avatarUrl: `https://api.dicebear.com/7.x/open-peeps/svg?seed=${this.stringToNumberHash(this.replyForm.value.agentName!)}`,
      agentName: this.replyForm.value.agentName!,
      message: this.replyForm.value.message!,
    };

    this.replySubmit.emit(formData);
  }

  onReset(): void {
    this.replyForm.reset();
    this.replyForm.patchValue({
      agentName: this.defaultAgentName(),
    });
    this.formReset.emit();
  }

  resetForm(): void {
    this.onReset();
  }

  clearMessage(): void {
    this.replyForm.patchValue({ message: '' });
    this.replyForm.get('message')?.markAsUntouched();
  }
}
