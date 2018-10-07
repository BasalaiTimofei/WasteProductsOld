import { NgModule } from '@angular/core';

import {
  MatCardModule, MatListModule, MatIconModule, MatDividerModule, MatToolbarModule, MatMenuModule,
  MatButtonModule, MatCheckboxModule, MatProgressSpinnerModule, MatChipsModule, MatSnackBarModule,
   MatDialogModule, MatTooltipModule, MatInputModule, MatFormFieldModule, MatOptionModule,
   MatAutocompleteModule, MatTableModule, MatPaginatorModule, MatSelectModule, MatExpansionModule
} from '@angular/material';

/* Popups & Modals */
import { MatBottomSheetModule } from '@angular/material';

@NgModule({
  imports: [
    MatCardModule, MatListModule, MatIconModule, MatDividerModule,
    MatMenuModule, MatButtonModule, MatCheckboxModule, MatProgressSpinnerModule, MatChipsModule,
    MatSnackBarModule, MatDialogModule, MatTooltipModule, MatInputModule, MatFormFieldModule, MatOptionModule,
    MatAutocompleteModule, MatTableModule, MatPaginatorModule, MatToolbarModule, MatSelectModule, MatExpansionModule,

    /* Popups & Modals */
    MatBottomSheetModule,
  ],
  exports: [
    MatCardModule, MatListModule, MatIconModule, MatDividerModule,
    MatMenuModule, MatButtonModule, MatCheckboxModule, MatProgressSpinnerModule, MatChipsModule,
    MatSnackBarModule, MatDialogModule, MatTooltipModule, MatInputModule, MatFormFieldModule, MatOptionModule,
    MatAutocompleteModule, MatTableModule, MatPaginatorModule, MatToolbarModule, MatSelectModule, MatExpansionModule,

    /* Popups & Modals */
    MatBottomSheetModule
  ]
})
export class MaterialModule { }
