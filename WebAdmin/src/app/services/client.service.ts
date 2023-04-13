import { Injectable } from '@angular/core';
import {Client} from "../modules/client";
import {Observable} from "rxjs";
import {Config} from "../modules/config";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import { HttpHeaders } from '@angular/common/http';
import { SessionsService } from './sessions.service';

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  public get options(): { headers: HttpHeaders }  {
    return {
      headers: new HttpHeaders({
        'Authorization': 'Bearer ' + this.sessions.token
      })
    };
  }


  constructor(private http: HttpClient,
    private sessions: SessionsService) { }

  public GetAllClients():Observable<Client[]>{

    return this.http.get<Client[]>(environment.api + '/Stations/GetAll',this.options);

  }
  public GetUnverified():Observable<Client[]> {

    return this.http.get<Client[]>(environment.api + '/Stations/GetUnverified',this.options);

  }
  public GetVerifiedAndUnblocked():Observable<Client[]> {

    return this.http.get<Client[]>(environment.api + '/Stations/GetVerifiedAndUnblocked',this.options);

  }
  public AddTrust(id:number):Observable<Client[]>{
    return this.http.put<any>(environment.api + '/Stations/Trust/' + id ,this.options);
  }

  public save(client: Client): Observable<Client> {
      return this.http.put<Client>(environment.api + '/Stations/UpdateOne/' + client.id, client,this.options);
  }
  public findById(id: number): Observable<Client[]> {
    return this.http.get<Client[]>(environment.api + '/Stations/GetOne/' + id,this.options);
  }

}
