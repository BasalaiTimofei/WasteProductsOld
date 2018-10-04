import { GroupProduct } from 'src/app/models/groups/groupProduct';
import { GroupComment } from 'src/app/models/groups/groupComment';

export class GroupBoard {
    Id: string;
    Name: string;
    Information: string;
    CreatorId: string;
    GroupId: string;
    GroupProducts: Array<GroupProduct>;
    GroupProductComments: Array<GroupComment>;
}
