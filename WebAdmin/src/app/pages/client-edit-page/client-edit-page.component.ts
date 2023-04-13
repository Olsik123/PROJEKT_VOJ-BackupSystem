import { Component, OnInit } from '@angular/core';
import {Client} from "../../modules/client";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {ClientService} from "../../services/client.service";

@Component({
  selector: 'app-client-edit-page',
  templateUrl: './client-edit-page.component.html',
  styleUrls: ['./client-edit-page.component.scss']
})
export class ClientEditPageComponent implements OnInit {


  public client: Client;

  public form: FormGroup;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private fb: FormBuilder,
              private service: ClientService) {
  }

  ngOnInit(): void {
    let id = +this.route.snapshot.params['id'];

      this.service.findById(id).subscribe(client => {
      this.client= client[0];
      console.log(client[0].alias)
      this.form = this.createForm(this.client);
    });
  }

  private createForm(client: Client): FormGroup {
    return this.fb.group({
      alias: [ client.alias, Validators.required ],
      configIds: this.fb.array(client.configIds),
      mac: [ client.mac, Validators.required ],
      ip: [ client.ip, Validators.required ],
      verified: [ client.verified, Validators.required ],
      ban: [client.ban, Validators.required]
    });
  }

  public save(): void {
    this.form.value.configIds = this.ConfigIdsBack(this.form.value.configIds);
    Object.assign(this.client, this.form.value);
    console.log(this.form.value)

    this.service.save(this.client).subscribe(client => {
      this.router.navigate([ 'clients' ])
    });
  }
  private ConfigIdsBack(configIds: string[]) : number[] {

    let clientNumberIds : number[] = [];

    for (let id of configIds) {
      clientNumberIds.push(parseInt(id));
    }

    return clientNumberIds;
  }
}
