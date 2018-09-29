import { Component, Input, Inject, HostListener, EventEmitter } from '@angular/core';
import { trigger, state, style, transition, animate, AnimationEvent, group, query } from '@angular/animations';

import { ImagePreviewOverlay } from './image-preview-overlay';
import { FILE_PREVIEW_DIALOG_DATA } from './image-preview-overaly.tokens';

const ESCAPE = 27;
const ANIMATION_TIMINGS = '400ms cubic-bezier(0.25, 0.8, 0.25, 1)';

@Component({
  selector: 'app-image-preview',
  templateUrl: './image-preview.component.html',
  styleUrls: ['./image-preview.component.css'],
  animations: [
    trigger('fade', [
      state('fadeOut', style({ opacity: 0 })),
      state('fadeIn', style({ opacity: 1 })),
      transition('* => fadeIn', animate(ANIMATION_TIMINGS))
    ]),
    trigger('slideContent', [
      state('void', style({ transform: 'translate3d(0, 25%, 0) scale(0.9)', opacity: 0 })),
      state('enter', style({ transform: 'none', opacity: 1 })),
      state('leave', style({ transform: 'translate3d(0, 25%, 0)', opacity: 0 })),
      transition('* => *', animate(ANIMATION_TIMINGS)),
    ])
  ]
})

export class ImagePreviewComponent {
  loading = true;
  animationState: 'void' | 'enter' | 'leave' = 'enter';
  animationStateChanged = new EventEmitter<AnimationEvent>();

  @HostListener('document:keydown', ['$event']) private handleKeydown(event: KeyboardEvent) {
    if (event.keyCode === ESCAPE) {
      this.dialogRef.close();
    }
  }
  constructor(
    public dialogRef: ImagePreviewOverlay,
    @Inject(FILE_PREVIEW_DIALOG_DATA) public image: any) { }
    onLoad(event: Event) {
      this.loading = false;
    }

   onAnimationStart(event: AnimationEvent) {
    this.animationStateChanged.emit(event);
   }

  onAnimationDone(event: AnimationEvent) {
    this.animationStateChanged.emit(event);
  }

  startExitAnimation() {
    this.animationState = 'leave';
  }
}
