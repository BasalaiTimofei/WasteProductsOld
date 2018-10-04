import { GroupBoard } from 'src/app/models/groups/groupBoard';
import { GroupUser } from 'src/app/models/groups/groupUser';

export class Group {
    Id: string;
    Name: string;
    AdminId: string;
    Information: string;
    GroupBoards: Array<GroupBoard>;
    GroupUsers: Array<GroupUser>;
}
