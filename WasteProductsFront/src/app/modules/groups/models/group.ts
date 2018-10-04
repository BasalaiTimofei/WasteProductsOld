import { BoardModel } from './board';
import { UserModel } from './user';

export class GroupInfoModel {
  Name: string;
  Information: string;
}

export class GroupModel extends GroupInfoModel {
  Id: string;
  AdminId: string;
  GroupBoards: Array<BoardModel>;
  GroupUsers: Array<UserModel>;
}


export class GroupOfUserModel extends GroupInfoModel {
  Id: string;
  RightToCreateBoards: boolean;
}
