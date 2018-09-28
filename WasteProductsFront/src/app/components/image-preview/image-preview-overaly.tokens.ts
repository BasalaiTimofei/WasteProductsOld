import { InjectionToken } from '@angular/core';

import { Image } from '../../services/image-preview/image-preview.service';

export const FILE_PREVIEW_DIALOG_DATA = new InjectionToken<Image>('FILE_PREVIEW_DIALOG_DATA');
