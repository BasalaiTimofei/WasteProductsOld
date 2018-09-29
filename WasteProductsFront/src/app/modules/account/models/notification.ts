export class Notification {
  constructor(
    public Id: string,
    public Read: boolean,
    public Date: Date,
    public Subject: string,
    public Message: string) {
  }
}
