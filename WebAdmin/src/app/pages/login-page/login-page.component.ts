import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {SessionsService} from "../../services/sessions.service";
import {Router} from "@angular/router";
import {filter} from "rxjs";
import {ThisReceiver} from "@angular/compiler";


@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent implements OnInit {


  public form: FormGroup;

  constructor(private fb: FormBuilder,
              private router: Router,
              private service: SessionsService) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      login: [ '', Validators.required ],
      password: [ '', Validators.required ],
    });
  }

  public login(): void {
    debugger
    this.service.saveAdminId(this.form.value).subscribe();
    this.service.login(this.form.value).pipe(
      filter(result => result === true)
    ).subscribe(() => this.router.navigate(['homepage']));
  }
}
