import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {Config} from "../../modules/config";
import {ConfigNewFormComponent} from "../../components/config-new-form/config-new-form.component";
import {ActivatedRoute, Router} from "@angular/router";
import {ConfigService} from "../../services/config.service";
import {Setting} from "../../modules/setting";
import {SettingsService} from "../../services/settings.service";

@Component({
  selector: 'app-settings-page',
  templateUrl: './settings-page.component.html',
  styleUrls: ['./settings-page.component.scss']
})
export class SettingsPageComponent implements OnInit {

  public setting: Setting;
  public CronArray:string[];
  public form: FormGroup;
  public idn:number;
  public id:string|null;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private fb: FormBuilder,
              private service: SettingsService) {
  }

  ngOnInit(): void {
    let id = sessionStorage.getItem('adminId')
    console.log(id)
    this.service.findById(id).subscribe(setting => {
      this.setting = setting;
      console.log(this.setting)
      this.form = this.createForm(this.setting);
    });
  }
  private createForm(setting:Setting): FormGroup {
    return this.fb.group({
      id: [setting.id, Validators.required],
      password: [setting.password, Validators.required],
      email: [setting.email, Validators.required],
      frequency: [setting.frequency, Validators.required],
    });
  }
  public save(): void {
    this.id = sessionStorage.getItem('adminId')
    this.idn = Number(this.id)
    console.log(this.form.value)
    this.form.value.id = this.idn
    console.log(this.form.value)
    Object.assign(this.setting, this.form.value);

    console.log(this.setting);

    this.service.save(this.setting).subscribe(() => {
      this.router.navigate(['homepage']);
    });
  }


}
