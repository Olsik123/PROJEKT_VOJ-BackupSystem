import { Component, OnInit } from '@angular/core';
import {ConfigService} from "../../services/config.service";
import {Config} from "../../modules/config";
import {Router} from "@angular/router";
import {config, Observable} from "rxjs";

@Component({
  selector: 'app-config-page',
  templateUrl: './config-page.component.html',
  styleUrls: ['./config-page.component.scss']
})
export class ConfigPageComponent implements OnInit {


  configs : Config[];
  public SearchedConfig: string='';
  constructor(private router: Router,
              private ConfigService:ConfigService) {
    this.ConfigService.GetAllConfigs().subscribe(result => this.configs = result);
  }

  ngOnInit(): void {
  }

  public edit(config:Config ): void {
    this.router.navigate([ 'configs', config.id ]);
  }

}
