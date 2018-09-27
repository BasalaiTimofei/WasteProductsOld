import { Injectable, Injector, ComponentRef } from '@angular/core';
import { Overlay, OverlayConfig, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal, PortalInjector } from '@angular/cdk/portal';

import { ImagePreviewComponent } from '../../components/image-preview/image-preview.component';
import { ImagePreviewOverlay } from '../../components/image-preview/image-preview-overlay';
import { FILE_PREVIEW_DIALOG_DATA } from '../../components/image-preview/image-preview-overaly.tokens';

export interface Image {
  name: string;
  url: string;
}

interface FilePreviewDialogConfig {
  panelClass?: string;
  hasBackdrop?: boolean;
  backdropClass?: string;
  image?: Image;
}

const DEFAULT_CONFIG: FilePreviewDialogConfig = {
  hasBackdrop: true,
  backdropClass: 'dark-backdrop',
  panelClass: 'tm-file-preview-dialog-panel',
  image: null
};

@Injectable()
export class ImagePreviewService {

  constructor(
    private injector: Injector,
    private overlay: Overlay) { }

open(config: FilePreviewDialogConfig = {}) {

  const dialogConfig = { ...DEFAULT_CONFIG, ...config };
  const overlayRef = this.createOverlay(dialogConfig);
  const dialogRef = new ImagePreviewOverlay(overlayRef);

  const overlayComponent = this.attachDialogContainer(overlayRef, dialogConfig, dialogRef);
  dialogRef.componentInstance = overlayComponent;
  overlayRef.backdropClick().subscribe(_ => dialogRef.close());

  return dialogRef;
}

private createOverlay(config: FilePreviewDialogConfig) {
  const overlayConfig = this.getOverlayConfig(config);
  return this.overlay.create(overlayConfig);
}

private attachDialogContainer(overlayRef: OverlayRef, config: FilePreviewDialogConfig, dialogRef: ImagePreviewOverlay) {
  const injector = this.createInjector(config, dialogRef);

  const containerPortal = new ComponentPortal(ImagePreviewComponent, null, injector);
  const containerRef: ComponentRef<ImagePreviewComponent> = overlayRef.attach(containerPortal);

  return containerRef.instance;
}

private createInjector(config: FilePreviewDialogConfig, dialogRef: ImagePreviewOverlay): PortalInjector {
  const injectionTokens = new WeakMap();

  injectionTokens.set(ImagePreviewOverlay, dialogRef);
  injectionTokens.set(FILE_PREVIEW_DIALOG_DATA, config.image);

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

