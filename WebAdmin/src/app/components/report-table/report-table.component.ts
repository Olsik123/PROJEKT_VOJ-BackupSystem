import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Report} from "../../modules/report";
import { ReportService} from "../../services/report.service";
import {Config} from "../../modules/config";
import {MatDialog} from '@angular/material/dialog';
import {DialogComponent} from "../dialog/dialog.component";

@Component({
  selector: 'app-report-table',
  templateUrl: './report-table.component.html',
  styleUrls: ['./report-table.component.scss']
})
export class ReportTableComponent implements OnInit {
  @Input()
  public reports : Report[] =[];
  @Output()
  public selected: EventEmitter<Report> = new EventEmitter<Report>();
  public SearchedConfig: string='';
  public SearchedClient: string='';
  public status:number=0;
  public dateto:string;
  public datefrom:string;


  constructor(private ReportService:ReportService,public dialog: MatDialog) {
    this.ReportService.GetAllReports().subscribe(result => this.reports = result);
  }




  ngOnInit(): void {
  }
  public sendRequest(): void {
    console.log("ss")
    debugger

    if (this.status == 0 )
    {
      if(this.datefrom == null || this.dateto == null)
      {
        this.ReportService.GetAllReports().subscribe(result => this.reports = result);
      }
      else{
        this.ReportService.GetAllReportWithParametersDate(this.dateto,this.datefrom).subscribe(result => this.reports = result);
      }

    }
    else {
      if(this.datefrom == null || this.dateto == null)
      {
        if (this.status == 1)
          this.ReportService.GetAllReportWithParametersStatus(true).subscribe(result => this.reports = result);
        else
          this.ReportService.GetAllReportWithParametersStatus(false).subscribe(result => this.reports = result);
      }
      else{
        if (this.status == 1)
        this.ReportService.GetAllReportWithParametersDateStatus(this.dateto,this.datefrom,true).subscribe(result => this.reports = result);
        else
          this.ReportService.GetAllReportWithParametersDateStatus(this.dateto,this.datefrom,false).subscribe(result => this.reports = result);
      }
    }

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

