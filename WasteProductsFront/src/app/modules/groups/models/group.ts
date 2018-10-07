import { BoardModel } from './board';
import { UserModel } from './user';

export class GroupInfoModel {
  AdminId: string;
  Name: string;
  Information: string;
}

export class GroupModel extends GroupInfoModel {
  Id: string;
  GroupBoards: Array<BoardModel>;
  GroupUsers: Array<UserModel>;
}


export class GroupOfUserModel extends GroupInfoModel {
  Id: string;
  RightToCreateBoards: boolean;
}
