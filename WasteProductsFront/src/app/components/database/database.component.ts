import { Component, OnInit } from '@angular/core';

import { DatabaseState } from '../../models/database/database-state';
import { DatabaseService } from '../../services/database/database.service';

@Component({
  selector: 'app-database',
  templateUrl: './database.component.html',
  styleUrls: ['./database.component.css']
})
export class DatabaseComponent implements OnInit {

  state: DatabaseState = null;

  constructor(private databaseService: DatabaseService) { }

  ngOnInit() {
    this.getDatabaseState();
  }

  getDatabaseState(): void {
    this.databaseService.getState()
      .subscribe(result => this.state = result);
  }

  reCreateDatabase(withTestData: boolean): void {
    this.databaseService.reCreate(withTestData)
      .subscribe(result => this.getDatabaseState());
  }

  deleteDatabase(): void {
    this.databaseService.delete()
      .subscribe(result => this.getDatabaseState());
  }

}
