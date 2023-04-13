import { Injectable } from '@angular/core';
import {Report} from "../modules/report";
import {Client} from "../modules/client";
import {environment} from "../../environments/environment";
import {Observable} from "rxjs";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {SessionsService} from "./sessions.service";
@Injectable({
  providedIn: 'root'
})
export class ReportService {

  public get options(): { headers: HttpHeaders }  {
    return {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + this.sessions.token
      })
    };
  }

  constructor(private http: HttpClient,
              private sessions: SessionsService) { }

  public GetAllReports():Observable<Report[]>{
    return this.http.get<Report[]>(environment.api + '/Reports/GetAll',this.options);
  }
  public GetAllErrors():Observable<Report[]>{

    return this.http.get<Report[]>(environment.api + '/Reports/GetAllErrors',this.options);
  }
  public GetAllReportWithParametersDate(dateto:string,datefrom:string):Observable<Report[]>{
    return this.http.get<Report[]>(environment.api + '/Reports/GetAllWithParamaters/' + datefrom + '/' + dateto,this.options);
  }
  public GetAllReportWithParametersDateStatus(dateto:string,datefrom:string,status:boolean):Observable<Report[]>{
    return this.http.get<Report[]>(environment.api + '/Reports/GetAllWithParamaters/'+ status + '/' + datefrom + '/' + dateto,this.options);
  }
  public GetAllReportWithParametersStatus(status:boolean):Observable<Report[]>{
    return this.http.get<Report[]>(environment.api + '/Reports/GetAllWithParamaters/' + status,this.options);
  }



}
