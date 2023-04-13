import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import { Config} from "../../modules/config";
import { ConfigService} from "../../services/config.service";
import { FormArray } from '@angular/forms';
import { ConfigNewFormComponent } from 'src/app/components/config-new-form/config-new-form.component';

@Component({
  selector: 'app-config-edit-page',
  templateUrl: './config-edit-page.component.html',
  styleUrls: ['./config-edit-page.component.scss']
})
export class ConfigEditPageComponent implements OnInit {

  public config: Config;
  public CronArray:string[];
  public form: FormGroup;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private fb: FormBuilder,
              private service: ConfigService) {
  }

  ngOnInit(): void {
    let id = +this.route.snapshot.params['id'];

    this.service.findById(id).subscribe(config => {
      this.SplitCron(config.frequency)
      this.config = config;
      this.form = this.createForm(this.config);
    });
  }

  private SplitCron(frequency:any)
  {
     this.CronArray = frequency.split(" ")
  }

  private createForm(config: Config): FormGroup {
    return this.fb.group({
      alias: [config.alias, Validators.required],
      clientIds: this.fb.array(config.clientIds),
      format: [config.format, Validators.required],
      type: [config.type, Validators.required],
      frequency: this.fb.group({
        minutes: this.CronArray[0],
        hours: this.CronArray[1],
        dayOfMonth: this.CronArray[2],
        months: this.CronArray[3],
        dayOfWeek: this.CronArray[4],
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

    this.service.save(this.config).subscribe(config => {
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
