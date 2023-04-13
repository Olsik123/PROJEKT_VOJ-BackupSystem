import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Client} from "../../modules/client";
import {ClientService} from "../../services/client.service";
import {Config} from "../../modules/config";

@Component({
  selector: 'app-client-table',
  templateUrl: './client-table.component.html',
  styleUrls: ['./client-table.component.scss']
})
export class ClientTableComponent implements OnInit {


  @Input()
  public clients: Client[] = [];

  @Output()
  public selected: EventEmitter<Client> = new EventEmitter<Client>();


  public SearchedClient: string='';
  public SearchedMAC:string='';
  public SearchedIP:string='';
  public SearchedVerify:string='';
  public SearchedBlock:string='';

  constructor(private ClientService:ClientService) {
    this.ClientService.GetAllClients().subscribe(result => this.clients = result);
  }

  ngOnInit(): void {
  }


  public clicked(client:Client): void {
    this.selected.emit(client);
  }


}
