import { Component, OnInit, HostBinding } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';

import { ImagePreviewOverlay } from '../image-preview-overlay';

@Component({
  selector: 'app-image-overlay-wrapper',
  templateUrl: './image-overlay-wrapper.component.html',
  styleUrls: ['./image-overlay-wrapper.component.scss'],
  animations: [
    trigger('slideDown', [
      state('void', style({ transform: 'translateY(-100%)' })),
      state('enter', style({ transform: 'translateY(0)' })),
      state('leave', style({ transform: 'translateY(-100%)' })),
      transition('* => *', animate('400ms cubic-bezier(0.25, 0.8, 0.25, 1)'))
    ])
  ]
})
export class ImageOverlayWrapperComponent implements OnInit {
  // Apply animation to the host element
  @HostBinding('@slideDown') slideDown = 'enter';

  // Inject remote control
  constructor(private dialogRef: ImagePreviewOverlay) { }

  ngOnInit() {
    // Animate toolbar out before overlay is closed
    this.dialogRef.beforeClose().subscribe(() => this.slideDown = 'leave');
  }

}
