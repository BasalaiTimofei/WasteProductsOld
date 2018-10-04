import { Component, Inject, HostListener, EventEmitter } from '@angular/core';
import { trigger, state, style, transition, animate, AnimationEvent } from '@angular/animations';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';

import { FormPreviewOverlay } from './form-preview-overlay';
import { FILE_PREVIEW_DIALOG_DATA } from './form-preview-overlay.tokens';
import { ProductService } from '../../services/product/product.service';

const ESCAPE = 27;
const ANIMATION_TIMINGS = '400ms cubic-bezier(0.25, 0.8, 0.25, 1)';

@Component({
  selector: 'app-form-product-overlay',
  templateUrl: './form-product-overlay.component.html',
  styleUrls: ['./form-product-overlay.component.css'],
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
export class FormProductOverlayComponent {
  animationState: 'void' | 'enter' | 'leave' = 'enter';
  animationStateChanged = new EventEmitter<AnimationEvent>();
  errorValidation = false;

  @HostListener('document:keydown', ['$event']) private handleKeydown(event: KeyboardEvent) {
    if (event.keyCode === ESCAPE) {
      this.closeForm();
    }
  }
  constructor(
    public dialogRef: FormPreviewOverlay,
    @Inject(FILE_PREVIEW_DIALOG_DATA) public form: any,
    private productService: ProductService,
    public snackBar: MatSnackBar,
    private router: Router) { }

  addToMyProducts(comment: string, rate: number) {
    if (!comment) {
      this.errorValidation = true;
    } else {
      this.productService.addProductDescription(rate, comment, this.form.id);
      this.closeForm();
      this.snackBar.open('Продукт добавлен успешно!', null, {
            duration: 4000,
            verticalPosition: 'top',
            horizontalPosition: 'center'
      });
    }
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

  closeForm() {
    this.dialogRef.close();
  }
}
