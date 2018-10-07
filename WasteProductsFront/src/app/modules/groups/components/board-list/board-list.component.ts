import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { BoardModel } from '../../models/board';
import { BoardService } from '../../services/board.service';

@Component({
  selector: 'app-board-list',
  templateUrl: './board-list.component.html',
  styleUrls: ['./board-list.component.css']
})
export class BoardListComponent implements OnInit {

  @Input() boards: BoardModel[];

  @Output() listChangedEvent: EventEmitter<any> = new EventEmitter<any>();


  constructor(private boardService: BoardService) { }

  ngOnInit() {
  }

  addBoard() {


  }

  removeBoard(boardId: any) {
    this.boardService.deleteBoard(boardId).subscribe(() => {
      this.listChangedEvent.emit(null);
    });
  }
}
