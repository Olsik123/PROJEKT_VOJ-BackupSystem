import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup} from "@angular/forms";

@Component({
  selector: 'app-admin-settings',
  templateUrl: './admin-settings.component.html',
  styleUrls: ['./admin-settings.component.scss']
})
export class AdminSettingsComponent implements OnInit {
  @Input()
  public form: FormGroup;

  @Output()
  public saved: EventEmitter<void> = new EventEmitter<void>();
  constructor(private fb: FormBuilder) {
  }

  ngOnInit(): void {
  }

  public submit(): void {
    console.log(this.form.value)
    if (this.form.valid) this.saved.emit();
    else {
      console.log('ugabuga');
    }
  }


}
