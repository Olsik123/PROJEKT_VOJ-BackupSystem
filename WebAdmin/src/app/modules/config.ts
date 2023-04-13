export class Config {
  public id:number = 0;
  public adminId: number = 1;
  public alias:string ='';
  public clientIds:number[] = [];
  public clients:string[] =[''];
  public format:string ='';
  public type:string ='';
  public frequency:string ='';
  public retention:number=0;
  public packages:number=0;
  public sources:string[] =[];
  public destinations:Destination[] = [];
}
export interface Destination {
  place:string;
  path:string;
  host:string;
}
