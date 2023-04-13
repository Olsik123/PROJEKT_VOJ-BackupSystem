import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {SessionsService} from "./sessions.service";
import {Observable} from "rxjs";
import {Config} from "../modules/config";
import {environment} from "../../environments/environment";
import {Setting} from "../modules/setting";

@Injectable({
  providedIn: 'root'
})
export class SettingsService {

  constructor(private http: HttpClient,
              private sessions: SessionsService) {
  }
  public get options(): { headers: HttpHeaders }  {
    return {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + this.sessions.token
      })
    };
  }

  public findById(id: string|null): Observable<Setting> {
    let idn = Number(id)
    return this.http.get<Setting>(environment.api + '/api/Admins/GetById/' + idn,this.options);
  }

  public save(setting: Setting): Observable<Setting> {
      return this.http.put<Setting>(environment.api + '/api/Admins/UpdateOne/'+ setting.id,setting,this.options);
 }

}
