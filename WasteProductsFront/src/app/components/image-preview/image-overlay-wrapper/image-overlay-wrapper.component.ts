import { Component, OnInit, HostBinding } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';

import { ImagePreviewOverlay } from '../image-preview-overlay';

@Component({
  selector: 'app-image-overlay-wrapper',
  templateUrl: './image-overlay-wrapper.component.html',
  styleUrls: ['./image-overlay-wrapper.component.css'],
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

  @HostBinding('@slideDown') slideDown = 'enter';

  constructor(private dialogRef: ImagePreviewOverlay) { }

  ngOnInit() {
    this.dialogRef.beforeClose().subscribe(() => this.slideDown = 'leave');
  }

}
