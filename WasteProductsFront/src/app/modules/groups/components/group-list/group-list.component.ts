import { Component, OnInit, Input } from '@angular/core';
import { GroupModel } from '../../models/group';

@Component({
  selector: 'app-group-list',
  templateUrl: './group-list.component.html',
  styleUrls: ['./group-list.component.css']
})
export class GroupListComponent implements OnInit {

  @Input() groups: GroupModel[] = [];

  constructor() { }

  ngOnInit() {
  }

}
