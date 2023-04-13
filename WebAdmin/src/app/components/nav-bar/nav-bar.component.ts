import { Component, OnInit } from '@angular/core';
import { DatePipe } from "@angular/common";
import {Router} from "@angular/router";
import {SessionsService} from "../../services/sessions.service";


@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  DateNumber : number;
  date : Date;

  constructor(public datepipe : DatePipe,
              private router: Router,
              private sessions: SessionsService) {
    setInterval(()=> {this.date=new Date()},1000)
  }
  public get authenticated(): boolean {
    return this.sessions.authenticated;
  }
  ngOnInit(): void {
  }

  GetDate() {
    return this.datepipe.transform(this.date,'dd.MM.yyyy')
  }
  GetTime() {
    return this.datepipe.transform(this.date,'HH:mm')
  }
  public logout(): void {
    this.sessions.logout();
    this.router.navigate([ 'login' ]);
  }
}

