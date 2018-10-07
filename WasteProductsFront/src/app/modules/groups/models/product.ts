export class ProductInfoModel {
  ProductId: string;
  Name: string;
  Information: string;
}

export class ProductModel extends ProductInfoModel {
  Id: string;
  GroupBoardId: string;
}
