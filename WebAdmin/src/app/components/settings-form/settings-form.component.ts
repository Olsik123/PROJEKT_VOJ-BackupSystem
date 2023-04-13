import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-settings-form',
  templateUrl: './settings-form.component.html',
  styleUrls: ['./settings-form.component.scss'],
})
export class SettingsFormComponent implements OnInit {
  @Input()
  public form: FormGroup;

  @Output()
  public saved: EventEmitter<void> = new EventEmitter<void>();

  constructor() {}

  ngOnInit(): void { console.log(this.form);}


}
