import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {catchError, map, Observable, of, tap} from "rxjs";
import {environment} from "../../environments/environment";
import {JwtHelperService} from "@auth0/angular-jwt";


@Injectable({
  providedIn: 'root'
})
export class SessionsService {

  public token: string|null = null;
  public adminId: string|null = null;

  constructor(private http: HttpClient,
              private  jwt: JwtHelperService) {
    this.token = this.loadToken();
    this.adminId = this.loadAdminId();
  }
  public login(credentials: any): Observable<boolean> {
    return this.http.post<string>(environment.api + '/api/Sessions', credentials).pipe(

      tap(token => this.token = token),
      tap(token => this.saveToken(token)),
      map(token => true),
      catchError(() => of(false)),

    );
  }
  public saveAdminId(credentials:any): Observable<boolean> {
    console.log(credentials)

     return this.http.post<string>(environment.api + '/api/Sessions/GetId', credentials).pipe(
      tap(token => this.adminId  = token),
      tap(token => this.saveId(token)),
       map(token => true),
       catchError(() => of(false)),
    )

  }
  public logout(): void {
    this.token = null;
    this.adminId = null;
    sessionStorage.removeItem('adminId');
    sessionStorage.removeItem('token');
  }

  public get authenticated(): boolean {
    return !!this.token && !this.jwt.isTokenExpired(this.token);
  }

  private saveToken(token: string): void {
    sessionStorage.setItem('token', token);
  }
  private saveId(id:string):void{
    sessionStorage.setItem('adminId', id);
    console.log("saved");
  }

  private loadToken(): string|null {
    return sessionStorage.getItem('token');
  }
  private loadAdminId(): string|null {
    return sessionStorage.getItem('adminId');
  }
}
