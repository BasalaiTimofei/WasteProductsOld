import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { BoardModel } from '../../models/board';
import { ProductModel } from '../../models/product';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css']
})
export class BoardComponent implements OnInit {

  @Input() board: BoardModel;

  @Output() removeBoardEvent: EventEmitter<string> = new EventEmitter<string>();

  constructor() { }

  ngOnInit() {
  }

  edit() {

  }

  remove() {
    this.removeBoardEvent.emit(this.board.Id);
  }

  addProduct() {

  }
  deleteProduct(productId: string, event: Event) {
    this.onItemClick(event);

    // TODO: service
  }

  private onItemClick(event: Event) {
    event.preventDefault();
    event.stopImmediatePropagation();
  }

}
