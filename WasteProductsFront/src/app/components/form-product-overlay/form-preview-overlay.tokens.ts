import { InjectionToken } from '@angular/core';

import { Form } from '../../services/form-preview/form-preview.service';

export const FILE_PREVIEW_DIALOG_DATA = new InjectionToken<Form>('FILE_PREVIEW_DIALOG_DATA');
