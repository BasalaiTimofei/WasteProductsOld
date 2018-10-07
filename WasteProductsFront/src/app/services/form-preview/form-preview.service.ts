import { Injectable, Injector, ComponentRef } from '@angular/core';
import { Overlay, OverlayConfig, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal, PortalInjector } from '@angular/cdk/portal';

import { FormProductOverlayComponent } from '../../components/form-product-overlay/form-product-overlay.component';
import { FormPreviewOverlay } from '../../components/form-product-overlay/form-preview-overlay';
import { FILE_PREVIEW_DIALOG_DATA } from '../../components/form-product-overlay/form-preview-overlay.tokens';

export interface Form {
  name: string;
  id: string;
  searchQuery?: string;
  rate?: number;
  comment?: string;
}

interface FilePreviewDialogConfig {
  panelClass?: string;
  hasBackdrop?: boolean;
  backdropClass?: string;
  form?: Form;
}

const DEFAULT_CONFIG: FilePreviewDialogConfig = {
  hasBackdrop: true,
  backdropClass: 'dark-backdrop',
  panelClass: 'tm-file-preview-dialog-panel',
  form: null
};

@Injectable()
export class FormPreviewService {

  constructor(
    private injector: Injector,
    private overlay: Overlay) { }

open(config: FilePreviewDialogConfig = {}) {

  const dialogConfig = { ...DEFAULT_CONFIG, ...config };
  const overlayRef = this.createOverlay(dialogConfig);
  const dialogRef = new FormPreviewOverlay(overlayRef);

  const overlayComponent = this.attachDialogContainer(overlayRef, dialogConfig, dialogRef);
  dialogRef.componentInstance = overlayComponent;
  overlayRef.backdropClick().subscribe(_ => dialogRef.close());

  return dialogRef;
}

private createOverlay(config: FilePreviewDialogConfig) {
  const overlayConfig = this.getOverlayConfig(config);
  return this.overlay.create(overlayConfig);
}

private attachDialogContainer(overlayRef: OverlayRef, config: FilePreviewDialogConfig, dialogRef: FormPreviewOverlay) {
  const injector = this.createInjector(config, dialogRef);

  const containerPortal = new ComponentPortal(FormProductOverlayComponent, null, injector);
  const containerRef: ComponentRef<FormProductOverlayComponent> = overlayRef.attach(containerPortal);

  return containerRef.instance;
}

private createInjector(config: FilePreviewDialogConfig, dialogRef: FormPreviewOverlay): PortalInjector {
  const injectionTokens = new WeakMap();

  injectionTokens.set(FormPreviewOverlay, dialogRef);
  injectionTokens.set(FILE_PREVIEW_DIALOG_DATA, config.form);

  return new PortalInjector(this.injector, injectionTokens);
}

private getOverlayConfig(config: FilePreviewDialogConfig): OverlayConfig {
  const positionStrategy = this.overlay.position()
    .global()
    .centerHorizontally()
    .centerVertically();

  const overlayConfig = new OverlayConfig({
    hasBackdrop: config.hasBackdrop,
    backdropClass: config.backdropClass,
    panelClass: config.panelClass,
    scrollStrategy: this.overlay.scrollStrategies.block(),
    positionStrategy
  });

  return overlayConfig;
  }
}
