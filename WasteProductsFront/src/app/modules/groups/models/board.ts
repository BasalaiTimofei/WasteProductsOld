import { ProductModel } from './product';
import { CommentModel } from './comment';

export class BoardInfoModel {
  Name: string;
  Information: string;
}

export class BoardModel extends BoardInfoModel {
  Id: string;
  CreatorId: string;
  GroupId: string;
  GroupProducts: Array<ProductModel>;
  GroupProductComments: Array<CommentModel>;
}
