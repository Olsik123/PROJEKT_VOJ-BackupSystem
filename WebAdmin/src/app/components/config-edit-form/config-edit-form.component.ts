import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  FormGroup,
  FormControl,
  FormArray,
  AbstractControl,
  FormBuilder,
  Validators,
} from '@angular/forms';
import { ClientService } from '../../services/client.service';
import { Destination } from 'src/app/modules/config';
import { Client } from '../../modules/client';
import { Event } from '@angular/router';

@Component({
  selector: 'app-config-edit-form',
  templateUrl: './config-edit-form.component.html',
  styleUrls: ['./config-edit-form.component.scss']
})
export class ConfigEditFormComponent implements OnInit {
  @Input()
  public form: FormGroup;

  @Output()
  public saved: EventEmitter<void> = new EventEmitter<void>();


  clients: Client[];
  value: string;
  tempClient: Client;
  constructor(private fb: FormBuilder, private ClientService: ClientService) {
    this.ClientService.GetVerifiedAndUnblocked().subscribe(result => (this.clients = result));
  }
  ngOnInit(): void {
  }
  addClient(event: any) {
    const formArr: FormArray = this.form.get('clientIds') as FormArray;

    if (event.target.checked) formArr.push(new FormControl(event.target.value));
    else {
      let index : number = 0;

      for (let x of formArr.controls) {
        if (x.value == event.target.value) formArr.removeAt(index);

        index++;
      }
    }

  }




  public checkClients(id :number)
  {
    const formArr: any = this.form.get('clientIds')?.value;
    return formArr.includes(id)
  }
  public static createSourceControl(
    fb: FormBuilder,
    source: string
  ): FormControl {
    return fb.control(source);
  }
  public static createClientControl(
    fb: FormBuilder,
    client: string
  ): FormControl {
    return fb.control(client);
  }
  public static createClientIdControl(
    fb: FormBuilder,
    clientId: number
  ): FormControl {
    return fb.control(clientId);
  }
  public saveFrequency(fb: FormBuilder, clientId: number): FormControl {
    return fb.control(clientId);
  }
  public static createDestinationControl(
    fb: FormBuilder,
    destination: Destination
  ): FormGroup {
    return fb.group({
      path: [destination.path, Validators.required],
      place: [destination.place, Validators.required],
      host: [destination.host],
    });
  }

  public get sources(): FormControl[] {
    return (this.form.get('sources') as FormArray).controls as FormControl[];
  }
  public get destinations(): FormControl[] {
    return (this.form.get('destinations') as FormArray)
      .controls as FormControl[];
  }

  public submit(): void {
    if (this.form.valid) this.saved.emit();
    else {
      console.log('ugabuga');
    }
  }

  public addSource(): void {
    let array = this.form.get('sources') as FormArray;
    array.push(ConfigEditFormComponent.createSourceControl(this.fb, ''));
  }

  public removeSource(i: number): void {
    let array = this.form.get('sources') as FormArray;
    if (array.length != 1) {
      array.removeAt(i);
    }
  }

  public addDestination(): void {
    let array = this.form.get('destinations') as FormArray;
    array.push(
      ConfigEditFormComponent.createDestinationControl(this.fb, {
        place: '',
        path: '',
        host: '',
      })
    );
  }

  public removeDestination(i: number): void {
    let array = this.form.get('destinations') as FormArray;
    if (array.length != 1) {
      array.removeAt(i);
    }
  }
}


