import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
interface Report{
  
}

@Component({
  selector: 'app-smr-rpt-salesorder-report',
  templateUrl: './smr-rpt-salesorder-report.component.html',
  styleUrls: ['./smr-rpt-salesorder-report.component.scss']
})
export class SmrRptSalesorderReportComponent {

  report_list: any [] = [];
  dashboard_list: any [] = [];
  report! : Report;
  responsedata: any;
  constructor(private formBuilder: FormBuilder,private router:Router, private ToastrService: ToastrService, public service: SocketService, private route: ActivatedRoute) {
    this.report = {} as Report;
  }

  ngOnInit(): void {
    this.GetSalesOrderSummary();

  }
  GetSalesOrderSummary(){
    // debugger
    //       var api = 'SmrRptSalesOrderReport/GetSalesOrderSummary';
    //       this.service.get(api).subscribe((result:any) => {
    //         $('#report_list').DataTable().destroy();
    //         this.responsedata = result;
    //         this.report_list = this.responsedata.report_list;
    //         setTimeout(()=>{  
    //           $('#report_list').DataTable();
    //         }, 1);
    //       });
        
        }
}
