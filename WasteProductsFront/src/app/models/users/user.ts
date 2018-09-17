import { UserProductDescription } from './user-product-description';

export class User {
  Id: string;
  UserName: string;
  Friends: User[];
  ProductDescriptions: UserProductDescription[];
}
