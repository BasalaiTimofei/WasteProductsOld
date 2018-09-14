import { NgModule } from '@angular/core';

import { MatCardModule, MatListModule, MatIconModule, MatDividerModule } from '@angular/material';
import { MatMenuModule, MatButtonModule, MatCheckboxModule, MatProgressSpinnerModule, MatChipsModule } from '@angular/material';
import { MatSnackBarModule, MatDialogModule, MatTooltipModule, MatInputModule } from '@angular/material';

@NgModule({
  imports: [
    MatCardModule, MatListModule, MatIconModule, MatDividerModule,
    MatMenuModule, MatButtonModule, MatCheckboxModule, MatProgressSpinnerModule, MatChipsModule,
    MatSnackBarModule, MatDialogModule, MatTooltipModule, MatInputModule
  ],
  exports: [
    MatCardModule, MatListModule, MatIconModule, MatDividerModule,
    MatMenuModule, MatButtonModule, MatCheckboxModule, MatProgressSpinnerModule, MatChipsModule,
    MatSnackBarModule, MatDialogModule, MatTooltipModule, MatInputModule
  ]
})
export class MaterialModule { }
