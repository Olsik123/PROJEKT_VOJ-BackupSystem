import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { ReportService} from "../../services/report.service";
import {Report} from "../../modules/report";
import {MatDialog} from '@angular/material/dialog';
import {DialogComponent} from "../../components/dialog/dialog.component";


@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit {

  @Input()
  public reports : Report[] =[];
  @Output()
  public selected: EventEmitter<Report> = new EventEmitter<Report>();


  constructor(private ReportService:ReportService,public dialog: MatDialog) {
    this.ReportService.GetAllErrors().subscribe(result => this.reports = result);
  }

  ngOnInit(): void {
  }
  openDialog(message:string) {
    const dialogRef = this.dialog.open(DialogComponent,{
        disableClose:false,
        data: message
      })

    ;

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

}
