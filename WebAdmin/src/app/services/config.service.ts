import { Injectable } from '@angular/core';
import {Config} from "../modules/config";
import {Destination} from "../modules/config";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable, of} from "rxjs";
import {environment} from "../../environments/environment";
import {SessionsService} from "./sessions.service";
import {calcPossibleSecurityContexts} from "@angular/compiler/src/template_parser/binding_parser";



@Injectable({
  providedIn: 'root'
})
export class ConfigService {

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

  public GetAllConfigs():Observable<Config[]> {

    return this.http.get<Config[]>(environment.api + '/Configurations/GetAll',this.options);

  }

  public findById(id: number): Observable<Config> {
    return this.http.get<Config>(environment.api + '/Configurations/GetOne/' + id,this.options);
  }

  public save(config: Config): Observable<Config> {
    if (config.id) {
      return this.http.put<Config>(environment.api + '/Configurations/UpdateOne/'+ config.id,config,this.options);

    } else {
      return this.http.post<Config>(environment.api + '/Configurations/CreatoneOne',config,this.options);
    }
  }
  public delete(config: Config): Observable<unknown>{
    console.log(environment.api + '/Configurations/DeleteOne/'+ config.id)
    return this.http.delete(environment.api + '/Configurations/DeleteOne/'+ config.id,this.options);
  }
}
