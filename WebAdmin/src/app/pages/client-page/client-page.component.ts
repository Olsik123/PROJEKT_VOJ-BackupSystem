import { Component, OnInit } from '@angular/core';
import {Config} from "../../modules/config";
import {Router} from "@angular/router";
import {Client} from "../../modules/client";
import {ClientService} from "../../services/client.service";

@Component({
  selector: 'app-client-page',
  templateUrl: './client-page.component.html',
  styleUrls: ['./client-page.component.scss']
})
export class ClientPageComponent implements OnInit {

  clients : Client[];
  constructor(private router: Router,
              private service:ClientService) {
    this.service.GetAllClients().subscribe(result => this.clients = result);
  }

  ngOnInit(): void {
  }

  public edit(client:Client ): void {
    this.router.navigate([ 'clients', client.id ]);
  }

}
