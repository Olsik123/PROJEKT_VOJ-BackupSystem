import { Component, OnInit } from '@angular/core';
import { ReportService} from "../../services/report.service";
import {Report} from "../../modules/report";

@Component({
  selector: 'app-report-page',
  templateUrl: './report-page.component.html',
  styleUrls: ['./report-page.component.scss']
})
export class ReportPageComponent implements OnInit {

  reports;
  public SearchedConfig: string='';
  public SearchedClient: string='';
  public SearchedReportSuccess: string = "0";


  constructor(private ReportService:ReportService) {
    this.reports= this.ReportService.GetAllReports();
  }





  ngOnInit(): void {
  }

}
