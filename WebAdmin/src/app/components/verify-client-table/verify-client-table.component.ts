import { Component, OnInit } from '@angular/core';
import { Client } from '../../modules/client';
import { ClientService } from '../../services/client.service';

@Component({
  selector: 'app-verify-client-table',
  templateUrl: './verify-client-table.component.html',
  styleUrls: ['./verify-client-table.component.scss'],
})
export class VerifyClientTableComponent implements OnInit {
  clients: Client[];
  public SearchedClient: string = '';
  public SearchedMAC: string = '';
  public SearchedIP: string = '';
  public SearchedVerify: string = '';

  constructor(private ClientService: ClientService) {
    this.ClientService.GetUnverified().subscribe(
      (result) => (this.clients = result)
    );
  }
  ngOnInit(): void {}

  Trust(id:number):void{
    this.ClientService.AddTrust(id).subscribe()
  }
}
