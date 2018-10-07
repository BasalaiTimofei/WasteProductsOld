import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { BoardModel, BoardInfoModel } from '../../models/board';
import { BoardService } from '../../services/board.service';
import { ProductModel, ProductInfoModel } from '../../models/product';
import { BehaviorSubject, Observable } from 'rxjs';
import { GroupDialogInfoComponent } from '../group-dialog-info/group-dialog-info.component';
import { MatDialog } from '@angular/material';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { remove } from 'lodash';
import { ConfirmModel } from '../../models/confirm';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css']
})
export class BoardComponent implements OnInit {

  @Input() public board: BoardModel;

  @Output() boardRemovedEvent: EventEmitter<string> = new EventEmitter<string>();

  constructor(
    private boardService: BoardService,
    private dialog: MatDialog) { }

  ngOnInit() {

  }

  editBoard() {
    const dialogRef = this.dialog.open<GroupDialogInfoComponent, { action: string, data: BoardInfoModel }, BoardInfoModel>(
      GroupDialogInfoComponent, {
        // width: '250px',
        data: {
          action: 'Update',
          data: Object.assign(new BoardInfoModel(), this.board)
        }
      });

    dialogRef.afterClosed().subscribe(result => {
      this.updateBoardInfo(result);
    });
  }

  updateBoardInfo(boardInfo: BoardInfoModel) {
    this.boardService.updateBoard(this.board.Id, boardInfo).subscribe(board => this.board = Object.assign(this.board, boardInfo));
  }

  deleteBoard() {
    const dialogRef = this.dialog.open<ConfirmDialogComponent, ConfirmModel, boolean>(
      ConfirmDialogComponent, {
        // width: '250px',
        data: {
          title: 'Подтвердите',
          question: 'Вы действительно хотите удалить борд?'
        }
      });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.boardService.deleteBoard(this.board.Id).subscribe(() => {
          this.boardRemovedEvent.emit(this.board.Id);
        });
      }
    });

  }

  addProduct(event: Event) {
    this.onItemClick(event);

    this.boardService.addProduct(this.board.Id, {Name: 'Some Name', Information: 'Some Info', ProductId: '0'}).subscribe();
  }

  updateProduct(productId: string, event: Event) {
    this.onItemClick(event);

    this.boardService.updateProduct(productId, {Name: 'Some Name', Information: 'Some Info', ProductId: '0'}).subscribe();
  }


  deleteProduct(productId: string, event: Event) {
    this.onItemClick(event);

    const dialogRef = this.dialog.open<ConfirmDialogComponent, ConfirmModel, boolean>(
      ConfirmDialogComponent, {
        // width: '250px',
        data: {
          title: 'Подтвердите',
          question: 'Вы действительно хотите удалить продукт?'
        }
      });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.boardService.deleteProduct(productId).subscribe((r) => {
          remove(this.board.GroupProducts, p => p.Id === productId);

        });
      }
    });
  }

  private onItemClick(event: Event) {
    event.preventDefault();
    event.stopImmediatePropagation();
  }

  private removeProductArray(array: ProductModel[], elementId: string) {
    return array.filter(e => e.Id !== elementId);
  }
}
