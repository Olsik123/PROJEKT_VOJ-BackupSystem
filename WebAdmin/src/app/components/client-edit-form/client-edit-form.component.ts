import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormArray, FormBuilder, FormControl, FormGroup} from "@angular/forms";
import {ConfigService} from "../../services/config.service";
import {Config} from "../../modules/config";

export interface Tile {
  color: string;
  cols: number;
  rows: number;
  text: string;
}
@Component({
  selector: 'app-client-edit-form',
  templateUrl: './client-edit-form.component.html',
  styleUrls: ['./client-edit-form.component.scss']
})
export class ClientEditFormComponent implements OnInit {

  @Input()
  public form: FormGroup;
  @Output()
  public saved: EventEmitter<void> = new EventEmitter<void>();


  value:string;

  configs : Config[];
  public SearchedConfig: string='';
  constructor(private fb: FormBuilder,private ConfigService:ConfigService) {
    this.ConfigService.GetAllConfigs().subscribe(result => this.configs = result);

  }
  tiles: Tile[] = [
    {text: 'One', cols: 1, rows: 1, color: 'lightblue'},
    {text: 'Two', cols: 2, rows: 2, color: 'lightgreen'},
    {text: 'Three', cols: 1, rows: 1, color: 'lightpink'},
    {text: 'Four', cols: 1, rows: 1, color: '#DDBDF1'},
    {text: 'Four', cols: 1, rows: 1, color: '#DDBDF1'},
    {text: 'Four', cols: 1, rows: 1, color: '#DDBDF1'},
    {text: 'Fove', cols: 2, rows: 1, color: '#DDBDF1'},
  ];

  ngOnInit(): void {
    console.log(this.form.value)
  }
  addConfig(event: any) {
    const formArr: FormArray = this.form.get('configIds') as FormArray;

    if (event.target.checked) formArr.push(new FormControl(event.target.value));
    else {
      let index : number = 0;

      for (let x of formArr.controls) {
        if (x.value == event.target.value) formArr.removeAt(index);

        index++;
      }
    }

  }
  public checkConfigs(id :number)
  {
    const formArr: any = this.form.get('configIds')?.value;
    return formArr.includes(id)
  }

  public submit(): void {
    if (this.form.valid)
      this.saved.emit();
  }
}



