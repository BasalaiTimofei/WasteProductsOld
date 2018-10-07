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
import { BoardProductDialogComponent } from '../board-product-dialog/board-product-dialog.component';
import { BoardDialogInfoComponent } from '../board-dialog-info/board-dialog-info.component';

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
    const dialogRef = this.dialog.open<BoardDialogInfoComponent, { action: string, data: BoardInfoModel }, BoardInfoModel>(
      BoardDialogInfoComponent, {
        // width: '250px',
        data: {
          action: 'Изменить',
          data: Object.assign(new BoardInfoModel(), this.board)
        }
      });

    dialogRef.afterClosed().subscribe(boardInfo => {
      if (boardInfo) {
        this.updateBoardInfo(boardInfo);
      }
    });
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

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.boardService.deleteBoard(this.board.Id)
          .subscribe(() => this.boardRemovedEvent.emit(this.board.Id));
      }
    });
  }

  private updateBoardInfo(boardInfo: BoardInfoModel) {
    this.boardService.updateBoard(this.board.Id, boardInfo).subscribe(board => this.board = Object.assign(this.board, boardInfo));
  }


  /* Products */

  addProduct() {
    const dialogRef = this.dialog.open<BoardProductDialogComponent, any, ProductInfoModel>(
      BoardProductDialogComponent, {
        width: '500px',
        height: '600px'
      });

    dialogRef.afterClosed().subscribe(productInfo => {
      if (productInfo) {
        this.boardService.addProduct(this.board.Id, productInfo).subscribe(
          product => {
            if (product) {
              this.board.GroupProducts.push(product);
            }
          }
        );
      }
    });
  }

  deleteProduct(productId: string) {
    const dialogRef = this.dialog.open<ConfirmDialogComponent, ConfirmModel, boolean>(
      ConfirmDialogComponent, {
        // width: '250px',
        data: {
          title: 'Подтвердите',
          question: 'Вы действительно хотите удалить продукт?'
        }
      });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.boardService.deleteProduct(productId)
          .subscribe(() => remove(this.board.GroupProducts, p => p.Id === productId));
      }
    });
  }

  editProduct(productId: string) {



  }

  private updateProduct(productId: string) {

    this.boardService.updateProduct(productId, { Name: 'Some Name', Information: 'Some Info', ProductId: '0' }).subscribe();
  }
}
