import { NgModule } from '@angular/core';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule, MatListModule, MatIconModule, MatDividerModule } from '@angular/material';
import { MatMenuModule, MatButtonModule, MatCheckboxModule, MatProgressSpinnerModule, MatChipsModule } from '@angular/material';
import { MatSnackBarModule, MatDialogModule, MatTooltipModule, MatInputModule, MatFormFieldModule } from '@angular/material';
import { MatOptionModule, MatAutocompleteModule, MatTableModule } from '@angular/material';
import { MatPaginatorModule } from '@angular/material';

@NgModule({
  imports: [
    MatCardModule, MatListModule, MatIconModule, MatDividerModule,
    MatMenuModule, MatButtonModule, MatCheckboxModule, MatProgressSpinnerModule, MatChipsModule,
    MatSnackBarModule, MatDialogModule, MatTooltipModule, MatInputModule, MatFormFieldModule, MatOptionModule,
    MatAutocompleteModule, MatTableModule, MatPaginatorModule, MatToolbarModule
  ],
  exports: [
    MatCardModule, MatListModule, MatIconModule, MatDividerModule,
    MatMenuModule, MatButtonModule, MatCheckboxModule, MatProgressSpinnerModule, MatChipsModule,
    MatSnackBarModule, MatDialogModule, MatTooltipModule, MatInputModule, MatFormFieldModule, MatOptionModule,
    MatAutocompleteModule, MatTableModule, MatPaginatorModule, MatToolbarModule
  ]
})
export class MaterialModule { }
