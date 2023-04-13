import { Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Config} from "../../modules/config";
import {ConfigService} from "../../services/config.service";

@Component({
  selector: 'app-config-table',
  templateUrl: './config-table.component.html',
  styleUrls: ['./config-table.component.scss']
})
export class ConfigTableComponent implements OnInit {

  @Input()
  public configs: Config[] = [];

  @Output()
  public selected: EventEmitter<Config> = new EventEmitter<Config>();

  public SearchedConfig: string='';
  constructor(private ConfigService:ConfigService) {
    this.ConfigService.GetAllConfigs().subscribe(result => this.configs = result);
  }

  ngOnInit(): void {

  }
  public clicked(config:Config): void {
    this.selected.emit(config);
  }
  public Delete(config:Config):void{
    this.ConfigService.delete(config).subscribe();
  }


}
