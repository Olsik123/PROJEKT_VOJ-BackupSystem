import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Config } from '../../modules/config';
import { ConfigService } from '../../services/config.service';
import { FormArray } from '@angular/forms';
import { ConfigNewFormComponent } from '../../components/config-new-form/config-new-form.component';

@Component({
  selector: 'app-config-new-page',
  templateUrl: './config-new-page.component.html',
  styleUrls: ['./config-new-page.component.scss'],
})
export class ConfigNewPageComponent implements OnInit {
  public config: Config = new Config();
  public form: FormGroup;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private service: ConfigService
  ) {}

  ngOnInit(): void {
    this.form = this.createForm(this.config);
  }
  private createForm(config: Config): FormGroup {
    return this.fb.group({
      alias: [config.alias, Validators.required],
      //clients : [config.clients, Validators.required], array
      clientIds: this.fb.array(config.clientIds),
      format: [config.format, Validators.required],
      type: [config.type, Validators.required],
      frequency: this.fb.group({
        minutes: ["*"],
        hours: ["*"],
        dayOfMonth: ["*"],
        months: ["*"],
        dayOfWeek: ["*"],
      }),
      retention: [config.retention, Validators.required],
      packages: [config.packages, Validators.required],
      sources: this.fb.array(
        config.sources.map((x) =>
          ConfigNewFormComponent.createSourceControl(this.fb, x)
        )
      ),
      destinations: this.fb.array(
        config.destinations.map((x) =>
          ConfigNewFormComponent.createDestinationControl(this.fb, x)
        )
      ),
    });
  }
  public save(): void {

    this.form.value.frequency = this.pareseCronBack(this.form.value.frequency);
    this.form.value.clientIds = this.clientIdsBack(this.form.value.clientIds);
    this.form.value.adminId = 22;

    Object.assign(this.config, this.form.value);

    console.log(this.config);

    this.service.save(this.config).subscribe(() => {
      this.router.navigate(['configs']);
    });
  }

  private pareseCronBack(cronGroup : any) : string {

    let cron : string = '';

    for (let key of Object.keys(cronGroup)) {
      cron += cronGroup[key] + ' ';
    }

    return cron.trim();
  }

  private clientIdsBack(clientIds : string[]) : number[] {

    let clientNumberIds : number[] = [];

    for (let id of clientIds) {
      clientNumberIds.push(parseInt(id));
    }

    return clientNumberIds;
  }


}
