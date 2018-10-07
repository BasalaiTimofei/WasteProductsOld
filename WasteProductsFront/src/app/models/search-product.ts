export class SearchProduct {
  constructor(
   public Id: string,
   public Name: string,
   public IsAdded: boolean,
   public PicturePath: string,
   public Composition: string,
   public AvgRating: number
 ) {  }
}
