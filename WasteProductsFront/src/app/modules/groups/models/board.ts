import { ProductModel } from './product';
import { CommentModel } from './comment';

export class BoardInfoModel {
  CreatorId: string;
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
