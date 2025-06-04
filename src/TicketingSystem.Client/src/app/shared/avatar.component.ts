import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-avatar',
  template: `
    <img [src]="avatarUrl || defaultAvatar" alt="Avatar" class="avatar-view" />
  `,
  styles: [
    `
      .avatar-view {
        width: 68px;
        height: 68px;
        border-radius: 50%;
        object-fit: cover;
        border: 1px solid #ccc;
      }
    `,
  ],
})
export class AvatarComponent {
  @Input() avatarUrl = '';
  public defaultAvatar =
    'https://api.dicebear.com/7.x/open-peeps/svg?seed=default';
}
